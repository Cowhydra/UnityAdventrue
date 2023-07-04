using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScene : BaseScene
{
    GameObject player;
    void Start()
    {
        Inits();
    }

    public void Inits()
    {
        FindAnyObjectByType<WaterBoss_BT>().enabled = false;
        Managers.UI.ShowSceneUI<GameUI>();
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(nameof(playerStartPos_Fix));
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
