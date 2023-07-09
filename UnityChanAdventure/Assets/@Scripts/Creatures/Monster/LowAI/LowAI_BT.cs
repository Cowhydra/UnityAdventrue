using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
public class LowAI_BT : Behavior_Tree
{

    public static int speed ;
    public static int fovRange ;
    public static int attackRange ;
    public static int attackdamage;
    private Define.MonsterAttackType myAttackType;
    private Monster _monster;
    protected override void Start()
    {
       
        _monster = GetComponent<Monster>();
        speed = _monster.MoveSpeed;
        fovRange = _monster.FovRange;
        attackRange=_monster.AttackRange;
        attackdamage = _monster.Attack;
        base.Start();
    }
    protected override Behavior_Node SetupTree()
    {

        Behavior_Node root = new Behavior_Selector(new List<Behavior_Node>
        { 
            new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(transform,_monster),
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
