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
        SceneType = Define.Scene.DesertScene;
        GameObject.FindAnyObjectByType<DeserBoss_BT>().enabled = false;
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        Managers.UI.ShowSceneUI<Joystick_UI>();
        Managers.UI.ShowSceneUI<GameUI>();
        
        //플레이어 버그 수정용....
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
