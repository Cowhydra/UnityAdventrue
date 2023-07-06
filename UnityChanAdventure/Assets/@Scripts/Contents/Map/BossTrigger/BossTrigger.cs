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
    //¥ÎªÁ +  ƒ∆æ¿ + 5√ »ƒ ∫∏Ω∫ Ω√¿€!
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
        GameObject Main_cm = GameObject.Find("PlayerMain_Cm");
        if(Main_cm != null)
        {
            Main_cm.SetActive(false);
        }
        yield return new WaitForSeconds(2.0f);
       if(Main_cm != null)
        {
            Main_cm.SetActive(true);
        }
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.5f);
        switch (Managers.Scene.CurrentScene.SceneType)
        {
            case Define.Scene.LavaScene:
                break;
            case Define.Scene.DesertScene:
                GameObject.FindObjectOfType<DeserBoss_BT>().enabled = true;
                break;
            case Define.Scene.WaterScene:
                GameObject.FindObjectOfType<WaterBoss_BT>().enabled = true;
                break;
            case Define.Scene.FightScene:
                break;
        }
      
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
