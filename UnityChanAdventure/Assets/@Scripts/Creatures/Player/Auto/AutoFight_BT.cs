using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AutoFight_BT : Behavior_Tree
{
    public static int speed;
    public static int fovRange;
    public static int attackRange;
    public static int attackdamage;
    private PlayerController _myCharacter;



    //���� ���� �� �ؾ� ����  ĳ���� ��Ʈ�ѷ� ���� �ѱ� + �ִϸ��̼�, �⺻ �̵� ���踦 .. ĳ���� ��Ʈ�ѷ��� �س���
    //���� ������ �ٽ� Navi�� �ٲٴ°� �̻��� 
    protected override void Start()
    {

        _myCharacter = GetComponent<PlayerController>();
        speed = 20;
        fovRange = 10;
        attackRange = 2;
        attackdamage = 25;
        base.Start();
    }
    protected override Behavior_Node SetupTree()
    {

        Behavior_Node root = new Behavior_Selector(new List<Behavior_Node>
        {
            new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInAttackRange_Player(transform),
                new TaskAttack_Player(transform,_myCharacter)
            
            }),
             new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInFovRange_Player(transform),
                new TaskGotoTarget(transform)
              
            }),

            new TaskPatorl_Player(transform),
        });

        return root;
    }
}
