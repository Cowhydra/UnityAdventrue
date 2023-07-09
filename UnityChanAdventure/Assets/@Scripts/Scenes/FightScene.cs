using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : BaseScene
{
    GameObject player;
    TitleEffectUI OpeningEffect;
    private void Start()
    {
        Managers.UI.ShowSceneUI<FightSceneUI>();
        SceneType = Define.Scene.FightScene;
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        Managers.UI.ShowSceneUI<Joystick_UI>();
        OpeningEffect = Managers.UI.ShowSceneUI<TitleEffectUI>();
        OpeningEffect.Title = "대난투 경기장";

         StartCoroutine(nameof(SetPlayerPos));

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

    public override void Clear()
    {

    }
}
