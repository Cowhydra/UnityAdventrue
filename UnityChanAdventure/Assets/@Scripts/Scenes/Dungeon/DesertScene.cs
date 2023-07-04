using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class DesertScene : BaseScene
{
    GameObject player;
    void Start()
    {
        Inits();
    }
     public void Inits()
    {
        GameObject.FindAnyObjectByType<DeserBoss_BT>().enabled = false;
        Managers.UI.ShowSceneUI<GameUI>();
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = GameObject.Find($"{gameObject.name}").transform.position;
        Debug.Log($"{gameObject.name}");
    }

    public override void Clear()
    {
        Managers.Clear();
    }


}
