using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class LongAttack : Behavior_Node
{
    //지금은 하드 코딩 되어 있지만 .. 만약 추후에  좀 더 체계적으로 한다면 DataManager에서 가져와야함 동일하게
    private float _attackTime = 7f;
    private float _attackCounter = 0f;
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    private Transform transform;
    public LongAttack(Transform transform)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override Define.Behavior_NodeState Evaluate()
    {

        if (DeserBoss_BT.isAttacking)
        {
            state = Define.Behavior_NodeState.RUNNING;
            return state;
        }

        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemy = target.GetComponent<Creature>();
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            if (_enemy.isDie)
            {
                ClearData("target");
                _animator.SetBool("Attack", false);
                _animator.SetBool("Walk", true);
            }
            else
            {
       
                transform.LookAt(target.position);

                transform.position = target.position + Vector3.up * 3;
                transform.gameObject.GetOrAddComponent<QSkill_1010>().enabled = true;
                DeserBoss_BT.isAttacking = true;
            }
            _attackCounter = 0f;
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }
}
