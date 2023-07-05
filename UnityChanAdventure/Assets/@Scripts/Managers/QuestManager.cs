using System.Collections;
using System.Collections.Generic;
using static Define;

public class QuestManager
{
    private List<Quest> activeQuests = new List<Quest>();
    public List<Quest> ActiveQuest { get { return activeQuests; } }
    public void StartQuest(Quest quest)
    {
        if (quest.State == QuestState.Pending&&!quest.isCleared)
        {
            quest.Enable();
            activeQuests.Add(quest);
            Managers.Event.ActiveQuest?.Invoke(quest);
        }
    }

    private void CompleteQuest(Quest quest)
    {
        if (quest.State == QuestState.Active)
        {
            quest.Disable();
            activeQuests.Remove(quest);
            Managers.Event.CompletedQuest?.Invoke(quest);
            Managers.Data.QuestData[quest.UniqueId].isCleared = true;
        }
        UnityEngine.Debug.Log("퀘스트 클리어 처리 DB");
    }
    public void CompleteQuest(int questid)
    {
        Quest quest = activeQuests.Find(s => s.UniqueId == questid);
        if(quest == null)
        {
            UnityEngine.Debug.Log("이상함");
        }
        else
        {
            CompleteQuest(quest);
        }
    }
}

