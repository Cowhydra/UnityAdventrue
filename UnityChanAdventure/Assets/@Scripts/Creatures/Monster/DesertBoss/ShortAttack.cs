using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class ShortAttack : Behavior_Node
{
    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    private GameObject Projectile;
    private Transform transform;
    public ShortAttack(Transform transform)
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
                transform.position = target.position -target.forward;

                DeserBoss_BT.isAttacking = true;
                //적 근처에 점프

            }
            _attackCounter = 0f;
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }


}
