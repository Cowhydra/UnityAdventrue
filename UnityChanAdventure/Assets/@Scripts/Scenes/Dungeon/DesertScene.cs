using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class DesertScene : BaseScene
{
    GameObject player;
    void Start()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();
        GameObject.FindAnyObjectByType<DeserBoss_BT>().enabled = false;
        Managers.UI.ShowSceneUI<GameUI>();
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = gameObject.transform.position;
    }

    public override void Clear()
    {
        Managers.Clear();
    }


}
