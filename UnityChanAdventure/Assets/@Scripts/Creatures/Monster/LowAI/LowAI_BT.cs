using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

//https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
public class LowAI_BT : Behavior_Tree
{

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 2f;


    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _fovRange;
    [SerializeField]
    private float _attackRange;

    [Header("발사체가 있는 몬스터는 여기")]
    [SerializeField]
    private GameObject MyProjectile;
    protected override void Start()
    {
        base.Start();
        speed = _speed;
        fovRange = _fovRange;
        attackRange = _attackRange;
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
