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

    //이제 이해 완료
    //Account - AccountNumber - 130  - AccountPW , Character,Items,Goods, 이런식으로 구조를 짯어야 헀는데
    //지금은 Account 130 - AccountNumber,AccountPW,CHarcter,Itms,Goods 이런 형태...

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
            // 연결 자체의 실패
            if (task.IsFaulted)
            {
                // Firebase 권한 및 네트워크 혹은 데이터 경로 확인 필요!
                Debug.Log("실패중");
                
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // accountRef에 대한 결과가 있을 때
                if (snapshot.Exists)
                {
                    // ID가 일치하는 경우
                    string storedPassword = snapshot.Child("AccountPW").Value.ToString();
                    Debug.Log($"저장된 비밀번호 : {storedPassword}");
                   // string storedPassword = snapshot.Child("AccountPW").GetValue(true).ToString();
                    if (password == storedPassword)
                    {
                        Debug.Log("Login Successful");
                        Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginSucess);
                        // 로그인 성공 후 처리 로직 추가
                        Debug.Log("로그인 성공후 로직-> 씬 넘기고 DataBase에서 받아오기 등 ");
                    }
                    else
                    {
                        Debug.Log("Wrong Password");
                        Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginFail_PW_Wrong);
                        // 비밀번호가 일치하지 않는 경우 처리 로직 추가
                    }
                }
                else
                {
                    Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.LoginFail_ID_NotFound);
                    Debug.Log("없는 계정 회원가입 시키기: 경고창 UI 띄우기");
                    
                    // 회원가입 처리 로직 추가
                }
            }
        });
    }

    public void CreateAccount(string accountNumber, string accountPassword)
    {
        //DatabaseReference accountRef = reference.Child("Account").Child(accountNumber);
        DatabaseReference accountRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber);
        // 계정 데이터 생성
        Dictionary<string, object> accountData = new Dictionary<string, object>
        {
            { "AccountPW", accountPassword }
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
                    Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.CreateAccount_Fail_IDSame);
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
                        Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.CreateAccount_Sucess);


                        // 새로운 DB 생성해서 신규캐릭터 맞이하기

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
        
        Debug.Log("장착중인 장비도 여기서 갱신해줘야함");
        foreach (var charkey in Managers.Data.CharacterDataDict.Keys)
        {
            FetchCharacterData(accountNumber, charkey);
            FetchAllGoodsData(accountNumber, charkey);
            FetchAllItemData(accountNumber, charkey);
            FetchEquipData(accountNumber, charkey);
        }
    }
    #region 신규 캐릭터 처리
    private void CreateCharacter(string accountNumber, int charcode,string charname)
    {
       // Character 데이터 생성
      // 원래 데이터 처리는 Server에서 해야 안전
      // 공용데이터는 따로 저장할 필요 없을듯? ( prefab path 등 ) 

        Dictionary<string, object> characterData = new Dictionary<string, object>
        {
            {"level", Managers.Data.CharacterDataDict[charcode].level },
            {"exp",Managers.Data.CharacterDataDict[charcode].exp },
            {"DateTime",DateTime.Now.ToString() },
            {"name",charname }
        };
        Managers.Data.CharacterDataDict[charcode].dateTime= DateTime.Now.ToString();
        Managers.Data.CharacterDataDict[charcode].name = charname;
        // Character 데이터를 Firebase에 쓰기
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
        // Character 데이터 생성
        // 원래 데이터 처리는 Server에서 해야 안전
        // 공용데이터는 따로 저장할 필요 없을듯? ( prefab path 등 ) 

        Dictionary<string, object> AccountGoods = new Dictionary<string, object>
        {
           { "Gold", 0 },
           { "RedDiamond", 0 },
           { "BlueDiamond", 0 },
        };
        // Character 데이터를 Firebase에 쓰기
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
        // Character 데이터 생성
        // 원래 데이터 처리는 Server에서 해야 안전
        // 공용데이터는 따로 저장할 필요 없을듯? ( prefab path 등 ) 

        Dictionary<string, object> AccountEquip = new Dictionary<string, object>
        {
           { $"{Define.ItemType.Boot}", 0 },
           { $"{Define.ItemType.Cloth}", 0 },
           { $"{Define.ItemType.Weapon}", 0 },
           { $"{Define.ItemType.Earring}", 0 },
           { $"{Define.ItemType.Ring}", 0 },
           { $"{Define.ItemType.Hat}", 0 },

        };
        // Character 데이터를 Firebase에 쓰기
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
        // Character 데이터 생성
        // 원래 데이터 처리는 Server에서 해야 안전


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

    #region 데이터 정보 불러오기
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
                    // 데이터가 존재하는 경우
                    Managers.Event.LoginProgess?.Invoke(Define.Login_Event_Type.CreateAccount_Fail_IDSame);
                    return;
                   // string accountPW = snapshot.Child("AccountPW").Value.ToString();
                   // Debug.Log("Account Password: " + accountPW);
                    // 데이터 처리 코드 추가
                }
                else
                {
                    CreateAccount(accountNumber, password);
                    // 데이터가 존재하지 않는 경우
                    Debug.Log("Account data does not exist.");
                }
            }
        });
    }
    //골드도 데이터를 가져옴 근데 매번 아이템을 구매하고 나서 해당 데이터를 DB에 넣어줄 것인지는 고민해봐야함
    //골드 구매할 떄도 DataBase의 골드를 확인 후 구매처리를 할 것인지 고민해야함
    //원래라면 해야 겠지만.. - 나는 서버가 없음 - 
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
                    // 아이템 데이터 추출
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
                    // 데이터가 존재하지 않는 경우
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
                    // 아이템 데이터 추출
                    int itemCode = int.Parse(itemSnapshot.Key);
                    int count = int.Parse(itemSnapshot.Child("count").Value.ToString());
                    int enhancement = int.Parse(itemSnapshot.Child("Enhancement").Value.ToString());
                    Managers.Data.ItemDataDict[itemCode].count = count;
                    Managers.Data.ItemDataDict[itemCode].enhancement = enhancement;
                    // 추출한 데이터 활용
                    Debug.Log("Item: itemCode = " + itemCode + ", count = " + count);
                }
                Debug.Log("추후 여기 위쪽에 데이터 동기화 과정 넣어야 합니다.");
                Debug.Log("그냥 불러온 데이터를 itemcode에 맞게 내 Data나 inventory에 넣어주면 됨");
            }
        });
    }
    #endregion

    #region 데이터 갱신
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
                        Debug.Log("아이템 개수가 0 인것의 강화를 올리려고함 오류!");
                        throw new Exception("아이템 개수가 0인 것의 강화를 올리려고함");
                    }

                }
                transaction.Child($"{updateType}").Value = $"{currentCount + acquireCount}";
                Debug.Log($"아이템 개수 변경 :: 변경전 = {currentCount} 변경 후 = {currentCount + acquireCount}");

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

    //캐릭터 업데이트는 언제 해줄까? DB 경험치 획득할 떄 마다 하면 내 FireBase 죽을듯

    public void UpdateCharacterLevel(string accountNumber, int charactercode, int acquireCount,Define.Update_DB_Character updateType)
    {
        DatabaseReference itemRef = reference.Child("Account").Child("AccountNumber").Child(accountNumber).Child("Characters").Child(charactercode.ToString());

        itemRef.RunTransaction(transaction =>
        {
            if (transaction.Value != null)
            {
                //레벨과 경험치 모두 같이 -> 획득한 값을 더해주기 때문에, 그냥 같이 처리 합니다.
                int current = int.Parse(transaction.Child($"{updateType}").Value.ToString());
                transaction.Child($"{updateType}").Value = $"{current + acquireCount}";
                Debug.Log($"{updateType} 변경 :: 변경전 = {current} 변경 후 = {current + acquireCount}");
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
                Debug.Log($"장비 변경 :: 변경전 = {currentItemcode} 변경 후 = {updateType}");
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
                //레벨과 경험치 모두 같이 -> 획득한 값을 더해주기 때문에, 그냥 같이 처리 합니다.
  
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
    #region 데이터 삭제

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
