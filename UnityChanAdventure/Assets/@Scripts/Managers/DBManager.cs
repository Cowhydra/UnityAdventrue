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
        DatabaseReference accountRef = reference.Child("Account").Child(accountNumber);

        // ���� ������ ����
        Dictionary<string, object> accountData = new Dictionary<string, object>
    {
        { "AccountNumber", accountNumber },
        { "AccountPW", accountPassword }
        // �߰� �ʵ� �� ���� ���⿡ �߰��� �� �ֽ��ϴ�.
    };

        // ���� �����͸� Firebase�� ���� �� �ߺ� ���� Ȯ��
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

                // �̹� �ش� ���� ��ȣ�� �����ϴ� ���
                if (snapshot.Exists)
                {
                    Managers.Event.PostNotification(Define.EVENT_TYPE.CreateAccount_Fail_IDSame, null);
                    Debug.Log("Account number already exists.");
                    return;
                }

                // ���� �����͸� Firebase�� ����
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
                       
                        
                        // ���ο� DB �����ؼ� �ű�ĳ���� �����ϱ�
                        
                        CreateCharacter(accountNumber, 100);
                        CreateInventory(accountNumber);
                    }
                });
            }
        });
    }

    #region �ű� ĳ���� ó��
    private void CreateCharacter(string accountNumber, int charcode)
    {
       // Character ������ ����
      // ���� ������ ó���� Server���� �ؾ� ����
      // ���뵥���ʹ� ���� ������ �ʿ� ������? ( prefab path �� ) 

        Dictionary<string, object> characterData = new Dictionary<string, object>
        {
           { "charcode", charcode },
           { "level", Managers.Data.CharacterDataDict[charcode].level },
           //{ "iconPath", Managers.Data.CharacterDataDict[charcode].iconPath },
           //{ "prefabPath", Managers.Data.CharacterDataDict[charcode].prefabPath }
        };
        // Character �����͸� Firebase�� ����
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
        // Character ������ ����
        // ���� ������ ó���� Server���� �ؾ� ����


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

    #region ������ ���� �ҷ�����
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
                    // �����Ͱ� �����ϴ� ���
                    string accountPW = snapshot.Child("AccountPW").Value.ToString();
                    Debug.Log("Account Password: " + accountPW);
                    // ������ ó�� �ڵ� �߰�
                }
                else
                {
                    // �����Ͱ� �������� �ʴ� ���
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
                    // �����Ͱ� �����ϴ� ���
                    int level = int.Parse(snapshot.Child("level").Value.ToString());
                    Debug.Log("Character Level: " + level);
                    // ������ ó�� �ڵ� �߰�
                }
                else
                {
                    // �����Ͱ� �������� �ʴ� ���
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
                    // ������ ������ ����
                    int itemCode = int.Parse(itemSnapshot.Key);
                    int count = int.Parse(itemSnapshot.Child("count").Value.ToString());

                    // ������ ������ Ȱ��
                    Debug.Log("Item: itemCode = " + itemCode + ", count = " + count);
                }
                Debug.Log("���� ���� ���ʿ� ������ ����ȭ ���� �־�� �մϴ�.");
                Debug.Log("�׳� �ҷ��� �����͸� itemcode�� �°� �� Data�� inventory�� �־��ָ� ��");
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
                        Debug.Log($"������ ���� ���� :: ������ = {currentCount} ���� �� = {currentCount + acquireCount}");
                        break;
                    case Define.UpdateDataTyoe.UpdateDB_ItemEnhance:
                        int currentEnhance = int.Parse(transaction.Child("Enhancement").Value.ToString());
                        transaction.Child("Enhancement").Value = $"{currentEnhance + acquireCount}";
                        Debug.Log($"������ ��ȭ��ġ ���� ::������ = {currentEnhance} ���� �� = {currentEnhance + acquireCount}");
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
