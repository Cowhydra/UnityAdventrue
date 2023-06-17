using Data;
using System;



public class MyCharacter
{

    public int AccountNumber { get; set; }
    // CharacterData 의  내 캐릭터 정보를 담은 class 에서 Mycharcter 로 옮기는 과정
    public CharacterData CharacterDataInfo { get; } = new CharacterData();
    public int CharacterCode { get { return CharacterDataInfo.charcode; } set { CharacterDataInfo.charcode = value; } }
    private Equipment _Equip;
    public Equipment EQUIP { get { return _Equip; } set { _Equip = value; } }

    private Inventory _inven;
    public Inventory Inven { get { return _inven; } set { _inven = value; } }


    public string JobType { get { return CharacterDataInfo.jobType; } set { CharacterDataInfo.jobType = value; } }
    public int MaxHp { get { return CharacterDataInfo.maxhp + EQUIP.EQUIP_MaxHp; } set { CharacterDataInfo.maxhp = value; } }

    private int CurrentHp;
    public int Hp { get { return CurrentHp; } set { CurrentHp = value; } }
    private int CurrentMana;
    public int Mana { get { return CurrentMana; } set { CurrentMana = value; } }
    public int MaxMana { get { return CharacterDataInfo.maxmana + EQUIP.EQUIP_MaxMp; } set { CharacterDataInfo.maxmana = value; } }
    public int MagicDef { get { return CharacterDataInfo.magicdef + EQUIP.EQUIP_MagicDef + (Level - 1) * 10; } set { CharacterDataInfo.magicdef = value; } }
    public int Def { get { return CharacterDataInfo.def + EQUIP.EQUIP_Def + (Level - 1) * 10; } set { CharacterDataInfo.def = value; } }
    public int MagicAttack { get { return CharacterDataInfo.magicattack + EQUIP.EQUIP_MagicAttack + (Level - 1) * 10; } set { CharacterDataInfo.magicattack = value; } }
    public int Attack { get { return CharacterDataInfo.attack + EQUIP.EQUIP_Attack + (Level - 1) * 10; } set { CharacterDataInfo.attack = value; } }
    public int AttackSpeed { get { return CharacterDataInfo.attackspeed; } set { CharacterDataInfo.attackspeed = value; } }
    public string IconPath { get { return CharacterDataInfo.iconPath; } set { CharacterDataInfo.iconPath = value; } }
    public string Name { get { return CharacterDataInfo.name; } set { CharacterDataInfo.name = value; } }
    public int Level { get { return CharacterDataInfo.level; } set { CharacterDataInfo.level = value; } }

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

        Managers.Data.CharacterDataDict.TryGetValue(characterDataInfo.charcode, out characterData);
        if (characterData == null) return null;
        myCharacter = new MyCharacter();
        myCharacter.Mana = characterDataInfo.maxmana;
        myCharacter.MaxMana = characterDataInfo.maxmana;
        myCharacter.Hp = characterDataInfo.maxhp;
        myCharacter.MaxHp = characterDataInfo.maxhp;
        myCharacter.Attack = characterDataInfo.attack;
        myCharacter.IconPath = characterDataInfo.iconPath;
        myCharacter.CharacterCode = characterDataInfo.charcode;
        myCharacter.AttackSpeed = characterDataInfo.attackspeed;
        myCharacter.MagicAttack = characterDataInfo.magicattack;
        myCharacter.JobType = characterDataInfo.jobType;
        myCharacter.Def = characterDataInfo.def;
        myCharacter.MagicDef = characterDataInfo.magicdef;
        myCharacter.Level = characterDataInfo.level;
        myCharacter.Name = characterDataInfo.name;
        myCharacter.EQUIP = new Equipment();
        myCharacter.Inven = new Inventory();
        return myCharacter;
    }






}
