using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class HighHpAttack : Behavior_Node
{
    //지금은 하드 코딩 되어 있지만 .. 만약 추후에  좀 더 체계적으로 한다면 DataManager에서 가져와야함 동일하게
    private float _attackTime = 6f;
    private float _attackCounter = 0f;
    private float _attackCounter2 = 0f;
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
        _attackCounter2 += Time.deltaTime;
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
                for(int i = 0; i < 4; i++)
                {
                    GameObject QSkill = Managers.Resource.Instantiate($"QSkill_{transform.gameObject.name}");
                    QSkill.GetOrAddComponent<QSkill_1030>();
                }
            }
            _attackCounter = 0f;
        }
        if (_attackCounter2 >= _attackTime*1.85f)
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
                for (int i = 0; i < 6; i++)
                {
                    GameObject WSKill = Managers.Resource.Instantiate($"WSkill_{transform.gameObject.name}");
                    WSKill.GetOrAddComponent<WSkill_1030>();
                }

            }
            _attackCounter2 = 0f;
        }
        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }
}
