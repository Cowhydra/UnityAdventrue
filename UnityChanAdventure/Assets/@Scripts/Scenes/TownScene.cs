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

    public void Inits()
    {
       Debug.Log("여기서 캐릭터가 들어오면 인벤토리 DB업데이트 해줘야함 Fetch!");

       Managers.UI.ShowSceneUI<ShopUI>();
       Managers.UI.ShowSceneUI<GameUI>();
       Managers.Resource.Instantiate("Player");
       Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
       Debug.Log("튜토리얼 창 띄우기 -> 따로 DB에 속성 만들기 귀찮으니 골드 등으로 첫 유저인지 확인");
        //DialogSystem dialog = Managers.UI.ShowPopupUI<DialogSystem>();
        // dialog.TalkType = Define.Npc_Type.TuotorialNpc;
        if (Managers.Game.Gold > 0)
        {

        }
        else
        {
            Managers.UI.ShowPopupUI<DialogSystem>().TalkType = Define.Npc_Type.TuotorialNpc;
            Managers.Game.GoldChange(30000);
        }

     

    }


    public override void Clear()
    {
        
    }
}
