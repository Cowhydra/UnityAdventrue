using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    private CharacterController _characterController;
    private Vector3 _direction;

    private MyCharacter _mycharacter;
    [SerializeField] private Animator _battleanim_TowHand;
    [SerializeField] private Animator _town_anim;
    [SerializeField] private Animator _battleanim_Magic;
    [SerializeField] private PlayerAttackArea attackarea;

    [SerializeField] private float speed;
    private bool _isSprint;

    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float _currentVelocity;


    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [SerializeField] private float jumpPower;
    private bool isAttacking;

    private Animator _animator;
 
  
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator=GetComponent<Animator>();
        attackarea=GetComponentInChildren<PlayerAttackArea>();
        _mycharacter=GetComponent<MyCharacter>(); 
    }
   
    private void Start()
    {
        #region Event
        
        Managers.Event.MoveInputAction -= SetMoveDir;
        Managers.Event.MoveInputAction += SetMoveDir;
        Managers.Event.KeyInputAction -= KeyInputExcute;
        Managers.Event.KeyInputAction += KeyInputExcute;
        #endregion
        DontDestroyOnLoad(gameObject.transform.parent);

    }
  
    private void OnDestroy()
    {
        Managers.Event.KeyInputAction -= KeyInputExcute;
        Managers.Event.MoveInputAction -= SetMoveDir;
    }

    private void Update()
    {
        ApplyGravity();
      
        ApplyMovement();
        ApplyRotation();
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
        if (Camera.main.transform == null) return;
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
        if (Camera.main.transform == null) return;
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
            yield return new WaitForSeconds(speed);
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
                break;
        }
    }
    private void OnBaseAttack()
    {
        if (!IsGrounded()) return;
        _animator.SetTrigger("OnBaseAttack");
        isAttacking = true;
    }
    public void OnAttackAnimationEvent(int damage)
    {
        attackarea.gameObject.SetActive(true);
        attackarea.SetDamage(damage*_mycharacter.Level+_mycharacter.Attack);
        isAttacking = false;

    }

    private bool IsGrounded() => _characterController.isGrounded;
}
