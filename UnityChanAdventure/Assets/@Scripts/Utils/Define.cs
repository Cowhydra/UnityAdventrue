public class Define
{


    public enum Scene
    {
        None,
        LoadingScene,
        LoginScene,
        LobbyScene,
        CharacterSelectScene,
        Lava_Dungeon,


    }

    public enum SortingOrder
    {

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
        LoginSucess,
        LoginFail_ID_NotFound,
        LoginFail_PW_Wrong,
        CreateAccount_Sucess,
        CreateAccount_Fail_IDSame,


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
        Enviroment=6,
        Skill=10
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
    public enum UpdateDataTyoe
    {
        UpdateDB_CharacterLevel,
        UpdateDB_ItemCount,
        UpdateDB_ItemEnhance,
    }

}
