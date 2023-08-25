public class Define
{
    public enum LoginType
    {
        None,
        Email,
        Google,
        PlayGames,
        Anomynous,

    }
    public enum CameraFov
    {
        Default=30,
        WaterScene=65,

    }
   public enum CinemachinePriority
    {

        PlayerMain_cm=25,
        Boss_com=26,
        PlayerSub_cm=20,

    }

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
        JoyStick=1,
        ShopUI=10,
        InvenItem=20,
        SKillBook=25,
        QuestInfo=99,
        DialogSystem=100,

    }
    #region About Event
    public enum KeyInput
    {
        Jump,
        Sprint,
        Attack,
        Auto,

    }
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
        DialogClose,

        


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
        None,
        QuestNpc,
        ShopNpc,
        EnhanceNpc,
        TuotorialNpc,
        Boss,
        Dungeon,
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
        QSkill,
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
        None,
        Desert,
        Forest,
        Water,
        Lava,
    }
    public enum BossAttack_Number
    {
        LongRange=25,
        ShortRange=10,
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
    public enum Update_DB_Goods
    {
        BlueDiamond,
        Gold,
        RedDiamond,
    }
    public enum Update_DB_EQUIPType
    {
        Weapon,
        Hat,
        Cloth,
        Boot,
        Earring,
        Ring,

    }
    #endregion
}
