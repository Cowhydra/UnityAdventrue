using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class Quest
{

    public int UniqueId;
    public string Name;
    public QuestState State;
    public int LevelRequirement;
    public int ExperienceReward;
    public int DiaReward;
    public int itemReward;
    public bool isCleared;

    public virtual void Enable()
    {
        Managers.Event.ActiveQuest += this.QuestActiveEvent;
        Managers.Event.CompletedQuest += this.QuestCompletedEvent;

        if (State == QuestState.Active)
        {
            this.QuestActive();
        }
        Managers.Sound.Play("QuestAccept");
    }

    public virtual void Disable()
    {
        Managers.Event.ActiveQuest -= this.QuestActiveEvent;
        Managers.Event.CompletedQuest -= this.QuestCompletedEvent;
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
        // Managers.Event.CompletedQuest?.Invoke(this);

    }
}
