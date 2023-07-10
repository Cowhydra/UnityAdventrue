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

        //t가 있었으나 찰나의 순간 사라지는거 방지용 
        Transform target = (Transform)t;
        if(target == null)
        {
            state = Define.Behavior_NodeState.FAILURE;
            return state;
        }
        if (Vector3.Distance(_transform.position, target.position) <= AutoFight_BT.attackRange*2+1)
        {

            state = Define.Behavior_NodeState.SUCCESS;
            return state;
        }
        state = Define.Behavior_NodeState.FAILURE;
        return state;
    }
}
