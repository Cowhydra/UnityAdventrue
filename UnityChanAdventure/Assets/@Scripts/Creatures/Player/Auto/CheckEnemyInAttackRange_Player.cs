using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInAttackRange_Player : Behavior_Node
{
    private Transform _transform;
    private Animator _animator;

    public CheckEnemyInAttackRange_Player(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();

    }

    public override Define.Behavior_NodeState Evaluate()
    {

        object t = GetData("target");
        if (t == null)
        {
            state = Define.Behavior_NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if (Vector3.Distance(_transform.position, target.position) <= AutoFight_BT.attackRange*2)
        {
            state = Define.Behavior_NodeState.SUCCESS;
            return state;
        }
        state = Define.Behavior_NodeState.FAILURE;
        return state;
    }
}
