using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskGotoTarget : Behavior_Node
{
    private Transform _transform;

    public TaskGotoTarget(Transform transform)
    {
        _transform = transform;
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position,  target.position,LowAI_BT.speed * Time.deltaTime);
            _transform.LookAt(target.position);
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }
}
