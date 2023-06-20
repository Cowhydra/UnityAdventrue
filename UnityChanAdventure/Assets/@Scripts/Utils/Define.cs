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


    }

    public enum SortingOrder
    {

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
    public enum ESkillType
    {
        None,
        Basic,
        QSKill,
        WSkill,
        ESkill,
        RSkill,

    }
    public enum EGameState
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
    public enum EDamageType
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
