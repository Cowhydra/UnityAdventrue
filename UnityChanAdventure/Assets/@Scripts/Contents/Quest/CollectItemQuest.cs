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
        this.TryComplete();

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

    }


    private void TryComplete()
    {
        if (Managers.Inven.Items[_objectItemCode].Count >= this._amountToCollect)
        {
            this.Complete();
        }
    }
}