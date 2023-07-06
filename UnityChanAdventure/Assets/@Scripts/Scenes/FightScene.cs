using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : BaseScene
{
    GameObject player;
    private void Start()
    {
        Managers.UI.ShowSceneUI<FightSceneUI>();
        SceneType = Define.Scene.FightScene;

        StartCoroutine(nameof(SetPlayerPos));
        Managers.Event.KeyInputAction -= AnyKeyInput;
        Managers.Event.KeyInputAction += AnyKeyInput;

    }

    private IEnumerator SetPlayerPos()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        while (true)
        {
            if (player == null) yield break;

            player.transform.position = GameObject.Find($"{gameObject.name}").transform.position;
            yield return null;

        }
    }

    public override void Clear()
    {

    }
    private void OnDestroy()
    {
        Managers.Event.KeyInputAction -= AnyKeyInput;
    }
    private void AnyKeyInput(Define.KeyInput keyinput)
    {
        StopCoroutine(nameof(SetPlayerPos));
    }
}
