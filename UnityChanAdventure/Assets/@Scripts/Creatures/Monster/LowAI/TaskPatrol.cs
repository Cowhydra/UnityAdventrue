using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskPatrol : Behavior_Node
{

    private Transform _transform;
    [SerializeField]
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private float _waitTime = 2f;
    private float _waitCounter = 0f;
    private bool _waiting = false;
    private Vector3 _randomDestination;

    public TaskPatrol(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
    }

    public override Define.Behavior_NodeState Evaluate()
    {
  

        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
                _animator.SetBool("Walk", true);
                GenerateRandomDestination();
            }
        }
        else
        {
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.5f)
            {
                _waitCounter = 0f;
                _waiting = true;
                _animator.SetBool("Walk", false);
            }
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }

    private void GenerateRandomDestination()
    {
        float radius = 15f; // 랜덤 위치 생성을 위한 반경 설정
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        _randomDestination = new Vector3(randomCircle.x, 0f, randomCircle.y) + _transform.position;

        if (NavMesh.SamplePosition(_randomDestination, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            _navMeshAgent.SetDestination(hit.position);
        }
    }

}
