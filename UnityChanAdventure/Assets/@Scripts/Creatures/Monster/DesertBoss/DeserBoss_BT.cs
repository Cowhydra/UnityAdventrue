using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System;

public class DeserBoss_BT : Behavior_Tree
{
    [SerializeField]

    //static�� ���� �������� ���� ������ �Ŵ����� ���� Ʈ�� �ȿ��� �ش� ��ġ���� �����;���


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
    //���� Desert Boss �� ó���ҷ��� ���������, ���� Boss �ڵ忡 ���� �����ڵ带
    //�ٸ��� �Ͽ� ��ũ��Ʈ�� ���� ����� �� ����

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
