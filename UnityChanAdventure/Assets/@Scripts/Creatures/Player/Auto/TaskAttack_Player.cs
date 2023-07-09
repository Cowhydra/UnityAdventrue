using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskAttack_Player : Behavior_Node
{


    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    private float _SkillattackCounter = 2.5f;
    private Animator _animator;
    private Transform _lastTarget;
    private Creature _enemy;
    private PlayerController playerController;
    private Transform transform;
    private bool isSkillAttacking;
    List<SkillButton> skillButtons= new List<SkillButton>();
    public TaskAttack_Player(Transform transform, PlayerController myCharacter)
    {
        this.transform = transform;
        _animator = transform.GetComponent<Animator>();
        this.playerController = myCharacter;
        skillButtons = GameObject.FindObjectsOfType<SkillButton>().ToList();
    }

    public override Define.Behavior_NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemy = target.transform.root.GetComponentInChildren<Creature>();
            _lastTarget = target;
        }

        _SkillattackCounter += Time.deltaTime;
        _attackCounter+= Time.deltaTime;
        if (_SkillattackCounter >= _attackTime&&!playerController.isAttacking)
        {

            if (_enemy.isDie)
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
        else if(_attackCounter>= _attackTime&&!playerController.isAttacking)
        {

            if (_enemy.isDie)
            {
                ClearData("target");
            }
            else
            {
               // transform.LookAt(target.position);
                Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Attack);
            }


            _attackTime = 0;
        }

        state = Define.Behavior_NodeState.RUNNING;
        return state;
    }


}
