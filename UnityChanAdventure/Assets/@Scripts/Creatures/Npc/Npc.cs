using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    // NPC�� ���� ��ȣ�� ������ �ְ�, �÷��̾ �ֺ� ������ ����, ��ȭ �˾� �� ����
    //�׷��� �� �˾����� ����Ʈ�� ���� �� �ְ� ���� 
    [SerializeField]
    private Define.Npc_Type NPC_Type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            DialogSystem dialog = Managers.UI.ShowPopupUI<DialogSystem>();
            dialog.TalkType = NPC_Type;
            Debug.Log("����Ʈ ����");
        }
    }

}
