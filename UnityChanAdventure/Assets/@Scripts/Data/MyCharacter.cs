using Data;
using System;




public class MyCharacter
{
    public Action InvenUI_ReFresh;
    // CharacterData 의  내 캐릭터 정보를 담은 class 에서 Mycharcter 로 옮기는 과정
    public CharacterData CharacterDataInfo { get; } = new CharacterData();
    public int CharacterCode { get { return CharacterDataInfo.charatercode; } set { CharacterDataInfo.charatercode = value; } }
    private Equipment _Equip;
    public Equipment EQUIP { get { return _Equip; } set { _Equip = value; } }

    public string JobType { get { return CharacterDataInfo.jobType; } set { CharacterDataInfo.jobType = value; } }
    public int MaxHp { get { return CharacterDataInfo.maxhp + EQUIP.EQUIP_MaxHp; } set { CharacterDataInfo.maxhp = value; } }

    private int CurrentHp;
    public int Hp { get { return CurrentHp; } set { CurrentHp = value; } }
    private int CurrentMana;
    public int Mana { get { return CurrentMana; } set { CurrentMana = value; } }
    public int MaxMana { get { return CharacterDataInfo.maxmana + EQUIP.EQUIP_MaxMp; } set { CharacterDataInfo.maxmana = value; } }
    public int Limit { get { return CharacterDataInfo.limit; } set { CharacterDataInfo.limit = value; } }
    public int MagicDef { get { return CharacterDataInfo.magicdef + EQUIP.EQUIP_MagicDef + (Level - 1) * 10; } set { CharacterDataInfo.magicdef = value; } }
    public int Def { get { return CharacterDataInfo.def + EQUIP.EQUIP_Def + (Level - 1) * 10; } set { CharacterDataInfo.def = value; } }
    public int MagicAttack { get { return CharacterDataInfo.magicattack + EQUIP.EQUIP_MagicAttack + (Level - 1) * 10; } set { CharacterDataInfo.magicattack = value; } }
    public int Attack { get { return CharacterDataInfo.attack + EQUIP.EQUIP_Attack + (Level - 1) * 10; } set { CharacterDataInfo.attack = value; } }
    public int AttackSpeed { get { return CharacterDataInfo.attackspeed; } set { CharacterDataInfo.attackspeed = value; } }
    public int AttackRange { get { return CharacterDataInfo.attackrange + EQUIP.EQUIP_AttackRange; } set { CharacterDataInfo.attackrange = value; } }
    public string SkillScript { get { return CharacterDataInfo.Skill; } set { CharacterDataInfo.Skill = value; } }
    public string IconPath { get { return CharacterDataInfo.iconPath; } set { CharacterDataInfo.iconPath = value; } }
    public string CardPath { get { return CharacterDataInfo.cardPath; } set { CharacterDataInfo.cardPath = value; } }
    public bool IsActive { get { return CharacterDataInfo.isActive; } set { CharacterDataInfo.isActive = value; } }
    public string Name { get { return CharacterDataInfo.name; } set { CharacterDataInfo.name = value; } }
    public int Level { get { return CharacterDataInfo.level; } set { CharacterDataInfo.level = value; } }
    public string TMI { get { return CharacterDataInfo.tmi; } set { CharacterDataInfo.tmi = value; } }
    private int _exp;
    public int Exp
    {
        get { return _exp; }
        set
        {
            if (Exp > RequireExp)
            {
                Level++;
                Exp -= RequireExp;
            }
            _exp = value;
        }
    }
    public int RequireExp { get { return Level * 20 + 8; } }


    public void StatInit()
    {
        Hp = MaxHp;
        Mana = 0;



    }

    public static MyCharacter MakeCharacter(CharacterData characterDataInfo)
    {

        CharacterData characterData = null;
        MyCharacter myCharacter = null;

        Managers.Data.CharacterDataDict.TryGetValue(characterDataInfo.charatercode, out characterData);

        if (characterData == null) return null;

        myCharacter = new MyCharacter();
        myCharacter.Mana = characterDataInfo.maxmana;
        myCharacter.MaxMana = characterDataInfo.maxmana;
        myCharacter.Hp = characterDataInfo.maxhp;
        myCharacter.MaxHp = characterDataInfo.maxhp;
        myCharacter.Attack = characterDataInfo.attack;
        myCharacter.AttackRange = characterDataInfo.attackrange;
        myCharacter.CardPath = characterDataInfo.cardPath;
        myCharacter.IconPath = characterDataInfo.iconPath;
        myCharacter.CharacterCode = characterDataInfo.charatercode;
        myCharacter.AttackSpeed = characterDataInfo.attackspeed;
        myCharacter.SkillScript = characterDataInfo.Skill;
        myCharacter.MagicAttack = characterDataInfo.magicattack;
        myCharacter.JobType = characterDataInfo.jobType;
        myCharacter.Def = characterDataInfo.def;
        myCharacter.MagicDef = characterDataInfo.magicdef;
        myCharacter.IsActive = characterDataInfo.isActive;
        myCharacter.Level = characterDataInfo.level;
        myCharacter.Name = characterDataInfo.name;
        myCharacter.TMI = characterDataInfo.tmi;
        myCharacter.Limit = characterDataInfo.limit;
        myCharacter.EQUIP = new Equipment(characterDataInfo.charatercode);
        return myCharacter;
    }






}
