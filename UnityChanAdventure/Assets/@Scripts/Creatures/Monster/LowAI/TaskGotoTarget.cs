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
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            state = Define.Behavior_NodeState.RUNNING;
            _animator.SetBool("Walk", true);
        }
        else
        {
            state = Define.Behavior_NodeState.SUCCESS;
        }

        return state;
    }

}
