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
                Managers.DB.ChecK_Account(129.ToString(), 4565.ToString());
                Managers.DB.CreateAccount(135.ToString(), 4568.ToString());
                Managers.DB.FetchAccountData(135.ToString());
                Managers.DB.FetchCharacterData(135.ToString(),100);
               // Managers.DB.FetchAllItemData(135.ToString());
                Managers.DB.UpdateItem(135.ToString(), 10001, 5);

            }
        });
    }
}
