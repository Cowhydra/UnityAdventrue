using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Creature, IDamage
{
    public int MyCode;
    private Define.MonsterAttackType _attackType;
    private Animator _animator;
    private SkinnedMeshRenderer meshRenderer;
    private Color origihmeshcolor;
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


            Debug.Log("Hp 관련 이벤트");
        }
    }
    public float MyHpRatio()
    {
        return _hp / _maxhp;
    } 

    private void Awake()
    {
        MyCode = int.Parse(gameObject.name.Substring(0,4));
        _animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        origihmeshcolor=meshRenderer.material.color;    
        Init();
        _animator.runtimeAnimatorController = Managers.Resource.Load<RuntimeAnimatorController>(Managers.Data.MonsterDataDict[MyCode].prefabPath+"_anim");

    }
    public void OnDamage(int damage)
    {
        Hp -= Math.Max(1, damage - _level - _def);
        _animator.SetTrigger("Damage");
        StartCoroutine(nameof(OnHitColor));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
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
        _hpregen = _level * 2;

        gameObject.GetOrAddComponent<LowAI_BT>().enabled = true;

    }
    public override void Die()
    {
        base.Die();
        //골드 및 경험 치 생성
        GameObject Gold = Managers.Resource.Instantiate("Gold");
        Gold.GetComponent<Gold>().SetValue(_level);
        Gold.transform.position = gameObject.transform.position;
        GameObject Exp = Managers.Resource.Instantiate("Exp");
        Exp.GetComponent<Exp>().SetValue(_level);
        Exp.transform.position = gameObject.transform.position;
        //퀘스트 관련 몬스터 이벤트
        Managers.Event.MonsterDie?.Invoke(MyCode);

        if (Util.Probability(30))
        {
            //아이템 생성
            int itemcode=Managers.Data.ItemCodes[UnityEngine.Random.Range(0, Managers.Data.ItemCodes.Count)];
            GameObject dropitem= Managers.Resource.Instantiate("Itemoutside");
            dropitem.transform.position = gameObject.transform.position;
            dropitem.GetOrAddComponent<Itemoutside>().ItemCode = itemcode;

        }
        gameObject.GetOrAddComponent<LowAI_BT>().enabled = false;
        StartCoroutine(nameof(Monster_Die));
        
       
    }
    private IEnumerator Monster_Die()
    {
        _animator.SetTrigger("Die");
        yield return new WaitForSeconds(3.0f);
      Managers.Resource.Destroy(gameObject);
    }


    private IEnumerator OnHitColor()
    {
        meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        meshRenderer.material.color = origihmeshcolor;
    }
}

