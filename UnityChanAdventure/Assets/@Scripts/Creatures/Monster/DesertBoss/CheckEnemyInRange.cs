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
      
        Debug.Log("���� ���⿡ ���� �׾����� Ȯ���ؾ���");
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
        //����� DeserBoss_BT.longAttackRange �̷��� static �Լ��� ������
        //���Ŀ��� DataManager���� Attack Range���� ���� ������ �� ����
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
