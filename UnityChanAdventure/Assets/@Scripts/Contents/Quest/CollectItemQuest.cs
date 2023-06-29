using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemQuest : Quest
{
    private int _objectItemCode;
    private int _amountToCollect;

    public CollectItemQuest(int uniqueId, string name, int levelRequirement, int experienceReward, int DiaRward, int itemcode, int objectItemCode, int amountToCollect)
    {
        this.UniqueId = uniqueId;
        this.Name = name;
        this.LevelRequirement = levelRequirement;
        this.ExperienceReward = experienceReward;
        this.DiaReward = DiaRward;
        this._objectItemCode = objectItemCode;
        this.itemReward= itemcode;
        this._amountToCollect = amountToCollect;
    }
    protected override void QuestActive()
    {
        var uniqueId = _objectItemCode;
        Managers.Event.AddItem -= TryComplete;
        Managers.Event.AddItem += TryComplete;

         this.TryComplete(_objectItemCode);

    }

    protected override void QuestCompleted()
    {

        if (DiaReward != 0)
        {
            Managers.Game.BlueDiamondChange(DiaReward);
        }
        if (ExperienceReward != 0)
        {
            GameObject.FindObjectOfType<MyCharacter>().Exp += ExperienceReward;
        }
        if (itemReward != 0)
        {
            Managers.Inven.Add(itemReward);
        }
        Managers.Event.AddItem -= TryComplete;
    }


    private void TryComplete(int objectitemcode)
    {
        if (Managers.Inven.Items[objectitemcode].Count >= this._amountToCollect)
        {
            this.Complete();
        }
    }
}