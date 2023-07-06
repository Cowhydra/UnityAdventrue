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
    private int _attackRange;
    public int AttackRange
    {
        get
        {
            return _attackRange;
        }
        private set
        {
            _attackRange = value;
        }
    }
    private int _movespeed { get; set; }
    public int MoveSpeed
    {
        get
        {
            return _movespeed;
        }
       private set
        {
            _movespeed = value;
        }
    }
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
    public int Attack
    {
        get { return _attack; }
        private set
        {
            _attack = value;
        }
    }
    private int _fovRange;
    public int FovRange
    {
        get { return _fovRange; }
        private set
        {
            _fovRange = value;
        }
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
        _movespeed = Managers.Data.MonsterDataDict[MyCode].speed;
        _fovRange = Managers.Data.MonsterDataDict[MyCode].fovRange;
        _attackRange = Managers.Data.MonsterDataDict[MyCode].attackRange;
    }

    private void OnEnable()
    {
        isDie = false;
        _hp = _maxhp;
        _hpregen = _level * 2;
        if (MyCode % 10 != 0) 
        {
            gameObject.GetOrAddComponent<LowAI_BT>().enabled = true;
        }


    }
    public override void Die()
    {
        base.Die();
        //골드 및 경험 치 생성
        GameObject Gold = Managers.Resource.Instantiate("Gold");
        Gold.GetComponent<Gold>().SetValue(_level,gameObject.transform);
        GameObject Exp = Managers.Resource.Instantiate("Exp");
        Exp.GetComponent<Exp>().SetValue(_level,gameObject.transform);
       
        //퀘스트 관련 몬스터 이벤트
        Managers.Event.MonsterDie?.Invoke(MyCode);

        if (Util.Probability(30))
        {
            //아이템 생성
            int itemcode=Managers.Data.ItemCodes[UnityEngine.Random.Range(0, Managers.Data.ItemCodes.Count)];
            GameObject dropitem= Managers.Resource.Instantiate("Itemoutside");
            dropitem.transform.position = gameObject.transform.position+Vector3.up;
            dropitem.GetOrAddComponent<Itemoutside>().ItemCode = itemcode;

        }
        if (MyCode % 10 != 0)
        {
            gameObject.GetOrAddComponent<LowAI_BT>().enabled = false;
        }
        StopAllCoroutines();
        StartCoroutine(nameof(Monster_Die));
        
       
    }
    private IEnumerator Monster_Die()
    {

        yield return new WaitForSeconds(1.0f);
        _animator.SetTrigger("Die");
        yield return new WaitForSeconds(1.0f);
        Managers.Resource.Destroy(gameObject);
    }
    public float MyHpRatio()
    {
        return _hp / _maxhp;
    }
    public void GoAttack(Transform transform)
    {
        if (_attackType == Define.MonsterAttackType.Melee)
        {
            transform.GetComponent<IDamage>().OnDamage(Attack);
        }
        else
        {
            GameObject Projectile = Managers.Resource.Instantiate($"{gameObject.name}_Projectile");
            Projectile.GetOrAddComponent<MonProjectileController>().SetProjectile(gameObject.transform.position, 10);
        }
    }

}

