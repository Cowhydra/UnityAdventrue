using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : BaseScene
{
    GameObject player;
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
       Debug.Log("여기서 캐릭터가 들어오면 인벤토리 DB업데이트 해줘야함 Fetch!");
        SceneType = Define.Scene.TownScene;
        Managers.UI.ShowSceneUI<ShopUI>();
        Managers.UI.ShowSceneUI<GameUI>();

        OpeningEffect = Managers.UI.ShowSceneUI<TitleEffectUI>();
        OpeningEffect.Title=  "평범한 마을";
        if (GameObject.Find("Player") == null)
        {
            Managers.Resource.Instantiate("Player");
        }

        Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        Managers.UI.ShowSceneUI<Joystick_UI>();
        if (Managers.Game.Gold > 0)
        {
        }
        else
        {
            Debug.Log("추후 여기 ");
          //  Managers.UI.ShowPopupUI<DialogSystem>().TalkType = Define.Npc_Type.TuotorialNpc;
           // Managers.Game.GoldChange(30000);
        }





        StartCoroutine(nameof(SetPlayerPos));
        GameObject.FindGameObjectWithTag("AroundTarget").GetComponent<CinemachineVirtualCamera>()
            .m_Lens.FieldOfView = (int)Define.CameraFov.Default;

    }
    private IEnumerator SetPlayerPos()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        while (OpeningEffect!=null)
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
