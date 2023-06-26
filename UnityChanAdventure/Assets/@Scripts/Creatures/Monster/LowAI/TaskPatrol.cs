using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class TaskPatrol : Behavior_Node
{
    private Transform _transform;
    private List<Vector3> _waypoints;
    private int _currentWaypointIndex = 0;
    private float _waitTime = 2f; 
    private float _waitCounter = 0f;
    private bool _waiting = false;
    private Animator _animator;
    private CharacterController _characterController;
    public TaskPatrol(Transform transform, List<Vector3> waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
        _animator = transform.GetComponent<Animator>();
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        Debug.Log("추후 여기에 몬스터 죽었는지 확인해야함");
        //if (_transform.GetComponent<Monster>().isDie)
        //{
        //    state = Define.Behavior_NodeState.FAILURE;
        //    return state;
        //}
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
                _animator.SetBool("Walk", true);
            }
        }
        else
        {
            Vector3 wp = _waypoints[_currentWaypointIndex];

            if (Vector3.Distance(_transform.position, wp) < 0.3f)
            {
              
                _transform.position = wp;
                _waitCounter = 0f;
                _waiting = true;
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
                _animator.SetBool("Walk", false);
            }
            else
            {
                Vector3 moveDirection = wp - _transform.position;
                moveDirection.Normalize();
                _characterController.Move(moveDirection * LowAI_BT.speed * Time.deltaTime);
                _transform.LookAt(wp);
            }
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }

}
