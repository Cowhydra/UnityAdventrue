using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
public class ShortAttack : Behavior_Node
{
    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    private Transform transform;
    private NavMeshAgent navMesh;

    private int maxcount = 5;
    private int currentcount = 0;
    public ShortAttack(Transform transform)
    {
       this.transform = transform;
        _animator = transform.GetComponent<Animator>();
        navMesh=transform.GetComponent<NavMeshAgent>();
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
                Debug.Log($"target Pos: {target.position}");
                navMesh.SetDestination(target.position+0.5f*target.forward);
                navMesh.speed += 15;
                Managers.Resource.Instantiate($"ESkill_{transform.name}",transform);
                Debug.Log("ShortAttack");
                currentcount++;
                //적 근처에 점프

            }
            navMesh.speed -= 15;
            _attackCounter = 0f;
            if (currentcount == maxcount)
            {
                Managers.Resource.Instantiate($"WSkill_{transform.name}", transform);
                currentcount = 0;
                state = Define.Behavior_NodeState.FAILURE;
                Debug.Log("랜덤이동 기믹 넣어주자");
                return state;
            }
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }


}
