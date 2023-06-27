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
                Debug.Log($"���ҽ� �ε� �Ϸ�: {totalCount}");
                Managers.Data.Init();
                Debug.Log("���⼭ ĳ���Ͱ� ������ �κ��丮 DB������Ʈ ������� Fetch!");
                //������ Init()�� ���� �־�� �մϴ�.
                //�̰� ��¦ �Ҿ� 
                Managers.UI.ShowSceneUI<ShopUI>();
                Managers.Game.GoldChange(30000);
                // Managers.DB.ChecK_Account(516551.ToString(), 26519.ToString());
                //Managers.DB.CharacterInit(230619.ToString(), 100,"ȣȣȣ");
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
