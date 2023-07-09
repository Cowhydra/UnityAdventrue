using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskPatorl_Player : Behavior_Node
{
    private Transform _transform;
    [SerializeField]
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private float _waitTime = 2f;
    private float _waitCounter = 0f;
    private bool _waiting = false;
    private Vector3 _randomDestination;
    private Vector3 _movedir;

    public TaskPatorl_Player(Transform transform)
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
                GenerateRandomDestination();

                _animator.SetFloat("PosX", -_movedir.x);
                _animator.SetFloat("PosZ", -_movedir.z);
            }
        }
        else
        {
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.5f)
            {
                _waitCounter = 0f;
                _waiting = true;

                _animator.SetFloat("PosX", 0);
                _animator.SetFloat("PosZ", 0);
            }
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }

    private void GenerateRandomDestination()
    {
        float radius = 15f; // ���� ��ġ ������ ���� �ݰ� ����
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        _randomDestination = new Vector3(randomCircle.x, 0f, randomCircle.y) + _transform.position;

        if (NavMesh.SamplePosition(_randomDestination, out NavMeshHit hit, 8f, NavMesh.AllAreas))
        {
            _navMeshAgent.SetDestination(hit.position);
            _movedir = hit.position - _transform.position;
        }
    }
}
