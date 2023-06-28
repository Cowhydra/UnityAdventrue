using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : BaseScene
{
    // Start is called before the first frame update
    void Start()
    {
        SetResources();
    }

    public void SetResources()
    {
        Managers.Resource.LoadAllAsync<Object>("LoadingResource", (key, count, totalCount) =>
        {
            if (count == totalCount)
            {
                Debug.Log($"리소스 로딩 완료: {totalCount}");
                Managers.Data.Init();
                Debug.Log("여기서 캐릭터가 들어오면 인벤토리 DB업데이트 해줘야함 Fetch!");
                //데이터 Init()은 지워 주어야 합니다.
                //이거 살짝 불안 
                Managers.UI.ShowSceneUI<ShopUI>();

                Debug.Log("튜토리얼 창 띄우기 -> 따로 DB에 속성 만들기 귀찮으니 골드 등으로 첫 유저인지 확인");
                //DialogSystem dialog = Managers.UI.ShowPopupUI<DialogSystem>();
               // dialog.TalkType = Define.Npc_Type.TuotorialNpc;
              
                Managers.Game.GoldChange(30000);

                // Managers.DB.ChecK_Account(516551.ToString(), 26519.ToString());
                //Managers.DB.CharacterInit(230619.ToString(), 100,"호호호");
                // Managers.DB.DeleteCharacter(5555.ToString(), 100);
                // Managers.DB.FetchAccountData(516551.ToString());
                // Managers.DB.FetchCharacterData(516551.ToString(),100);
                // Managers.DB.FetchAllItemData(516551.ToString());
                // Managers.DB.UpdateItem(516551.ToString(), 10001, 5);
                // Managers.DB.UpdateCharacter(516551.ToString(), 100, 5,Define.Update_DB_Character.level);
                // Managers.DB.UpdateCharacter(516551.ToString(), 100, 500, Define.Update_DB_Character.exp);
                // Managers.DB.UpdateEquip(516551.ToString(), 100, 10002, Define.Update_DB_EQUIPType.Boot);
              
            }
        });
    }

    public override void Clear()
    {
        
    }
}
