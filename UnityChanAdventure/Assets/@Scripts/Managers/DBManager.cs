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

    //���� ���� �Ϸ�
    //Account - AccountNumber - 130  - AccountPW , Character,Items,Goods, �̷������� ������ ­��� ���µ�
    //������ Account 130 - AccountNumber,AccountPW,CHarcter,Itms,Goods �̷� ����...

    public void Init()
    {

        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(DBurl);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void ChecK_Account(string accountNumber, string password)
    {
        if (accountNumber == string.Empty || password == string.Empty)
        {
            Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginNotBlink);
            return;
        }
        //DatabaseReference accountRef = reference.Child("Account").Child(accountNumber);
        DatabaseReference accountRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber);
        accountRef.GetValueAsync().ContinueWith(task =>
        {
            // ���� ��ü�� ����
            if (task.IsFaulted)
            {
                // Firebase ���� �� ��Ʈ��ũ Ȥ�� ������ ��� Ȯ�� �ʿ�!
                Debug.Log("������");
                
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // accountRef�� ���� ����� ���� ��
                if (snapshot.Exists)
                {
                    // ID�� ��ġ�ϴ� ���
                    string storedPassword = snapshot.Child("AccountPW").Value.ToString();
                    Debug.Log($"����� ��й�ȣ : {storedPassword}");
                   // string storedPassword = snapshot.Child("AccountPW").GetValue(true).ToString();
                    if (password == storedPassword)
                    {
                        Debug.Log("Login Successful");
                        Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginSucess);
                        // �α��� ���� �� ó�� ���� �߰�
                        Debug.Log("�α��� ������ ����-> �� �ѱ�� DataBase���� �޾ƿ��� �� ");
                    }
                    else
                    {
                        Debug.Log("Wrong Password");
                        Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginFail_PW_Wrong);
                        // ��й�ȣ�� ��ġ���� �ʴ� ��� ó�� ���� �߰�
                    }
                }
                else
                {
                    Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginFail_ID_NotFound);
                    Debug.Log("���� ���� ȸ������ ��Ű��: ���â UI ����");
                    
                    // ȸ������ ó�� ���� �߰�
                }
            }
        });
    }

    public void CreateAccount(string accountNumber, string accountPassword)
    {
        //DatabaseReference accountRef = reference.Child("Account").Child(accountNumber);
        DatabaseReference accountRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber);
        // ���� ������ ����
        Dictionary<string, object> accountData = new Dictionary<string, object>
        {
            { "AccountPW", accountPassword }
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
                    Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.CreateAccount_Fail_IDSame);
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
                        Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.CreateAccount_Sucess);


                        // ���ο� DB �����ؼ� �ű�ĳ���� �����ϱ�

                    }
                });
            }
        });
    }
    public void CharacterInit(string accountNumber,int charcode,string charname)
    {
        CreateCharacter(accountNumber, charcode, charname);
        CreateInventory(accountNumber, charcode);
        CreateGoods(accountNumber, charcode);
        CreateEquips(accountNumber, charcode);
    }
    public void DataFetch(string accountNumber)
    {
        
        Debug.Log("�������� ��� ���⼭ �����������");
        foreach (var charkey in Managers.Data.CharacterDataDict.Keys)
        {
            FetchCharacterData(accountNumber, charkey);
            FetchAllGoodsData(accountNumber, charkey);
            FetchAllItemData(accountNumber, charkey);
            FetchEquipData(accountNumber, charkey);
        }
    }
    #region �ű� ĳ���� ó��
    private void CreateCharacter(string accountNumber, int charcode,string charname)
    {
       // Character ������ ����
      // ���� ������ ó���� Server���� �ؾ� ����
      // ���뵥���ʹ� ���� ������ �ʿ� ������? ( prefab path �� ) 

        Dictionary<string, object> characterData = new Dictionary<string, object>
        {
            {"level", Managers.Data.CharacterDataDict[charcode].level },
            {"exp",Managers.Data.CharacterDataDict[charcode].exp },
            {"DateTime",DateTime.Now.ToString() },
            {"name",charname }
        };
        Managers.Data.CharacterDataDict[charcode].dateTime= DateTime.Now.ToString();
        Managers.Data.CharacterDataDict[charcode].name = charname;
        // Character �����͸� Firebase�� ����
        //  DatabaseReference characterRef = reference.Child("Account").Child(accountNumber).Child("Character").Child(charcode.ToString());
        DatabaseReference characterRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString());
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
                Managers.Data.CharacterDataDict[charcode].isActive = true;
                Managers.Event.CreateOrDeleteCharacter?.Invoke(charcode);
            }
        });

        
    }

    private void CreateGoods(string accountNumber,int charcode)
    {
        // Character ������ ����
        // ���� ������ ó���� Server���� �ؾ� ����
        // ���뵥���ʹ� ���� ������ �ʿ� ������? ( prefab path �� ) 

        Dictionary<string, object> AccountGoods = new Dictionary<string, object>
        {
           { "Gold", 0 },
           { "RedDiamond", 0 },
           { "BlueDiamond", 0 },
        };
        // Character �����͸� Firebase�� ����
        DatabaseReference characterRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString()).Child("Goods");
        characterRef.UpdateChildrenAsync(AccountGoods).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error creating Goods: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                Debug.Log("Goods created successfully.");
            }
        });
    }
    private void CreateEquips(string accountNumber,int charcode)
    {
        // Character ������ ����
        // ���� ������ ó���� Server���� �ؾ� ����
        // ���뵥���ʹ� ���� ������ �ʿ� ������? ( prefab path �� ) 

        Dictionary<string, object> AccountEquip = new Dictionary<string, object>
        {
           { $"{Define.ItemType.Boot}", 0 },
           { $"{Define.ItemType.Cloth}", 0 },
           { $"{Define.ItemType.Weapon}", 0 },
           { $"{Define.ItemType.Earring}", 0 },
           { $"{Define.ItemType.Ring}", 0 },
           { $"{Define.ItemType.Hat}", 0 },

        };
        // Character �����͸� Firebase�� ����
        DatabaseReference characterRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString()).Child("EQUIPS");
        characterRef.UpdateChildrenAsync(AccountEquip).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error creating EQUIP: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                Debug.Log("EQUIP created successfully.");
            }
        });
    }

    private void CreateInventory(string accountNumber,int charcode)
    {
        // Character ������ ����
        // ���� ������ ó���� Server���� �ؾ� ����


        Dictionary<string, object> itemdata = new Dictionary<string, object>();

        foreach (var i in Managers.Data.ItemDataDict.Keys)
        {
            itemdata.Add("count", Managers.Data.ItemDataDict[i].count);
            itemdata.Add("Enhancement", 0);

            DatabaseReference ItemRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString()).Child("Items").Child(i.ToString());
            ItemRef.UpdateChildrenAsync(itemdata).ContinueWith(task =>
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
    public void CheckAccountID(string accountNumber,string password)
    {

        DatabaseReference accountRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber);
       
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
                    Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.CreateAccount_Fail_IDSame);
                    return;
                   // string accountPW = snapshot.Child("AccountPW").Value.ToString();
                   // Debug.Log("Account Password: " + accountPW);
                    // ������ ó�� �ڵ� �߰�
                }
                else
                {
                    CreateAccount(accountNumber, password);
                    // �����Ͱ� �������� �ʴ� ���
                    Debug.Log("Account data does not exist.");
                }
            }
        });
    }
    //��嵵 �����͸� ������ �ٵ� �Ź� �������� �����ϰ� ���� �ش� �����͸� DB�� �־��� �������� ����غ�����
    //��� ������ ���� DataBase�� ��带 Ȯ�� �� ����ó���� �� ������ ����ؾ���
    //������� �ؾ� ������.. - ���� ������ ���� - 
    public void FetchAllGoodsData(string accountNumber,int charcode)
    {

        DatabaseReference GoodsRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString()).Child("Goods");

        GoodsRef.GetValueAsync().ContinueWith(task =>
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
                    // Goods
                    string goodsName = itemSnapshot.Key;
                    int count = int.Parse(itemSnapshot.Value.ToString());
                    switch (goodsName)
                    {
                        case "BlueDiamond":
                            Managers.Game.BlueDiamondChange(count);
                            break;
                        case "RedDiamond":
                            Managers.Game.RedDiamondChange(count);
                            break;
                        case "Gold":
                            Managers.Game.GoldChange(count);
                            break;
                    }

                }

            }
        });
    }
    public void FetchEquipData(string accountNumber,int charcode)
    {

        DatabaseReference equipRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString()).Child("EQUIPS");

        equipRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving account data: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot itemSnapshot in snapshot.Children)
                {
                    // ������ ������ ����
                    string equiptype = itemSnapshot.Key;
                    int itemcode = int.Parse(itemSnapshot.Child("equiptype").Value.ToString());
                    Managers.Data.EquipData[(Define.ItemType)Enum.Parse(typeof(Define.ItemType), equiptype)]= itemcode;
                }

            }
        });
    }
    public void FetchCharacterData(string accountNumber, int charcode)
    {
        DatabaseReference characterRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString());
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

                        int code = int.Parse(snapshot.Key);
                        Managers.Data.CharacterDataDict[code].level= int.Parse(snapshot.Child("level").Value.ToString());
                        Managers.Data.CharacterDataDict[code].dateTime = snapshot.Child("DateTime").Value.ToString();
                        Managers.Data.CharacterDataDict[code].exp = int.Parse(snapshot.Child("exp").Value.ToString());
                        Managers.Data.CharacterDataDict[code].isActive = true;
                        Managers.Data.CharacterDataDict[code].name = snapshot.Child("name").Value.ToString();


                }
                else
                {
                    // �����Ͱ� �������� �ʴ� ���
                    Debug.Log("Character data does not exist.");
                }
            }
        });
    }
    public void FetchAllItemData(string accountNumber,int charcode)
    {
        DatabaseReference ItemsRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString()).Child("Items");

        ItemsRef.GetValueAsync().ContinueWith(task =>
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
                    int enhancement = int.Parse(itemSnapshot.Child("Enhancement").Value.ToString());
                    Managers.Data.ItemDataDict[itemCode].count = count;
                    Managers.Data.ItemDataDict[itemCode].enhancement = enhancement;
                    // ������ ������ Ȱ��
                    Debug.Log("Item: itemCode = " + itemCode + ", count = " + count);
                }
                Debug.Log("���� ���� ���ʿ� ������ ����ȭ ���� �־�� �մϴ�.");
                Debug.Log("�׳� �ҷ��� �����͸� itemcode�� �°� �� Data�� inventory�� �־��ָ� ��");
            }
        });
    }
    #endregion

    #region ������ ����
    public void UpdateItem(string accountNumber, int charcode,int itemCode, int acquireCount, Define.Update_DB_Item updateType =Define.Update_DB_Item.count)
    {
        DatabaseReference itemRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString()).Child("Items").Child(itemCode.ToString());

        itemRef.RunTransaction(transaction =>
        {
            if (transaction.Value != null)
            {
                int currentCount = int.Parse(transaction.Child($"{updateType}").Value.ToString());
                if (updateType == Define.Update_DB_Item.Enhancement)
                {
                    if (transaction.Child($"count").Value.Equals(0))
                    {
                        Debug.Log("������ ������ 0 �ΰ��� ��ȭ�� �ø������� ����!");
                        throw new Exception("������ ������ 0�� ���� ��ȭ�� �ø�������");
                    }

                }
                transaction.Child($"{updateType}").Value = $"{currentCount + acquireCount}";
                Debug.Log($"������ ���� ���� :: ������ = {currentCount} ���� �� = {currentCount + acquireCount}");

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

    //ĳ���� ������Ʈ�� ���� ���ٱ�? DB ����ġ ȹ���� �� ���� �ϸ� �� FireBase ������

    public void UpdateCharacterLevel(string accountNumber, int charactercode, int acquireCount,Define.Update_DB_Character updateType)
    {
        DatabaseReference itemRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charactercode.ToString());

        itemRef.RunTransaction(transaction =>
        {
            if (transaction.Value != null)
            {
                //������ ����ġ ��� ���� -> ȹ���� ���� �����ֱ� ������, �׳� ���� ó�� �մϴ�.
                int current = int.Parse(transaction.Child($"{updateType}").Value.ToString());
                transaction.Child($"{updateType}").Value = $"{current + acquireCount}";
                Debug.Log($"{updateType} ���� :: ������ = {current} ���� �� = {current + acquireCount}");
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
    public void UpdateEquip(string accountNumber, int charactercode, int euipcode, Define.Update_DB_EQUIPType updateType)
    {
        DatabaseReference itemRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charactercode.ToString()).Child("EQUIPS");

        itemRef.RunTransaction(transaction =>
        {
            if (transaction.Value != null)
            {
                int currentItemcode = int.Parse(transaction.Child($"{updateType}").Value.ToString());
                transaction.Child($"{updateType}").Value = $"{euipcode}";
                Debug.Log($"��� ���� :: ������ = {currentItemcode} ���� �� = {updateType}");
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
    public void UpdateCharacterActive(string accountNumber, int charactercode, bool isAcquire)
    {
        DatabaseReference itemRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charactercode.ToString());

        itemRef.RunTransaction(transaction =>
        {
            if (transaction.Value != null)
            {
                //������ ����ġ ��� ���� -> ȹ���� ���� �����ֱ� ������, �׳� ���� ó�� �մϴ�.
  
                transaction.Child($"isActive").Value = $"{isAcquire}";
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




    #endregion;
    #region ������ ����

    public void DeleteCharacter(string accountNumber, int charcode)
    {
        DatabaseReference characterRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString());

        characterRef.RemoveValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error deleting character: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                Debug.Log("Character deleted successfully.");
                Managers.Event.CreateOrDeleteCharacter?.Invoke(charcode);
                Managers.Data.CharacterDataDict[charcode].isActive = false;
            }
        });
    }
    public void DeleteCharacterChildren(string accountNumber, int charcode)
    {
        DatabaseReference charactersRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charcode.ToString());

        charactersRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving character: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    DatabaseReference childRef = charactersRef.Child(childSnapshot.Key);
                    childRef.RemoveValueAsync().ContinueWith(removeTask =>
                    {
                        if (removeTask.IsFaulted)
                        {
                            Debug.LogError("Error deleting child node: " + removeTask.Exception);
                        }
                        else if (removeTask.IsCompleted)
                        {
                            Debug.Log("Child node deleted successfully.");
                        }
                    });
                }
            }
        });
    }

    #endregion
}
