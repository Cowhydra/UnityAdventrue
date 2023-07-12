using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TaskAttack_Player : Behavior_Node
{


   //공격 타이머 설정
    private float _attackTime = 5f;
    private float _attackCounter = 0f;
    private float _SkillattackCounter = 5f;
    private NavMeshAgent _navMeshAgent;

    //각 요소들 
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    private PlayerController playerController;
    private Transform transform;
    List<SkillButton> skillButtons= new List<SkillButton>();
    public TaskAttack_Player(Transform transform, PlayerController myCharacter)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
        this.playerController = myCharacter;
        skillButtons = GameObject.FindObjectsOfType<SkillButton>().ToList();
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            //적을 transform.root에서 가져오는 이유는 적 콜라이더와 적 Creatrue 컴포넌트를
            //붙여놓은 위치가 달라서 어쩔 수 없음 - > 설계미스
            _enemy = target.transform.root.GetComponentInChildren<Creature>();
            _lastTarget = target;
        }

        _SkillattackCounter += Time.deltaTime;
        _attackCounter+= Time.deltaTime;
        if (target == null)
        {
            return Define.Behavior_NodeState.FAILURE;
        }
        if (_SkillattackCounter >= _attackTime&&!playerController.isAttacking)
        {
            if (_enemy==null||_enemy.isDie)
            {
                ClearData("target");
            }
            else
            {
                //transform.LookAt(target.position);
                int i=Random.Range(0, skillButtons.Count);
                skillButtons[i].ExcuteSKill(skillButtons[i].ButtonSKillType);
                _SkillattackCounter = 0f;

            }
        }
        if(_attackCounter>= _attackTime&&!playerController.isAttacking)
        {

            if (_enemy == null||_enemy.isDie)
            {
                ClearData("target");
            }
            else
            {
                transform.LookAt(target.position);
                Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Attack);
            }

            _attackTime = 0;
        }

        if (playerController.isAttacking)
        {
            _navMeshAgent.isStopped = true;
        }
        else
        {
            _navMeshAgent.isStopped = false;
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }


}
