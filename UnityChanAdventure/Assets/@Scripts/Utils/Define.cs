public class Define
{


    public enum Scene
    {
        None,
        LoadingScene,
        LoginScene,
        TownScene,
        CharacterSelectScene,


        TestScene,
        LavaScene,
        DesertScene,
        WaterScene,
        FightScene,


    }

    public enum SortingOrder
    {
        ShopUI=10,
        InvenItem=20,
        DialogSystem=100,

    }
    #region About Event
    public enum UIEvent
    {
        Click,
        OnDrag,
        PointerEnter,
        PointerExit,
        OnBeginDrag,
        OnEndDrag,
        OnDrop

    }
    public enum EVENT_TYPE
    {
        SelectCharacter,

        PlayerStatsChange,
        PlayerEquipChanageUI,

        InventoryOpen,
        InventoryClose,
        InventoryItemSelect,
        GoodsChange,
        ShopOpen,
        ShopClose,

        DialogOpen,
        DialogClose


    }
    public enum Login_Event_Type
    {
        LoginSucess,
        LoginFail_ID_NotFound,
        LoginFail_PW_Wrong,
        CreateAccount_Sucess,
        CreateAccount_Fail_IDSame,
        LoginNotBlink,
    }
    #endregion
    #region GameContents
    public enum Npc_Type 
    { 
        Shop,
        Enhance,
        Quest,
    }
    public enum TalkingBar_Type
    {
        None,
        QuestNpc,
        ShopNpc,
        EnhanceNpc,
        TuotorialNpc,
    }
    public enum BossDistance
    {
        Long,
        Short,
        Medium

    }
    public enum BossHp
    {
        Low,
        Middle,
        High,
    }
   public enum MonsterAttackType
    {
        Melee,
        RangeAttack,
        All,

    }
    public enum QuestState
    {
        Pending,
        Active,
        Completed
    }
    public enum QuestType
    {
        DefeatEnemy,
        CollectItem,

    }

    public enum Behavior_NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public enum Goods
    {
        Gold,
        BlueDiamond,
        RedDiamond,

    }

    public enum SkillType
    {
        None,
        Basic,
        QSKill,
        WSkill,
        ESkill,
        RSkill,

    }
    public enum GameState
    {
        None,
        EnterDungeon,
        ActiveBoss,
        TimeOut,
        KillBoss,
        ExitDungeon,

    }
    public enum LayerMask
    {
        Enviroment = 7,
        Player=6,
        Enemy=8,
        DestoryableEnv=9,
        Skill = 10
    }
    public enum DamageType
    {
        Nomal,
        Cirtical,
        Item,
        Heal,
    }
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
    public enum ItemType
    {
        Weapon,
        Hat,
        Cloth,
        Boot,
        Earring,
        Ring,
        Consume,
        Ingredient,
        Max,

    }
    public enum ItemGrade
    {
        common
       ,Uncommon
       ,Rare
       ,Relic
       ,Chronicle
       ,Unique
       ,Legendary
       ,Epic
       ,RelicEpic
       ,Mythic

    }
    public enum MonsterEnvType
    {
        Desert,
        Forest,
        Water,
        Lava,
    }
    #endregion
    #region AboutDB
    public enum Update_DB_Item
    {
      count,
      Enhancement,

    }
    public enum Update_DB_Character
    {
        level,
        exp,

    }
    public enum Update_DB_EQUIPType
    {
        Boot,
        Cloth,
        Weapon,
        Earring,
        Ring,
        Hat,

    }
    #endregion
}
