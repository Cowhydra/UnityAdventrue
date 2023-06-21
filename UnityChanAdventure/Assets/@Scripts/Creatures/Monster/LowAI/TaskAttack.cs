using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskAttack : Behavior_Node
{
    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    public TaskAttack(Transform transform)
    {
       // _animator=transform.GetComponent<Animator>();   
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemy = target.GetComponent<Creature>();
            _lastTarget=target;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
          
            if (_enemy.isDie)
            {
                ClearData("target");
                // _animator.SetBool("Attacking", false);
                //_animator.SetBool("Walking", true);
            }
            else
            {
                _attackCounter = 0f;
            }
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }
}
