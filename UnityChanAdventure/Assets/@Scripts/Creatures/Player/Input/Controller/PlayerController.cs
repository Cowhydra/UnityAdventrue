using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    //조이스틱에서 인풋을 받으면, 플레이어 컨트롤러(캐릭터 움직임 제어 등 )에서 해당 움직임들을 처리하게 설계 ( 이벤트 받기  )
    private CharacterController _characterController;
    private Vector3 _direction;

    private MyCharacter _mycharacter;
    [SerializeField] private PlayerAttackArea attackarea;
    private NavMeshAgent _navMeshAgent;

    [SerializeField] private float speed;
    private bool _isSprint;

    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float _currentVelocity;


    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [SerializeField] private float jumpPower;
    public bool isAttacking;
    private bool IsAuto;
    private Animator _animator;

    private int _currentSkill;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator=GetComponent<Animator>();
        attackarea=GetComponentInChildren<PlayerAttackArea>();
        _mycharacter=GetComponent<MyCharacter>();
        _navMeshAgent=GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = false;
    }
   
    private void Start()
    {
        #region Event
        
        Managers.Event.MoveInputAction -= SetMoveDir;
        Managers.Event.MoveInputAction += SetMoveDir;
        Managers.Event.KeyInputAction -= KeyInputExcute;
        Managers.Event.KeyInputAction += KeyInputExcute;
        Managers.Event.SkillAction -= OnSkill;
        Managers.Event.SkillAction += OnSkill;
        #endregion
        DontDestroyOnLoad(gameObject.transform.parent);

    }
  
    private void OnDestroy()
    {
        Managers.Event.KeyInputAction -= KeyInputExcute;
        Managers.Event.MoveInputAction -= SetMoveDir;
        Managers.Event.SkillAction -= OnSkill;
    }

    #region 기본동작 ( 이동, 중력, 회전)
    private void Update()
    {
        if (_characterController.enabled)
        {
            ApplyGravity();

            ApplyMovement();
            ApplyRotation();
        }

    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    float rotateSpeed = 5.0f;
    private void ApplyRotation()
    {
        if (Camera.main == null) return;
        Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveInput = new Vector3(horizontal, 0f, vertical);
        //Vector3 moveInput = new Vector3(_direction.x, 0f, _direction.z);
        Vector3 rotateVec = forwardVec * moveInput.z + rightVec * moveInput.x;

        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotateVec);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    private void ApplyMovement()
    {
        if (isAttacking) return;
        #region PC 테스트용 임시코드
        if (Camera.main == null) return;
        Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveInput = new Vector3(horizontal, 0f, vertical);
        //Vector3 moveInput = new Vector3(_direction.x, 0f, _direction.z);

        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

        Vector3 moveVec = forwardVec * moveInput.z + rightVec * moveInput.x+_direction.y*Vector3.up;

        float offset = _isSprint ? 1f : 0.5f;
        _animator.SetFloat("PosX", horizontal * offset);
        _animator.SetFloat("PosZ", vertical * offset);

        _characterController.Move(moveVec * speed * Time.deltaTime);
        #endregion
    }
    #endregion

    private void SetMoveDir(Vector2 movedir)
    {
        _direction = new Vector3(movedir.x, 0.0f, movedir.y);
        float offset = _isSprint == true ? 1f : 0.5f;
        _animator.SetFloat("PosX", -_direction.x*offset);
        _animator.SetFloat("PosZ", -_direction.z*offset);
    }
    private void Jump()
    {
        if (!IsGrounded()) return;
        Debug.Log("점프");
        _velocity += jumpPower;
    }


    private IEnumerator Sprint_co()
    {
        if (!_isSprint)
        {
            _isSprint = true;
            speed += 3;
            yield return new WaitForSeconds(5);
            speed -= 3;
            _isSprint = false;
        }
      
    }

    private void KeyInputExcute(Define.KeyInput KeyInput)
    {
        switch (KeyInput)
        {
            case Define.KeyInput.Jump:
                Jump();
                break;
            case Define.KeyInput.Sprint:
                StartCoroutine(nameof(Sprint_co));
                break;
            case Define.KeyInput.Attack:
                OnBaseAttack();
                break;
            case Define.KeyInput.Auto:
                Auo_Fight();
                break;
        }
    }
    private void Auo_Fight()
    {
        if (IsAuto)
        {
            AutoOff();
        }
        else
        {
            AutoOn();
        }
    }

    private void AutoOn()
    {
        IsAuto = true;
        GetComponent<CharacterController>().enabled = false;
        _navMeshAgent.enabled = true;
        gameObject.GetOrAddComponent<AutoFight_BT>().enabled = true;
    }
    public void AutoOff()
    {
        IsAuto = false;
        GetComponent<CharacterController>().enabled = true;
        _navMeshAgent.enabled = false;
        gameObject.GetOrAddComponent<AutoFight_BT>().enabled = false;
    }


    #region About Animation Event
    //공격 애니메이션 
    private void OnBaseAttack()
    {
        if (!IsGrounded()) return;
        _animator.SetTrigger("OnBaseAttack");
        isAttacking = true;

    }
    public void OnAttackAnimationEvent(int damage)
    {
        if (attackarea == null)
        {
            attackarea = GetComponentInChildren<PlayerAttackArea>();
        }
        attackarea.gameObject.SetActive(true);
        attackarea.SetDamage(damage*_mycharacter.Level+_mycharacter.Attack);
        switch (damage)
        {
            case 10:
                Managers.Sound.Play("Attack1");
                break;
            case 20:
                Managers.Sound.Play("Attack2");
                break;
            case 30:
                Managers.Sound.Play("Attack3");
                break;
            case 40:
                Managers.Sound.Play("Attack4");
                break;
        }
        isAttacking = false;

    }

    //onSkill 로 스킬 코드에 대한 이벤트를 받으면 -> 애니메이션 SetBool을 이용해
    //애니메이션 으로 이동해 애니메이션 이벤트를 발동하도록 설정
    private void OnSkill(int skillcode)
    {
        if (isAttacking) return;
        if (skillcode == 0) return;
        isAttacking = true;
        _currentSkill = skillcode;
        _animator.SetTrigger("Skill");
        GameObject SkillReadyEffect = Managers.Resource.Instantiate("PlayerSkillUse");
        SkillReadyEffect.transform.position = gameObject.transform.position;
        _animator.SetBool($"{Managers.Data.SkillDataDict[skillcode].animname}", true);
    }

    private void OnSkillAnimationEvent()
    {
        if (_currentSkill == 0) return;
        Managers.SKill.ExcuteSKill(_currentSkill, gameObject.transform);
        isAttacking = false;
        _animator.SetBool($"{Managers.Data.SkillDataDict[_currentSkill].animname}", false);
    }

    #endregion




    private bool IsGrounded() => _characterController.isGrounded;
}
