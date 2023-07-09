using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class DesertScene : BaseScene
{
    GameObject player;
    TitleEffectUI OpeningEffect;
    void Start()
    {
        Inits();
    }
     public void Inits()
    {
        SceneType = Define.Scene.DesertScene;
        GameObject.FindAnyObjectByType<DeserBoss_BT>().enabled = false;
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        Managers.UI.ShowSceneUI<Joystick_UI>();
        Managers.UI.ShowSceneUI<GameUI>();
        OpeningEffect = Managers.UI.ShowSceneUI<TitleEffectUI>();
        OpeningEffect.Title = "사막 지역";
        //플레이어 버그 수정용....
        StartCoroutine(nameof(SetPlayerPos));
 

    }

    public override void Clear()
    {
       
    }
    private IEnumerator SetPlayerPos()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        while (OpeningEffect != null)
        {
            if (player == null) yield break;

            player.transform.position = GameObject.Find($"{gameObject.name}").transform.position;
            yield return null;

        }
    }


}
