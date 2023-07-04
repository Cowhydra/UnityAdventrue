using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.UIElements;

public class TaskAttack : Behavior_Node
{
    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    private GameObject Projectile;
    private Transform transform;
    public TaskAttack(Transform transform,GameObject Projectile=null)
    {
        this.transform = transform;
        _animator =transform.GetComponent<Animator>();   
        if(Projectile!=null)
        {
           this.Projectile= Projectile;
        }
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemy = target.GetComponent<Creature>();
            _lastTarget=target;
            //방법 1. 그냥 LowAI 에 Static 으로 내 코드 작성 후 DataManageㄱ에서 불러오는건데 모발겜이라..
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
                if (Projectile != null)
                {
                    GameObject Pro = Managers.Resource.Instantiate($"{transform.gameObject.name}_Projectile");
                    //   GameObject Pro= Managers.Resource.PoolInstantiate(Projectile);
                    Pro.GetOrAddComponent<MonProjectileController>().SetProjectile(transform.position, 10);
                }
                else
                {

                    _enemy.GetComponent<IDamage>().OnDamage(LowAI_BT.attackdamage);
                }

                _attackCounter = 0f;
            }
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }
}
