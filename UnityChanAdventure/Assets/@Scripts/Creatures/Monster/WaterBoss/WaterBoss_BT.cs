using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class WaterBoss_BT : Behavior_Tree
{
    [SerializeField]

    //static�� ���� �������� ���� ������ �Ŵ����� ���� Ʈ�� �ȿ��� �ش� ��ġ���� �����;���


    public static float speed = 2f;
    public static float fovRange = 6f;



    [SerializeField]
    private float _shortattackRange;
    [SerializeField]
    private float _longAttackRange;

 
    //���� Desert Boss �� ó���ҷ��� ���������, ���� Boss �ڵ忡 ���� �����ڵ带
    //�ٸ��� �Ͽ� ��ũ��Ʈ�� ���� ����� �� ����

    protected override void Start()
    {
        base.Start();



    }

    protected override Behavior_Node SetupTree()
    {

        Behavior_Node root = new Behavior_Selector(new List<Behavior_Node>
        {
            new Behavior_Sequence(new List<Behavior_Node>
            {
               new CheckMyHp(transform,Define.BossHp.Low),
             
            }),

            new Behavior_Sequence(new List<Behavior_Node>
            {
               new CheckMyHp(transform,Define.BossHp.Middle),

            }),
            new Behavior_Sequence(new List<Behavior_Node>
            {
               new CheckMyHp(transform,Define.BossHp.High),

            }),

             new FindTarget(transform),


        });

        return root;
    }
}
