using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : BaseScene
{
    PlayerController playerController;
    TitleEffectUI OpeningEffect;
    // Start is called before the first frame update
    void Start()
    {
        Inits();
    }
    protected override void Init()
    {
        base.Init();
       
    }

    public void Inits()
    {
        Debug.Log("���⼭ ĳ���Ͱ� ������ �κ��丮 (��� ��  )DB������Ʈ ������� Fetch!");
        SceneType = Define.Scene.TownScene;

        #region �� �ʱ�ȭ 

        Managers.UI.ShowSceneUI<ShopUI>();
        Managers.UI.ShowSceneUI<GameUI>();

        OpeningEffect = Managers.UI.ShowSceneUI<TitleEffectUI>();
        OpeningEffect.Title=  "����� ����";
       
        if (GameObject.Find("Player") == null)
        {
            Managers.Resource.Instantiate("Player");
        }
        playerController = FindAnyObjectByType<PlayerController>();
        if(playerController != null)
        {
            playerController.AutoOff();
        }
 
        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        Managers.UI.ShowSceneUI<Joystick_UI>();


        #endregion
        if (Managers.Game.Gold > 0)
        {
        }
        else
        {
       //   Managers.UI.ShowPopupUI<DialogSystem>().TalkType = Define.Npc_Type.TuotorialNpc;
        }





        StartCoroutine(nameof(SetPlayerPos));
        GameObject.FindGameObjectWithTag("AroundTarget").GetComponent<CinemachineVirtualCamera>()
            .m_Lens.FieldOfView = (int)Define.CameraFov.Default;
        Managers.EQUIP.Init();

    }
    private IEnumerator SetPlayerPos()
    {
       
        while (OpeningEffect!=null)
        {
            if (playerController == null) yield break;

            playerController.transform.position = GameObject.Find($"{gameObject.name}").transform.position;
            yield return null;

        }
    }

    public override void Clear()
    {
        
    }
}
