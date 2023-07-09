using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskGotoTarget : Behavior_Node
{
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Vector3 _movedir;
    public TaskGotoTarget(Transform transform)
    {
        _transform = transform;
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
        _animator = transform.GetComponent<Animator>();
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        _navMeshAgent.SetDestination(target.position);
        _movedir = target.position - _transform.position;
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            state = Define.Behavior_NodeState.RUNNING;

            if (_transform.gameObject.layer == (int)Define.LayerMask.Enemy)
            {
                _animator.SetBool("Walk", true);
            }
            else
            {
                _animator.SetFloat("PosX", -_movedir.x);
                _animator.SetFloat("PosZ", -_movedir.z);
            }
          
        }
        else
        {
            state = Define.Behavior_NodeState.SUCCESS;
        }

        return state;
    }

}
