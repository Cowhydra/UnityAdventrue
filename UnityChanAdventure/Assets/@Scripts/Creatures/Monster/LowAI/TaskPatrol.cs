using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class TaskPatrol : Behavior_Node
{
    private Transform _transform;
    private Transform[] _waypoints;
    private int _currentWaypointIndex = 0;
    private float _waitTime = 1f; 
    private float _waitCounter = 0f;
    private bool _waiting = false;
    private float _speed = 0f;
    private Animator _animator;

    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
      //  _animator = transform.GetComponent<Animator>();
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
              //  _animator.SetBool("Walking", true);
            }
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
            {
                _transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
             //   _animator.SetBool("Walking", false);
            }
            else
            {
                _transform.position = Vector3.MoveTowards(
                    _transform.position, wp.position,LowAI_BT.speed*Time.deltaTime);
                _transform.LookAt(wp.position);
            }
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }

}
