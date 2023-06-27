using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class MiddleAttack : Behavior_Node
{
    //지금은 하드 코딩 되어 있지만 .. 만약 추후에  좀 더 체계적으로 한다면 DataManager에서 가져와야함 동일하게
    private float _attackTime = 3f;
    private float _attackCounter = 0f;
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    private Transform transform;
    private NavMeshAgent navMesh;
    private Vector3 _randomDestination;
    public MiddleAttack(Transform transform)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
        navMesh = transform.GetComponent<NavMeshAgent>();
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
                //원형 반원형  유도 

                int count = 30;
                float intervalAngle = 360 / count;

               int randomNum=Random.Range(0, 100);
                if (randomNum % 3 == 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        float angle = intervalAngle * i;
                        float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
                        float y = Mathf.Sin(angle * Mathf.PI / 180.0f);

                        if (x > 0 && y > 0)
                        {
                            GameObject RSkill = Managers.Resource.Instantiate($"RSkill_{transform.gameObject.name}", transform);
                            RSkill.GetComponent<RSkill_1010>() .SetDir(new Vector3(x, 0, y), 10);
                        }
                    }
                }
                else if(randomNum % 3 == 1)
                {
                    for (int i = 0; i < count; i++)
                    {
                        float angle = intervalAngle * i;
                        float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
                        float y = Mathf.Sin(angle * Mathf.PI / 180.0f);

                        if (x > 0 )
                        {
                            GameObject RSkill = Managers.Resource.Instantiate($"RSkill_{transform.gameObject.name}", transform);
                            RSkill.GetComponent<RSkill_1010>().SetDir(new Vector3(x, 0, y), 5);
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        float angle = intervalAngle * i;
                        float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
                        float y = Mathf.Sin(angle * Mathf.PI / 180.0f);

                        GameObject RSkill = Managers.Resource.Instantiate($"RSkill_{transform.gameObject.name}", transform);
                        RSkill.GetComponent<RSkill_1010>().SetDir(new Vector3(x, 0, y), 5);
                      

                    }
                }

                GenerateRandomDestination();
            }
            _attackCounter = 0f;
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }

    private void GenerateRandomDestination()
    {
        float radius = 6f; // 랜덤 위치 생성을 위한 반경 설정
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        _randomDestination = new Vector3(randomCircle.x, 0f, randomCircle.y) + transform.position;

        if (NavMesh.SamplePosition(_randomDestination, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            navMesh.SetDestination(hit.position);
        }
    }
}
