using System.Collections;
using System.Collections.Generic;
using static Define;

public class QuestManager
{
    private List<Quest> activeQuests = new List<Quest>();

    public void StartQuest(Quest quest)
    {
        if (quest.State == QuestState.Pending&&!quest.isCleared)
        {
            quest.Enable();
            activeQuests.Add(quest);
            Managers.Event.ActiveQuest?.Invoke(quest);
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (quest.State == QuestState.Active)
        {
            quest.Disable();
            activeQuests.Remove(quest);
            Managers.Event.CompletedQuest?.Invoke(quest);
        }
        UnityEngine.Debug.Log("퀘스트 클리어 처리 DB");
    }
}

