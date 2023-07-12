using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScene : BaseScene
{
    GameObject player;
    TitleEffectUI OpeningEffect;
    void Start()
    {
        Inits();
    }

    public void Inits()
    {
        FindAnyObjectByType<WaterBoss_BT>().enabled = false;
         SceneType= Define.Scene.WaterScene;
        Managers.UI.ShowSceneUI<GameUI>();
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        Managers.UI.ShowSceneUI<Joystick_UI>();
        player = GameObject.FindGameObjectWithTag("Player");
        OpeningEffect = Managers.UI.ShowSceneUI<TitleEffectUI>();
        OpeningEffect.Title = "수중 신전";

        //플레이어 위치 버그 수정용
        StartCoroutine(nameof(SetPlayerPos));

        GameObject.FindGameObjectWithTag("AroundTarget").GetComponent<CinemachineVirtualCamera>()
       .m_Lens.FieldOfView = (int)Define.CameraFov.WaterScene;
        GameObject.FindGameObjectWithTag("AroundTarget").GetComponent<CinemachineVirtualCamera>()
     .m_Lens.FarClipPlane = 60;
    }

    public override void Clear()
    {
        Managers.Clear();
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
