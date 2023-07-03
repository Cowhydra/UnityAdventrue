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
                GameObject.FindAnyObjectByType<DeserBoss_BT>().enabled = false;
                Managers.UI.ShowSceneUI<GameUI>();
                Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();


               // Managers.UI.ShowPopupUI<DialogSystem>().TalkType = Define.Npc_Type.Dungeon;
            }
        });
    }

    public override void Clear()
    {

    }
}
