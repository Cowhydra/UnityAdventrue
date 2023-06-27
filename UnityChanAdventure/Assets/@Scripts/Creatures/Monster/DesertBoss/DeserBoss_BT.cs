using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System;

public class DeserBoss_BT : Behavior_Tree
{
    [SerializeField]

    //static을 쓰지 않으려면 전부 데이터 매니저로 각각 트리 안에서 해당 수치들을 가져와야함


    public static float speed = 2f;
    public static float fovRange = 6f;

    public static float longAttackRange = 15f;
    public static float shortattackRange = 5f;

    private static float height = 6f;
   

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _fovRange;
    [SerializeField]
    private float _shortattackRange;
    [SerializeField]
    private float _longAttackRange;

    [SerializeField]
    private GameObject Projectile1;
    [SerializeField]
    private GameObject Projectile2;
    //현재 Desert Boss 만 처리할려고 만들었으나, 추후 Boss 코드에 따라 실행코드를
    //다르게 하여 스크립트를 적게 사용할 수 있음

    protected override void Start()
    {
        base.Start();
        speed = _speed;
        fovRange = _fovRange;
        shortattackRange = _shortattackRange;
        longAttackRange=_longAttackRange;


    }

    protected override Behavior_Node SetupTree()
    {

        Behavior_Node root = new Behavior_Selector(new List<Behavior_Node>
        {
            new Behavior_Sequence(new List<Behavior_Node>
            {
               new CheckMyHp(transform),
               new LazyPattern(transform)
            }),

            new Behavior_Sequence(new List<Behavior_Node>
            {
               new CheckEnemyInRange(transform,Define.BossDistance.Long),
               new LongAttack(transform),
            }),
             new Behavior_Sequence(new List<Behavior_Node>
            {
                   new CheckEnemyInRange(transform,Define.BossDistance.Short),
                   new ShortAttack(transform)
            }),
             new Behavior_Sequence(new List<Behavior_Node>
             {
                 new CheckEnemyInRange(transform,Define.BossDistance.Medium),
                 new MiddleAttack(transform)
             }),
             new FindTarget(transform),
             new TaskPatrol(transform)


        }) ;

        return root;
    }
}
