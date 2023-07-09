using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;


public class CheckEnemyInAttackRange : Behavior_Node
{
    private Transform _transform;
    private Animator _animator;

    public CheckEnemyInAttackRange(Transform transform)
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
        if (Vector3.Distance(_transform.position, target.position) <= LowAI_BT.attackRange+2)
        {
         
            _animator.SetBool("Attack", true);
             _animator.SetBool("Walk", false);

            state = Define.Behavior_NodeState.SUCCESS;
            return state;
        }
        //_animator.SetBool("Walk", true);
         _animator.SetBool("Attack", false);
        state = Define.Behavior_NodeState.FAILURE;
        return state;
    }
}
