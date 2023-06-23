public class Define
{


    public enum Scene
    {
        None,
        LoadingScene,
        LoginScene,
        TownScene,
        CharacterSelectScene,
        Lava_Dungeon,
        TestScene,



    }

    public enum SortingOrder
    {
        ShopUI=30,

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
        PlayerEquipChanage,

        InventoryOpen,
        InventoryClose,
        InventoryItemSelect,
        GoodsChange,
        ShopOpen,
        ShopClose,


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
        Enviroment = 6,
        Enemy=8,
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
