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
    //private EnemtManager _enemyManager;
    public TaskAttack(Transform transform)
    {
       // _animator=transform.GetComponent<Animator>();   
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            //_enemyManager=target.GetComponent<EnemyManager>();
            _lastTarget=target;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            bool enemyisDead = false; //enemydead 확인하기
            if (enemyisDead)
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
