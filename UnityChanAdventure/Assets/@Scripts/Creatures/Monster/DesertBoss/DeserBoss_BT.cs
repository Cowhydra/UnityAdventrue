using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System;

public class DeserBoss_BT : Behavior_Tree
{
    [SerializeField]

    //static�� ���� �������� ���� ������ �Ŵ����� ���� Ʈ�� �ȿ��� �ش� ��ġ���� �����;���
    public static float longAttackRange = 15f;
    public static float shortattackRange = 5f;

    [SerializeField]
    private float _shortattackRange;
    [SerializeField]
    private float _longAttackRange;

 
    //���� Desert Boss �� ó���ҷ��� ���������, ���� Boss �ڵ忡 ���� �����ڵ带
    //�ٸ��� �Ͽ� ��ũ��Ʈ�� ���� ����� �� ����

    protected override void Start()
    {
       
        shortattackRange = _shortattackRange;
        longAttackRange=_longAttackRange;

        base.Start();
    }

    protected override Behavior_Node SetupTree()
    {

        Behavior_Node root = new Behavior_Selector(new List<Behavior_Node>
        {
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
