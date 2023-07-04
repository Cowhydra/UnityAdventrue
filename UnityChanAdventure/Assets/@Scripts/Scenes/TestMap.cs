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
                Debug.Log($"���ҽ� �ε� �Ϸ�: {totalCount}");
                Managers.Data.Init();
                Managers.EQUIP.Init();
                Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();

            }

        });
    }
}
