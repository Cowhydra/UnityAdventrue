using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertScene : BaseScene
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
                Debug.Log($"���ҽ� �ε� �Ϸ�: {totalCount}");
                Managers.Data.Init();

            }
        });
    }

    public override void Clear()
    {

    }
}
