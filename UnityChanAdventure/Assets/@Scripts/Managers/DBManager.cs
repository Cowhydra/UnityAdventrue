using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;
using Firebase.Database;
using static Cinemachine.DocumentationSortingAttribute;

public class DBManager
{

    public string DBurl = "https://unitychanadventure-default-rtdb.firebaseio.com/";
    DatabaseReference reference;

    public void Init()
    {

        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(DBurl);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }



    public void ChecK_Account(string accountNumber, string password)
    {
        DatabaseReference accountRef = reference.Child("Account").Child($"{accountNumber}");

        accountRef.GetValueAsync().ContinueWith(task =>
        {
            // 연결 자체의 실패
            if (task.IsFaulted)
            {
                // Firebase 권한 및 네트워크 혹은 데이터 경로 확인 필요!
                throw new Exception("Error retrieving account data");
                
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // accountRef에 대한 결과가 있을 때
                if (snapshot.Exists)
                {
                    // ID가 일치하는 경우
                    string storedPassword = snapshot.Child("AccountPW").GetValue(true).ToString();
                    if (password == storedPassword)
                    {
                        Debug.Log("Login Successful");
                        Managers.Event.PostNotification(Define.EVENT_TYPE.LoginSucess,null);
                        // 로그인 성공 후 처리 로직 추가
                        Debug.Log("로그인 성공후 로직-> 씬 넘기고 DataBase에서 받아오기 등 ");
                    }
                    else
                    {
                        Debug.Log("Wrong Password");
                        Managers.Event.PostNotification(Define.EVENT_TYPE.LoginFail_PW_Wrong, null);
                        // 비밀번호가 일치하지 않는 경우 처리 로직 추가
                    }
                }
                else
                {
                    Managers.Event.PostNotification(Define.EVENT_TYPE.LoginFail_ID_NotFound, null);
                    Debug.Log("회원가입 시키기: 경고창 UI 띄우기");
                    // 회원가입 처리 로직 추가
                }
            }
        });
    }

    public void CreateAccount(string accountNumber, string accountPassword)
    {
        // 계정 데이터 생성
        Dictionary<string, object> accountData = new Dictionary<string, object>
    {
        { "AccountNumber", accountNumber },
        { "AccountPW", accountPassword }
        // 추가 필드 및 값을 여기에 추가할 수 있습니다.
    };

        // 계정 데이터를 Firebase에 쓰기 전 중복 여부 확인
        DatabaseReference accountRef = reference.Child("Account").Child(accountNumber);
        accountRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error checking account duplication: " + task.Exception);
                return ;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // 이미 해당 계정 번호가 존재하는 경우
                if (snapshot.Exists)
                {
                    Managers.Event.PostNotification(Define.EVENT_TYPE.CreateAccount_Fail_IDSame,null);
                    Debug.Log("Account number already exists.");
                    return ;
                }

                // 계정 데이터를 Firebase에 쓰기
                DatabaseReference newAccountRef = reference.Child("Account").Child(accountNumber);
                newAccountRef.UpdateChildrenAsync(accountData).ContinueWith(createTask =>
                {
                    if (createTask.IsFaulted)
                    {
                        Debug.LogError("Error creating account: " + createTask.Exception);
                        return ;
                    }

                    if (createTask.IsCompleted)
                    {
                        Debug.Log("Account created successfully.");
                        Managers.Event.PostNotification(Define.EVENT_TYPE.CreateAccount_Sucess, null);
                        // 새로운 캐릭터 생성
                        int charcode = 100;
                        CreateCharacter(accountNumber, charcode);
                    }
                });
            }
        });
    }
    private void CreateCharacter(string accountNumber, int charcode)
    {
        // Character 데이터 생성
        //원래 데이터 처리는 Server에서 해야 안전
        //Dictionary<string, object> characterData = new Dictionary<string, object>
        //{
            

        //    { "charcode", charcode },
        //    { "jobType", $"{Managers.Data.CharacterDataDict[charcode].jobType}" },
        //    { "maxhp", Managers.Data.CharacterDataDict[charcode].maxhp },
        //    { "maxmana", Managers.Data.CharacterDataDict[charcode]. maxmana },
        //    { "magicdef",Managers.Data.CharacterDataDict[charcode]. magicdef },
        //    { "def", Managers.Data.CharacterDataDict[charcode].def },
        //    { "magicattack", Managers.Data.CharacterDataDict[charcode].magicattack },
        //    { "attack", Managers.Data.CharacterDataDict[charcode].attack },
        //    { "attackspeed", Managers.Data.CharacterDataDict[charcode].attackspeed },
        //    { "level", Managers.Data.CharacterDataDict[charcode].level },
        //    { "iconPath", Managers.Data.CharacterDataDict[charcode].iconPath },
        //    { "prefabPath", Managers.Data.CharacterDataDict[charcode].prefabPath }



        //    // 추가 필드 및 값을 여기에 추가할 수 있습니다.
        //};

        // Character 데이터를 Firebase에 쓰기
        DatabaseReference characterRef = reference.Child("Character").Child(accountNumber).Child(charcode.ToString());
        characterRef.UpdateChildrenAsync((IDictionary<string, object>)Managers.Data.CharacterDataDict).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error creating character: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                Debug.Log("Character created successfully.");
            }
        });
    }


    public void ReadDB()
    {

    }
    public void ReadAllDB()
    {

    }

    //Write DB 관련일은 원래 서버에서 해야함
    public void WriteDB()
    {

    }
}
