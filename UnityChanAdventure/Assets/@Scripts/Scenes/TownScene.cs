using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : BaseScene
{
    GameObject player;
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
        if (GameObject.Find("Player") == null)
        {
            Managers.Resource.Instantiate("Player");
        }

       Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
        if (Managers.Game.Gold > 0)
        {
        }
        else
        {
            Debug.Log("추후 여기 ");
          //  Managers.UI.ShowPopupUI<DialogSystem>().TalkType = Define.Npc_Type.TuotorialNpc;
           // Managers.Game.GoldChange(30000);
        }

        //플레이어 위치 버그 수정용
        StartCoroutine(nameof(SetPlayerPos));
        Managers.Event.KeyInputAction -= AnyKeyInput;
        Managers.Event.KeyInputAction += AnyKeyInput;

        GameObject.FindGameObjectWithTag("AroundTarget").GetComponent<CinemachineVirtualCamera>()
            .m_Lens.FieldOfView = (int)Define.CameraFov.Default;

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
    private void OnDestroy()
    {
        Managers.Event.KeyInputAction -= AnyKeyInput;
    }
    private void AnyKeyInput(Define.KeyInput keyinput)
    {
        StopCoroutine(nameof(SetPlayerPos));
    }

    public override void Clear()
    {
        
    }
}
