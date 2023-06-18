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
                Managers.DB.Init();
                Managers.DB.Init();
               // Managers.DB.ChecK_Account(129.ToString(), 4565.ToString());
                Managers.DB.CreateAccount(16551.ToString(), 26519.ToString());
               // Managers.DB.FetchAccountData(1681.ToString());
                //Managers.DB.FetchCharacterData(1681.ToString(),100);
               // Managers.DB.FetchAllItemData(1681.ToString());
               // Managers.DB.UpdateItem(1611.ToString(), 10001, 5);
              //  Managers.DB.UpdateCharacterLevel(1611.ToString(), 100, 5);
            }
        });
    }
}
