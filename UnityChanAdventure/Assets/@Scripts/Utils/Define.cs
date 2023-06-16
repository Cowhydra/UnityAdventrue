public class Define
{


    public enum Scene
    {
        None,
        LoadingScene,
        Title,
        Lobby,
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


    }

    public enum ESkillType
    {
        None,
        Basic,
        Q_SKill,
        W_Skill,
        E_Skill,
        R_Skill,

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

}
