using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    public override void Clear()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        SetResources();
    }

    public  void SetResources()
    {
        Managers.Resource.LoadAllAsync<Object>("LoadingResource", (key, count, totalCount) =>
        {
            if (count == totalCount)
            {
                Debug.Log("리소스 로딩 완료");
                Managers.Data.Init();
                //Managers.DB.Init();
                Managers.DB.ChecK_Account(123.ToString(), 4564.ToString());
                Managers.DB.CreateAccount(123.ToString(), 4567.ToString());
            }
        });
    }
}
