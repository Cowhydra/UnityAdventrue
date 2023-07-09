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
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, AutoFight_BT.fovRange , 1 << _enemyLayerMask);

            if (colliders.Length > 0)
            {
                Collider closestCollider = null;
                float closestDistance = Mathf.Infinity;


                //���� ����� �� -> �� (���� ���忡���� ����� ���� ���� )
                foreach (Collider collider in colliders)
                {
                    float distance = Vector3.Distance(_transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestCollider = collider;
                        closestDistance = distance;
                    }
                }
                if (closestCollider != null && (_transform.position - closestCollider.transform.position).sqrMagnitude < 25f)
                {
                    parent.parent.SetData("target", closestCollider.transform);
                    _movedir = closestCollider.transform.position - _transform.position;
                    _animator.SetFloat("PosX", -_movedir.x);
                    _animator.SetFloat("PosZ", -_movedir.z);
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
                _animator.SetFloat("PosX",0);
                _animator.SetFloat("PosZ", 0);
                return state;
            }
            state = Define.Behavior_NodeState.SUCCESS;
            return state;
        }



    }
}
