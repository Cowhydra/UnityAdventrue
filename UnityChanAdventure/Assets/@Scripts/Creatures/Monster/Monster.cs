using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Creature, IDamage, IAttack
{
    public int MyCode;
    private Define.MonsterAttackType _attackType;
    private Animator _animator;
    public int AttackRange { get; set; }
    public int Hp
    {
        get { return _hp; }
        set
        {
            _hp = value;
            if (_hp < 0)
            {
                Die();
            }


            Debug.Log("Hp ���� �̺�Ʈ");
        }
    }
    public float MyHpRatio()
    {
        return _hp / _maxhp;
    } 
    public void MyAttack(Define.MonsterAttackType attacktype)
    {

    }
    private void Awake()
    {
        MyCode = int.Parse(gameObject.name);
        _animator = GetComponent<Animator>();
        Init();

     
    }
    public void OnDamage(int damage)
    {
        Hp -= Math.Max(1, damage - _level - _def);
        _animator.SetTrigger("Damage");
    }

    private void Init()
    {
        _hp = Managers.Data.MonsterDataDict[MyCode].maxhp;
        _def = Managers.Data.MonsterDataDict[MyCode].def;
        _magicdef = (int)UnityEngine.Random.Range(_def - 10, _def + 10);
        _attack = Managers.Data.MonsterDataDict[MyCode].attack;
        _level = Managers.Data.MonsterDataDict[MyCode].level;
        _attackType = Managers.Data.MonsterDataDict[MyCode].AttackType;
        if (_attackType == Define.MonsterAttackType.RangeAttack)
        {
            AttackRange = UnityEngine.Random.Range(10, Mathf.Max(10 + _level, 22));
        }
    }

    private void OnEnable()
    {
        isDie = false;
        _hp = _maxhp;
        StartCoroutine(nameof(HpRegen_co));
        _hpregen = _level * 2;
    }
    public override void Die()
    {
        base.Die();
        GameObject Gold = Managers.Resource.Instantiate("Gold");
        Gold.GetComponent<Gold>().SetValue(_level);
        GameObject Exp = Managers.Resource.Instantiate("Exp");
        Exp.GetComponent<Exp>().SetValue(_level);
        _animator.SetTrigger("Die");
        Debug.Log("���� ó��");
    }
    private IEnumerator HpRegen_co()
    {
        Hp += _level * 5 + UnityEngine.Random.Range(5, 10);
        yield return new WaitForSeconds(_healthRegenDelay);
    }


    private IEnumerator MonDie_Co()
    {
        yield return new WaitForSeconds(2.0f);
        Managers.Resource.Destroy(gameObject);
    }


}
