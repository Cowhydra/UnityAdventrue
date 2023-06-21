using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class Quest
{
    protected QuestChannel _questsChannel;

    public string UniqueId;
    public string Name;
    public QuestState State;
    public int LevelRequirement;
    public int ExperienceReward;

    protected virtual void Enable()
    {
        this._questsChannel.QuestActivatedEvent += this.QuestActiveEvent;
        this._questsChannel.QuestCompleteEvent += this.QuestCompletedEvent;

        if (State == QuestState.Active)
        {
            this.QuestActive();
        }
    }

    protected virtual void Disable()
    {
        this._questsChannel.QuestActivatedEvent -= this.QuestActiveEvent;
        this._questsChannel.QuestCompleteEvent -= this.QuestCompletedEvent;
    }

    private void QuestActiveEvent(Quest activeQuest)
    {
        if (activeQuest.UniqueId != this.UniqueId) return;

        this.State = QuestState.Active;
        this.QuestActive();
    }

    protected abstract void QuestActive();

    private void QuestCompletedEvent(Quest completedQuest)
    {
        if (completedQuest.UniqueId != this.UniqueId) return;

        State = QuestState.Completed;
        this.QuestCompleted();
    }

    protected abstract void QuestCompleted();

    protected void Complete()
    {
        this._questsChannel.CompleteQuest(this);
    }
}
