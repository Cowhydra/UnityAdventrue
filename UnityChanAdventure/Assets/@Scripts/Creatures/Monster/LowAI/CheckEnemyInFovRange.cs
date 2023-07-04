using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyInFovRange : Behavior_Node
{
    private  int _enemyLayerMask = (int)Define.LayerMask.Player;
    private Animator _animator;
    private Transform _transform;
    public CheckEnemyInFovRange(Transform transform)
    {
        _transform=transform;
        _animator = transform.GetComponent<Animator>();
        _enemyLayerMask = transform.gameObject.layer ==(int)Define.LayerMask.Enemy ?(int)Define.LayerMask.Player : (int)Define.LayerMask.Enemy;


    }

    public override Define.Behavior_NodeState Evaluate()
    {

        if (_transform.GetComponent<Monster>().isDie)
        {
            state = Define.Behavior_NodeState.FAILURE;
            return state;
        }
        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, LowAI_BT.fovRange, 1<<_enemyLayerMask);

            if (colliders.Length > 0)
            {
                Collider closestCollider = null;
                float closestDistance = Mathf.Infinity;


                //���� AI�� ������ �ȴٸ� ���� ������ �� ���� ����� ������ óġ�� �� �ֵ��� AI ����
                foreach (Collider collider in colliders)
                {
                    float distance = Vector3.Distance(_transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestCollider = collider;
                        closestDistance = distance;
                    }
                }
                if (closestCollider != null && (_transform.position -closestCollider.transform.position).sqrMagnitude<25f)
                {
                    parent.parent.SetData("target", closestCollider.transform);
                      _animator.SetBool("Walk", true);
                    state = Define.Behavior_NodeState.SUCCESS;
                    return state;
                }

            }

            state = Define.Behavior_NodeState.FAILURE;
            return state;
        }
        else
        {
            Transform target = (Transform)GetData("target");
            //�Ÿ��� �缭 �Ÿ����� �ְԵǸ� �ٽ� ���� ���·� ���ư��� �����!
            //������ �� ��ġ�� �������� �ϰ� ������ �������� �ʴ� ���̶����,
            ///�ڽ��� �¾ ������ ��ȸ�ϴ� �� ���� ���� ������ �� �ִ�.
            if ((_transform.position - target.position).sqrMagnitude >= 25f)
            {
                state = Define.Behavior_NodeState.FAILURE;
                _animator.SetBool("Walk", true);
                _animator.SetBool("Attack", false);
                return state;
            }
            state = Define.Behavior_NodeState.SUCCESS;
            return state;
        }



    }

}

