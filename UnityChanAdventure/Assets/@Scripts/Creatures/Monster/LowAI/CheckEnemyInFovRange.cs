using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyInFovRange : Behavior_Node
{
    private readonly int _enemyLayerMask = 1 << (int)Define.LayerMask.Enemy;
    private Animator _animator;
    private Transform _transform;
    public CheckEnemyInFovRange(Transform transform)
    {
        _transform=transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, LowAI_BT.fovRange, _enemyLayerMask);

            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                _animator.SetBool("Walking", true);
                state = Define.Behavior_NodeState.SUCCESS;
                return state;
            }

            state = Define.Behavior_NodeState.FAILURE;
            return state;
        }
        state= Define.Behavior_NodeState.SUCCESS;
        return state;
    }

}

