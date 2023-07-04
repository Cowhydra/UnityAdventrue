using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMap : MonoBehaviour
{
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
              GameObject go= Managers.Resource.Instantiate("Itemoutside");
                go.GetComponent<Itemoutside>().ItemCode = 10001;
                Managers.Scene.LoadScene(Define.Scene.TownScene);
            }

        });
    }
}
