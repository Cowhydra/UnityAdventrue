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
            // ���� ��ü�� ����
            if (task.IsFaulted)
            {
                // Firebase ���� �� ��Ʈ��ũ Ȥ�� ������ ��� Ȯ�� �ʿ�!
                throw new Exception("Error retrieving account data");
                
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // accountRef�� ���� ����� ���� ��
                if (snapshot.Exists)
                {
                    // ID�� ��ġ�ϴ� ���
                    string storedPassword = snapshot.Child("AccountPW").GetValue(true).ToString();
                    if (password == storedPassword)
                    {
                        Debug.Log("Login Successful");
                        Managers.Event.PostNotification(Define.EVENT_TYPE.LoginSucess,null);
                        // �α��� ���� �� ó�� ���� �߰�
                        Debug.Log("�α��� ������ ����-> �� �ѱ�� DataBase���� �޾ƿ��� �� ");
                    }
                    else
                    {
                        Debug.Log("Wrong Password");
                        Managers.Event.PostNotification(Define.EVENT_TYPE.LoginFail_PW_Wrong, null);
                        // ��й�ȣ�� ��ġ���� �ʴ� ��� ó�� ���� �߰�
                    }
                }
                else
                {
                    Managers.Event.PostNotification(Define.EVENT_TYPE.LoginFail_ID_NotFound, null);
                    Debug.Log("ȸ������ ��Ű��: ���â UI ����");
                    // ȸ������ ó�� ���� �߰�
                }
            }
        });
    }

    public void CreateAccount(string accountNumber, string accountPassword)
    {
        // ���� ������ ����
        Dictionary<string, object> accountData = new Dictionary<string, object>
    {
        { "AccountNumber", accountNumber },
        { "AccountPW", accountPassword }
        // �߰� �ʵ� �� ���� ���⿡ �߰��� �� �ֽ��ϴ�.
    };

        // ���� �����͸� Firebase�� ���� �� �ߺ� ���� Ȯ��
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

                // �̹� �ش� ���� ��ȣ�� �����ϴ� ���
                if (snapshot.Exists)
                {
                    Managers.Event.PostNotification(Define.EVENT_TYPE.CreateAccount_Fail_IDSame,null);
                    Debug.Log("Account number already exists.");
                    return ;
                }

                // ���� �����͸� Firebase�� ����
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
                        // ���ο� ĳ���� ����
                        int charcode = 100;
                        CreateCharacter(accountNumber, charcode);
                    }
                });
            }
        });
    }
    private void CreateCharacter(string accountNumber, int charcode)
    {
        // Character ������ ����
        //���� ������ ó���� Server���� �ؾ� ����
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



        //    // �߰� �ʵ� �� ���� ���⿡ �߰��� �� �ֽ��ϴ�.
        //};

        // Character �����͸� Firebase�� ����
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

    //Write DB �������� ���� �������� �ؾ���
    public void WriteDB()
    {

    }
}
