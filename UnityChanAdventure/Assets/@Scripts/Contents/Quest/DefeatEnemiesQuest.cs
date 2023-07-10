using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemiesQuest : Quest
{
   
    private int _enemyToTargetCode;

    public int NumberOfEnemiesToDestroy;
    private int _actualEnemiesDestroyed;
    public int ActualEnemiesDestroyed { get { return _actualEnemiesDestroyed; } set { _actualEnemiesDestroyed = value; } }
    public DefeatEnemiesQuest(int uniqueId, string name, int levelRequirement, int experienceReward, int DiaReword, int itemReward,int enemyToTargetCode, int numberOfEnemiesToDestroy)
    {
        this.UniqueId = uniqueId;
        this.Name = name;
        this.LevelRequirement = levelRequirement;
        this.ExperienceReward = experienceReward;
        this.DiaReward = DiaReword;
        this.itemReward= itemReward;
        this._enemyToTargetCode = enemyToTargetCode;
        this.NumberOfEnemiesToDestroy = numberOfEnemiesToDestroy;
    }

    public override void Enable()
    {
        base.Enable();

     
    }

    protected override void QuestCompleted()
    {
        //퀘스트가 완료되면 이벤트 제거
   
        if (DiaReward != 0)
        {
            Managers.Game.BlueDiamondChange(DiaReward);
        }
        if(ExperienceReward != 0)
        {
            GameObject.FindObjectOfType<MyCharacter>().Exp += ExperienceReward;
        }
        if (itemReward != 0)
        {
            Managers.Inven.Add(itemReward);
        }
        Managers.UI.ShowPopupUI<WarningText>().Set_WarningText($"{Name}퀘스트 완료.", Color.green);
        Managers.Sound.Play($"QuestClear");
        Managers.Event.MonsterDie -= EnemyDiedEvent;
    }

    protected override void QuestActive()
    {
        Managers.Event.MonsterDie += EnemyDiedEvent;
    }

    private void EnemyDiedEvent(int monstercode)
    {
        if (monstercode == _enemyToTargetCode)
        {
            this._actualEnemiesDestroyed++;
        }

        if (_actualEnemiesDestroyed == NumberOfEnemiesToDestroy)
        {
            this.Complete();
        }
    }
}