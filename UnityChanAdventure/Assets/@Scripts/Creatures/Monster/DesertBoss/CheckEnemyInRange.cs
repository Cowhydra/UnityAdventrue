using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyInRange : Behavior_Node
{
    private Transform _transform;
    private Animator _animator;
    private Define.BossDistance _distance;
    public CheckEnemyInRange(Transform transform,Define.BossDistance distance)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _distance = distance;
    }

    public override Define.Behavior_NodeState Evaluate()
    {
      
        Debug.Log("추후 여기에 몬스터 죽었는지 확인해야함");
        //if (_transform.GetComponent<Monster>().isDie)
        //{
        //    state = Define.Behavior_NodeState.FAILURE;
        //    return state;
        //}

        object t = GetData("target");
        if (t == null)
        {
            state = Define.Behavior_NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        //현재는 DeserBoss_BT.longAttackRange 이러한 static 함수를 썻지만
        //추후에는 DataManager에서 Attack Range등을 직접 가져올 수 있음
        if (_distance == Define.BossDistance.Long)
        {
            if (Vector3.Distance(_transform.position, target.position) >= DeserBoss_BT.longAttackRange)
            {
                //_animator.SetBool("Attack", true);
                //_animator.SetBool("Walk", false);
                state = Define.Behavior_NodeState.SUCCESS;
                return state;
            }
        }
        else if (_distance == Define.BossDistance.Short)
        {
            if (Vector3.Distance(_transform.position, target.position) <= DeserBoss_BT.shortattackRange)
            {
                //_animator.SetBool("Attack", true);
                //_animator.SetBool("Walk", false);
                state = Define.Behavior_NodeState.SUCCESS;
                return state;
            }

        }
        else if(_distance == Define.BossDistance.Medium)
        {
            if(Vector3.Distance(_transform.position, target.position)>DeserBoss_BT.shortattackRange 
                && Vector3.Distance(_transform.position, target.position) < DeserBoss_BT.longAttackRange)
            {
                state = Define.Behavior_NodeState.SUCCESS;
                return state;
            }
        }

        state = Define.Behavior_NodeState.FAILURE;
        return state;
    }
}
