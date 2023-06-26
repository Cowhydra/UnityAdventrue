using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskGotoTarget : Behavior_Node
{
    private Transform _transform;

    CharacterController _characterController;
    Animator _animator;
    public TaskGotoTarget(Transform transform)
    {
        _transform = transform;
        _animator=transform.GetComponent<Animator>();   
    }

    public override Define.Behavior_NodeState Evaluate()
    {

        Transform target = (Transform)GetData("target");

        Vector3 direction = target.position - _transform.position;
        float distance = direction.magnitude;

        if (distance > 0.01f)
        {
            direction.Normalize();

            // 이동 속도와 시간에 따라 이동량 계산
            Vector3 moveAmount = direction * LowAI_BT.speed * Time.deltaTime;

            // 캐릭터 컨트롤러를 사용하여 이동
            _characterController.Move(moveAmount);

            _transform.LookAt(target.position);

            state = Define.Behavior_NodeState.RUNNING;
            _animator.SetBool("Walk", true);
        }
        else
        {
            state = Define.Behavior_NodeState.SUCCESS;
        }

        return state;
    }
    /*
     *  public override Define.Behavior_NodeState Evaluate()
    {
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
            if (Vector3.Distance(_transform.position, wp) < 0.01f)
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
     */
}
