using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
public class LowAI_BT : Behavior_Tree
{

    public static float speed ;
    public static float fovRange ;
    public static float attackRange ;


    [Header("�߻�ü�� �ִ� ���ʹ� ����")]
    [SerializeField]
    private GameObject MyProjectile;
    protected override void Start()
    {
        base.Start();
        speed = Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].speed;
        fovRange = Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].fovRange;
        attackRange = Managers.Data.MonsterDataDict[GetComponent<Monster>().MyCode].attackRange;
    }
    protected override Behavior_Node SetupTree()
    {

        Behavior_Node root = new Behavior_Selector(new List<Behavior_Node>
        { 
            new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(transform,MyProjectile),
            }),
             new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInFovRange(transform),
                new TaskGotoTarget(transform),
            }),
           
            new TaskPatrol(transform),
        });

        return root;
    }
}
