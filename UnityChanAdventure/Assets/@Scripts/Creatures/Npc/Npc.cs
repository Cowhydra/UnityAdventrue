using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    // NPC는 고유 번호를 가지고 있고, 플레이어가 주변 범위에 오면, 대화 팝업 을 실행
    //그래서 그 팝업에서 퀘스트를 받을 수 있게 설정 
    [SerializeField]
    private Define.Npc_Type NPC_Type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            DialogSystem dialog = Managers.UI.ShowPopupUI<DialogSystem>();
            dialog.TalkType = NPC_Type;
            Debug.Log("퀘스트 띄우기");
        }
    }

}
