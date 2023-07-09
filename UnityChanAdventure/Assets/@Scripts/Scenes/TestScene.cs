using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene
{
    // Start is called before the first frame update
    void Start()
    {
        SetResources();
    }

    public void SetResources()
    {
        Managers.Resource.LoadAllAsync<Object>("LoadingResource", (key, count, totalCount) =>
        {
            if (count == totalCount)
            {
                Debug.Log($"리소스 로딩 완료: {totalCount}");
                Managers.Data.Init();
                Managers.EQUIP.Init();
                Managers.Scene.LoadScene(Define.Scene.TownScene);
            }
           
        });
    }

    public override void Clear()
    {
       
    }
}
