using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;
using Firebase.Database;

public class DBManager
{
    public int AccountNumber;
    public string DBurl = "ttps://unitychanadventure-default-rtdb.firebaseio.com/";
    DatabaseReference reference;

    public void Init()
    {

        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(DBurl);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void FindDB()
    {
        DatabaseReference accountRef = reference.Child("AccountNumber").Child($"{AccountNumber}");

        accountRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

                throw new Exception("Error retrieving account data");
                //FireBase 권한 및 네트워크 혹은 데이터 경로 확인 필요!
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    return;
                }
                else
                {
                    return;
                }
            }
        });
    }

    public void WriteDB()
    {

    }
    public void ReadDB()
    {

    }
    public void ReadAllDB()
    {

    }
}
