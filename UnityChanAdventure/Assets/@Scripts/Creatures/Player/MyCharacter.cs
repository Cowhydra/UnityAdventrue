using Data;
using System;
using System.Collections;
using UnityEngine;

public class MyCharacter : Creature, IDamage, IListener
{
    private int myCharacterCode;
    private Animator _animator;

    private bool ishitted;
    [SerializeField]
    private float ishittedcooltime = 8.0f;
    #region 스텟
    public int MaxHp { get { return _maxhp; } private set { _maxhp = value; } }
    public int MaxMana { get { return _maxmana; } private set { _maxmana = value; } }
    public int MagicDef { get { return _magicdef; } private set { _magicdef = value; } }
    public int Def { get { return _def; } private set { _def = value; } }
    public int MagicAttack { get { return _magicattack; } private set { _magicattack = value; } }
    public int Attack { get { return _attack; } private set { _attack = value; } }
    public int AttackSpeed { get { return _attackspeed; } private set { _attackspeed = value; } }
    public string Name { get { return Managers.Data.CharacterDataDict[myCharacterCode].name; } private set { } }
    public int Level
    {
        get { return _level; }
        private set
        {
            _level = value;
            Managers.Data.CharacterDataDict[Managers.Game.currentCharNumber].level = _level;
            Debug.Log("캐릭터 레벨업 하면 DB 갱신 ");
            InitCharacter();
        }
    }
    public Define.MonsterAttackType EnemyAttackType = Define.MonsterAttackType.Melee;
    private int _mana;
    public int Mana
    {
        get { return _mana; }
        set
        {
            _mana = Math.Clamp(value, 0, _maxmana);
        }
    }
    private int _exp;
    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;
            if (_exp > RequireExp)
            {
                Level++;
                _exp -= RequireExp;

                Debug.Log("캐릭터 경험치를 획득하게 되면 DB를 갱신해야 합니다.");
            }

        }
    }
    public int Hp
    {
        get { return _hp; }
        set
        {
            _hp = Math.Clamp(value, 0, _maxhp);
            if (_hp < 0)
            {
                Die();
            }
            Managers.Event.PostNotification(Define.EVENT_TYPE.PlayerStatsChange, this);
            Debug.Log(_hp);
        }
    }
    public int RequireExp { get { return Level * 20 + 8; } }


    public void DieStatSet()
    {
        Hp = 50;
        _mana = 0;
        isDie = false;
    }

    private void InitCharacter()
    {
        if (!Managers.Data.CharacterDataDict.ContainsKey(myCharacterCode))
        {
            _maxhp = 1000;
            _maxmana = 100;
            _attack = 50;
            return;
        }

        _maxhp = Managers.Data.CharacterDataDict[myCharacterCode].maxhp + Managers.EQUIP.EQUIP_MaxHp;
        _maxmana = Managers.Data.CharacterDataDict[myCharacterCode].maxmana + Managers.EQUIP.EQUIP_MaxMp;
        _hp = _maxhp;
        _mana = _maxmana;
        _def = Managers.Data.CharacterDataDict[myCharacterCode].def + Managers.EQUIP.EQUIP_Def + (Level +1) * 5;
        _magicdef = Managers.Data.CharacterDataDict[myCharacterCode].magicdef + Managers.EQUIP.EQUIP_MagicDef + (Level + 1) * 5;
        _magicattack = Managers.Data.CharacterDataDict[myCharacterCode].magicattack + Managers.EQUIP.EQUIP_MagicAttack + (Level + 1) * 5;
        _attack = Managers.Data.CharacterDataDict[myCharacterCode].attack + Managers.EQUIP.EQUIP_Attack + (Level +1) * 5;
        _attackspeed = Managers.Data.CharacterDataDict[myCharacterCode].attackspeed;
        _level = Managers.Data.CharacterDataDict[myCharacterCode].level;
        Managers.Event.PostNotification(Define.EVENT_TYPE.PlayerStatsChange, this);
    }
    private void CharacterEQUIPStatsChange()
    {
        _maxhp = Managers.Data.CharacterDataDict[myCharacterCode].maxhp + Managers.EQUIP.EQUIP_MaxHp;
        _maxmana = Managers.Data.CharacterDataDict[myCharacterCode].maxmana + Managers.EQUIP.EQUIP_MaxMp;
        _def = Managers.Data.CharacterDataDict[myCharacterCode].def + Managers.EQUIP.EQUIP_Def + (Level - 1) * 10;
        _magicdef = Managers.Data.CharacterDataDict[myCharacterCode].magicdef + Managers.EQUIP.EQUIP_MagicDef + (Level - 1) * 10;
        _magicattack = Managers.Data.CharacterDataDict[myCharacterCode].magicattack + Managers.EQUIP.EQUIP_MagicAttack + (Level - 1) * 10;
        _attack = Managers.Data.CharacterDataDict[myCharacterCode].attack + Managers.EQUIP.EQUIP_Attack + (Level - 1) * 10;
        Managers.Event.PostNotification(Define.EVENT_TYPE.PlayerStatsChange, this);
    }
    #endregion
    public void OnDamage(int damage)
    {
      
        Debug.Log("Hit Animator");
        if (!ishitted)
        {
            Hp -= Math.Max(0, damage - _level - Def);
            ishitted = true;
            StartCoroutine(nameof(HitAnimation_co), damage);
        }
        Debug.Log("공격받음");
    }
    IEnumerator HitAnimation_co(int damage)
    {
        if (!ishitted)
        {
            _animator.SetFloat("Damaged", damage - _level - Def / damage);
            _animator.SetTrigger("Damaged");
            yield return new WaitForSeconds(ishittedcooltime);
            ishitted = false;
        }

    }
   public override void Die()
    {
        base.Die();
    }
    public void Start()
    {
        //멀티 고려시, 캐릭터 코드 혹은, 계정 넘버 가지고 있어야할듯?
        myCharacterCode = Managers.Game.currentCharNumber;
        InitCharacter();
        _animator=GetComponent<Animator>();
        StartCoroutine(nameof(Regenerat_co));

    }
    protected IEnumerator Regenerat_co()
    {
        while (true)
        {
            Mana += 5;
            Hp += 5;
            yield return new WaitForSeconds(_healthRegenDelay);
        }
    }

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case Define.EVENT_TYPE.PlayerEquipChanageUI:
                CharacterEQUIPStatsChange();
                break;
        }
    }


}
