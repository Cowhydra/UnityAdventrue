using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChannel 
{
    public Action<Quest> QuestCompleteEvent;
    public Action<Quest> QuestActivatedEvent;

    public void CompleteQuest(Quest completedQuest)
    {
        QuestCompleteEvent?.Invoke(completedQuest);
    }

    public void AssignQuest(Quest questToAssign)
    {
        QuestActivatedEvent?.Invoke(questToAssign);
    }
}
