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

        //플레이어 위치 버그 수정용
        StartCoroutine(nameof(SetPlayerPos));
        Managers.Event.KeyInputAction -= AnyKeyInput;
        Managers.Event.KeyInputAction += AnyKeyInput;
    }
    private void OnDestroy()
    {
        Managers.Event.KeyInputAction -= AnyKeyInput;
    }
    public override void Clear()
    {
        Managers.Clear();
    }
    private IEnumerator SetPlayerPos()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        while (true)
        {
            player.transform.position = GameObject.Find($"{gameObject.name}").transform.position;
            yield return null;

        }
    }

    private void AnyKeyInput(Define.KeyInput keyinput)
    {
        StopAllCoroutines();
    }
  
}
