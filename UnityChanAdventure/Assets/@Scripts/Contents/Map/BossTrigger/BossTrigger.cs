using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour,IListener
{
    private bool isTriggered = false;
    private void Start()
    {
        Managers.Event.AddListener(Define.EVENT_TYPE.DialogClose, this);
    }
    //��� +  �ƾ� + 5���� ���� ����!
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered &&other.CompareTag("Player"))
        {
            isTriggered = true;
            DialogSystem dialog=Managers.UI.ShowPopupUI<DialogSystem>();
            dialog.TalkType = Define.Npc_Type.Boss;
        }
    }
    private IEnumerator BossCutScene_co()
    {
        Time.timeScale = 0.5f;
        GameObject.FindGameObjectWithTag("AroundTarget").SetActive(false);
        yield return new WaitForSeconds(3.0f);
        GameObject.FindGameObjectWithTag("AroundTarget").SetActive(true);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.5f);
        GameObject.FindObjectOfType<DeserBoss_BT>().enabled = true;
    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case Define.EVENT_TYPE.DialogClose:
                StartCoroutine(nameof(BossCutScene_co));
                break;
        }
    }
}