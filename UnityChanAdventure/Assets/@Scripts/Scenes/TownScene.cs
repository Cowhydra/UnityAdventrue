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
       Debug.Log("���⼭ ĳ���Ͱ� ������ �κ��丮 DB������Ʈ ������� Fetch!");

       Managers.UI.ShowSceneUI<ShopUI>();
       Managers.UI.ShowSceneUI<GameUI>();
       Managers.Resource.Instantiate("Player");
       Managers.UI.ShowSceneUI<PlayerStatus_Canvas>();
       Debug.Log("Ʃ�丮�� â ���� -> ���� DB�� �Ӽ� ����� �������� ��� ������ ù �������� Ȯ��");
        //DialogSystem dialog = Managers.UI.ShowPopupUI<DialogSystem>();
        // dialog.TalkType = Define.Npc_Type.TuotorialNpc;
       player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(nameof(playerStartPos_Fix));
      
        if (Managers.Game.Gold > 0)
        {

        }
        else
        {
            Managers.UI.ShowPopupUI<DialogSystem>().TalkType = Define.Npc_Type.TuotorialNpc;
            Managers.Game.GoldChange(30000);
        }

        DontDestroyOnLoad(this);

    }
    private IEnumerator playerStartPos_Fix()
    {
        while (true)
        {
            player.transform.position = transform.position;
            if ((player.transform.position - transform.position).sqrMagnitude < 0.1f)
            {
                yield break;
            }

            yield return null;
        }

      
    }

    public override void Clear()
    {
        
    }
}
