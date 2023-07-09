using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class LongAttack : Behavior_Node
{
    //지금은 하드 코딩 되어 있지만 .. 만약 추후에  좀 더 체계적으로 한다면 DataManager에서 가져와야함 동일하게
    private float _attackTime = 3f;
    private float _attackCounter = 0f;
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    private Transform transform;
    private NavMeshAgent navMesh;

    public LongAttack(Transform transform)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
        navMesh = transform.GetComponent<NavMeshAgent>();
    }

    public override Define.Behavior_NodeState Evaluate()
    {


        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemy = target.GetComponent<Creature>();
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            if (_enemy.isDie)
            {
                ClearData("target");
                _animator.SetBool("Attack", false);
                _animator.SetBool("Walk", true);
            }
            else
            {
       
                transform.LookAt(target.position);
                navMesh.SetDestination(target.position-0.5f*target.forward);
                navMesh.speed += 20;
                GameObject QSkill= Managers.Resource.Instantiate($"QSkill_{transform.gameObject.name}",transform);
            }
            _attackCounter = 0f;
            navMesh.speed -= 20;
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }
}
