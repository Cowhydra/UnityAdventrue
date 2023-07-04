using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Managers.Resource.Instantiate("Player");

        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = gameObject.transform.position;
       // StartCoroutine(nameof(playerStartPos_Fix));
        //Managers.UI.ShowPopupUI<DialogSystem>().TalkType = Define.Npc_Type.Dungeon;
    }

    public override void Clear()
    {

    }
    private IEnumerator playerStartPos_Fix()
    {
        while (true)
        {
            player.transform.position = transform.position;

            if ((player.transform.position - transform.position).sqrMagnitude < 0.1f)
            {
                yield break;
            }

            yield return null;
        }


    }

}
