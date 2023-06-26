using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class FindTarget : Behavior_Node
{
    private int _enemyLayerMask = (int)Define.LayerMask.Player;
    private Animator _animator;
    private Transform _transform;
    public FindTarget(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _enemyLayerMask = transform.gameObject.layer == (int)Define.LayerMask.Enemy ? (int)Define.LayerMask.Player : (int)Define.LayerMask.Enemy;
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
            GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Player");

            if (Enemys.Length > 0)
            {
                GameObject closestEnemy = null;
                float closestDistance = Mathf.Infinity;


                //내가 AI를 돌리게 된다면 적을 추적할 때 가장 가까운 적부터 처치할 수 있도록 AI 수정
                foreach (GameObject Enemy in Enemys)
                {
                    float distance = Vector3.Distance(_transform.position, Enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestEnemy = Enemy;
                        closestDistance = distance;
                    }
                }
                if (closestEnemy != null && (_transform.position - closestEnemy.transform.position).sqrMagnitude < 25f)
                {
                    parent.parent.SetData("target", closestEnemy.transform);
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
            if ((_transform.position - target.position).sqrMagnitude >= 256f)
            {
                Debug.Log("타겟 재설정");
                state = Define.Behavior_NodeState.FAILURE;
                ClearData("target");
                //_animator.SetBool("Walk", true);
                //_animator.SetBool("Attack", false);
                return state;
            }
            state = Define.Behavior_NodeState.SUCCESS;
            return state;
        }



    }
}
