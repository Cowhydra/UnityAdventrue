using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class HighHpAttack : Behavior_Node
{
    //������ �ϵ� �ڵ� �Ǿ� ������ .. ���� ���Ŀ�  �� �� ü�������� �Ѵٸ� DataManager���� �����;��� �����ϰ�
    private float _attackTime = 3f;
    private float _attackCounter = 0f;
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    private Transform transform;

    public HighHpAttack(Transform transform)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override Define.Behavior_NodeState Evaluate()
    {


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
              
                GameObject QSkill = Managers.Resource.Instantiate($"QSkill_{transform.gameObject.name}", transform);
            }
            _attackCounter = 0f;
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }
}