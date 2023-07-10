using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInFovRange_Player : Behavior_Node
{
    private int _enemyLayerMask = (int)Define.LayerMask.Player;
    private Animator _animator;
    private Transform _transform;
    private Vector3 _movedir;
    public CheckEnemyInFovRange_Player(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _enemyLayerMask = transform.gameObject.layer == (int)Define.LayerMask.Enemy ? (int)Define.LayerMask.Player : (int)Define.LayerMask.Enemy;


    }

    public override Define.Behavior_NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Debug.Log("타겟 못찾고 있음");
            Transform targettransform= Util.GetNbhdMonster(_transform.position, 1<<_enemyLayerMask, Mathf.Infinity);
            if (targettransform != null)
            {
                parent.parent.SetData("target", targettransform);
                state = Define.Behavior_NodeState.SUCCESS;
                return state;
            }

            state = Define.Behavior_NodeState.FAILURE;
            return state;
        }
        state = Define.Behavior_NodeState.SUCCESS;
        return state;


    }
}
