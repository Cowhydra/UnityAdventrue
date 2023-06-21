using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
//https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
public class LowAI_BT : Behavior_Tree
{
    //Guard
    public UnityEngine.Transform[] waypoints;


    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 2f;

    protected override Behavior_Node SetupTree()
    {

        Behavior_Node root = new Behavior_Selector(new List<Behavior_Node>
        { 
            new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(transform),
            }),
             new Behavior_Sequence(new List<Behavior_Node>
            {
                new CheckEnemyInFovRange(transform),
                new TaskGotoTarget(transform),
            }),
            new TaskPatrol(transform, waypoints),
        });

        return root;
    }
}
