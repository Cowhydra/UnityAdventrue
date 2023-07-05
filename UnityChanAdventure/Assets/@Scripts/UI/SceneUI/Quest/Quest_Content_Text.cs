using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Quest_Content_Text : UI_Scene
{
    public override void Init()
    {
        base.Init();
        AddEvent();
        GetComponent<TextMeshProUGUI>().text = "";
        UIInit();

    }

    private int _questid;
    public int QuestID
    {
        get { return _questid;}
        set
        {
            _questid = value;
            Init();
        }
    }
    private void OnDestroy()
    {
        Managers.Event.MonsterDie -= RefreshUI_Mon;
        Managers.Event.AddItem -= RefreshUI_CollectItem;
    }
    private void AddEvent()
    {
        if (Managers.Data.QuestData[QuestID].QuestType == Define.QuestType.CollectItem)
        {
            Managers.Event.AddItem -= RefreshUI_CollectItem;
            Managers.Event.AddItem += RefreshUI_CollectItem;
        }
        else
        {
            Managers.Event.MonsterDie -= RefreshUI_Mon;
            Managers.Event.MonsterDie += RefreshUI_Mon;
        }
        Managers.Event.CompletedQuest -= QuestCompleted;
        Managers.Event.CompletedQuest += QuestCompleted;

    }

    private void RefreshUI_Mon(int monstercode)
    {
        if (monstercode != Managers.Data.QuestData[QuestID].enemyToTargetCode)
        {
            return;
        }
        DefeatEnemiesQuest myquest = Managers.Quest.ActiveQuest.Find(s => s.UniqueId == _questid) as DefeatEnemiesQuest;
       
        GetComponent<TextMeshProUGUI>().text = $"{Managers.Data.MonsterDataDict[monstercode].name}을 잡아라!!\n" +
            $"{ myquest.ActualEnemiesDestroyed}/{Managers.Data.QuestData[QuestID].Amount}";
        if (myquest.ActualEnemiesDestroyed == Managers.Data.QuestData[QuestID].Amount)
        {
            GetComponent<TextMeshProUGUI>().text
                = $"{Managers.Data.MonsterDataDict[monstercode].name}를 전부 잡았습니다.\n 클릭하여 보상을 획득해 주세요";
            gameObject.transform.GetChild(0).gameObject.BindEvent((PointerEventData data) => Managers.Quest.CompleteQuest(QuestID));
        }
        
    }
    private void RefreshUI_CollectItem(int itemcode)
    {
        if (!Managers.Inven.Items.ContainsKey(itemcode))
        {
            GetComponent<TextMeshProUGUI>().text = $"{Managers.Data.ItemDataDict[itemcode].name}을 모아라!!\n" +
               $"{0}/{Managers.Data.QuestData[QuestID].Amount}";

        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = $"{Managers.Data.ItemDataDict[itemcode].name}을 모아라!!\n" +
    $"{Managers.Inven.Items[itemcode].Count}/{Managers.Data.QuestData[QuestID].Amount}";

            if(Managers.Inven.Items[itemcode].Count== Managers.Data.QuestData[QuestID].Amount)
            {
                GetComponent<TextMeshProUGUI>().text
                 = $"{Managers.Inven.Items[itemcode].Count}를 모두 모았습니다.\n 클릭하여 보상을 획득해 주세요";

                gameObject.transform.GetChild(0).gameObject.BindEvent((PointerEventData data) => Managers.Quest.CompleteQuest(QuestID));
            }
        }
    }
    private void UIInit()
    {
        if (Managers.Data.QuestData[QuestID].QuestType == Define.QuestType.CollectItem)
        {
            RefreshUI_CollectItem(Managers.Data.QuestData[QuestID].objectItemCode);
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = $"{Managers.Data.MonsterDataDict[Managers.Data.QuestData[QuestID].enemyToTargetCode].name}을 잡아라!!\n" +
                      $"{0}/{Managers.Data.QuestData[QuestID].Amount}";

        }
    }
    private void QuestCompleted(Quest quest)
    {
        if (quest.UniqueId != QuestID)
        {
            return;
        }
        else
        {
            Managers.Resource.Destroy(gameObject);
        }
    }
   
}
