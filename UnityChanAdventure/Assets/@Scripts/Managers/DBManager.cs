using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;
using Firebase.Database;

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
        DatabaseReference accountRef = reference.Child("Account").Child(accountNumber);

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
        DatabaseReference accountRef = reference.Child("Account").Child(accountNumber);

        // 계정 데이터 생성
        Dictionary<string, object> accountData = new Dictionary<string, object>
    {
        { "AccountNumber", accountNumber },
        { "AccountPW", accountPassword }
        // 추가 필드 및 값을 여기에 추가할 수 있습니다.
    };

        // 계정 데이터를 Firebase에 쓰기 전 중복 여부 확인
        accountRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error checking account duplication: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // 이미 해당 계정 번호가 존재하는 경우
                if (snapshot.Exists)
                {
                    Managers.Event.PostNotification(Define.EVENT_TYPE.CreateAccount_Fail_IDSame, null);
                    Debug.Log("Account number already exists.");
                    return;
                }

                // 계정 데이터를 Firebase에 쓰기
                accountRef.SetValueAsync(accountData).ContinueWith(createTask =>
                {
                    if (createTask.IsFaulted)
                    {
                        Debug.LogError("Error creating account: " + createTask.Exception);
                        return;
                    }

                    if (createTask.IsCompleted)
                    {
                        Debug.Log("Account created successfully.");
                        Managers.Event.PostNotification(Define.EVENT_TYPE.CreateAccount_Sucess, null);
                       
                        
                        // 새로운 DB 생성해서 신규캐릭터 맞이하기
                        
                        CreateCharacter(accountNumber, 100);
                        CreateInventory(accountNumber);
                    }
                });
            }
        });
    }

    #region 신규 캐릭터 처리
    private void CreateCharacter(string accountNumber, int charcode)
    {
       // Character 데이터 생성
      // 원래 데이터 처리는 Server에서 해야 안전
      // 공용데이터는 따로 저장할 필요 없을듯? ( prefab path 등 ) 

        Dictionary<string, object> characterData = new Dictionary<string, object>
        {
           { "charcode", charcode },
           { "level", Managers.Data.CharacterDataDict[charcode].level },
           //{ "iconPath", Managers.Data.CharacterDataDict[charcode].iconPath },
           //{ "prefabPath", Managers.Data.CharacterDataDict[charcode].prefabPath }
        };
        // Character 데이터를 Firebase에 쓰기
        DatabaseReference characterRef = reference.Child("Account").Child(accountNumber).Child("Character").Child(charcode.ToString());
        characterRef.UpdateChildrenAsync(characterData).ContinueWith(task =>
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



    private void CreateInventory(string accountNumber)
    {
        // Character 데이터 생성
        // 원래 데이터 처리는 Server에서 해야 안전


        Dictionary<string, object> itemdata = new Dictionary<string, object>();

        foreach (var i in Managers.Data.ItemDataDict.Keys)
        {

            itemdata.Add("itemcode", Managers.Data.ItemDataDict[i].itemcode);
            itemdata.Add("count", Managers.Data.ItemDataDict[i].count);
            itemdata.Add("Enhancement", 0);

            DatabaseReference characterRef = reference.Child("Account").Child(accountNumber).Child("Items").Child(i.ToString());
            characterRef.UpdateChildrenAsync(itemdata).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error creating character: " + task.Exception);
                    return;
                }

                if (task.IsCompleted)
                {
                    Debug.Log("Item Inven Created successfully.");
                }
            });
            itemdata.Clear();

        }  
    }

    #endregion

    #region 데이터 정보 불러오기
    public void FetchAccountData(string accountNumber)
    {
        DatabaseReference accountRef = reference.Child("Account").Child(accountNumber);

        accountRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving account data: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    // 데이터가 존재하는 경우
                    string accountPW = snapshot.Child("AccountPW").Value.ToString();
                    Debug.Log("Account Password: " + accountPW);
                    // 데이터 처리 코드 추가
                }
                else
                {
                    // 데이터가 존재하지 않는 경우
                    Debug.Log("Account data does not exist.");
                }
            }
        });
    }
    public void FetchCharacterData(string accountNumber, int charcode)
    {
        DatabaseReference characterRef = reference.Child("Account").Child(accountNumber).Child("Character").Child(charcode.ToString());

        characterRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving character data: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    // 데이터가 존재하는 경우
                    int level = int.Parse(snapshot.Child("level").Value.ToString());
                    Debug.Log("Character Level: " + level);
                    // 데이터 처리 코드 추가
                }
                else
                {
                    // 데이터가 존재하지 않는 경우
                    Debug.Log("Character data does not exist.");
                }
            }
        });
    }
    public void FetchAllItemData(string accountNumber)
    {
        DatabaseReference itemsRef = reference.Child("Account").Child(accountNumber).Child("Items");

        itemsRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving item data: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot itemSnapshot in snapshot.Children)
                {
                    // 아이템 데이터 추출
                    int itemCode = int.Parse(itemSnapshot.Key);
                    int count = int.Parse(itemSnapshot.Child("count").Value.ToString());

                    // 추출한 데이터 활용
                    Debug.Log("Item: itemCode = " + itemCode + ", count = " + count);
                }
                Debug.Log("추후 여기 위쪽에 데이터 동기화 과정 넣어야 합니다.");
                Debug.Log("그냥 불러온 데이터를 itemcode에 맞게 내 Data나 inventory에 넣어주면 됨");
            }
        });
    }
    #endregion
    public void UpdateItem(string accountNumber, int itemCode, int acquireCount, Define.UpdateDataTyoe updateType=Define.UpdateDataTyoe.UpdateDB_ItemCount)
    {
        DatabaseReference itemRef = reference.Child("Account").Child(accountNumber).Child("Items").Child(itemCode.ToString());

        itemRef.RunTransaction(transaction =>
        {
            if (transaction.Value != null)
            {
                switch (updateType)
                {
                    case Define.UpdateDataTyoe.UpdateDB_ItemCount:
                        int currentCount = int.Parse(transaction.Child("count").Value.ToString());
                        transaction.Child("count").Value = $"{currentCount+ acquireCount}";
                        Debug.Log($"아이템 개수 변경 :: 변경전 = {currentCount} 변경 후 = {currentCount + acquireCount}");
                        break;
                    case Define.UpdateDataTyoe.UpdateDB_ItemEnhance:
                        int currentEnhance = int.Parse(transaction.Child("Enhancement").Value.ToString());
                        transaction.Child("Enhancement").Value = $"{currentEnhance + acquireCount}";
                        Debug.Log($"아이템 강화수치 변경 ::변경전 = {currentEnhance} 변경 후 = {currentEnhance + acquireCount}");
                        break;
                }

                return TransactionResult.Success(transaction);
            }
            return TransactionResult.Abort();
        }).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error updating item: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Item updated successfully.");
            }
        });
    }
}
