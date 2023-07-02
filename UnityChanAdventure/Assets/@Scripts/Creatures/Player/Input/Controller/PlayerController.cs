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

    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float _currentVelocity;


    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [SerializeField] private float jumpPower;


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
        Debug.Log("조이스틱 정상작동 확인");
        Managers.Event.MoveInputAction -= SetMoveDir;
       // Managers.Event.MoveInputAction += SetMoveDir;
        Managers.Event.KeyInputAction -= KeyInputExcute;
        Managers.Event.KeyInputAction += KeyInputExcute;
        #endregion
    }
    private void OnDestroy()
    {
        Managers.Event.KeyInputAction -= KeyInputExcute;
        Managers.Event.MoveInputAction -= SetMoveDir;
    }

    private void Update()
    {
        ApplyGravity();
       // ApplyRotation();
        ApplyMovement();
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

    private void ApplyRotation()
    {
        if (_direction.sqrMagnitude < 0.1f) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        #region PC 테스트용 임시코드
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");
        float offset = _isSprint == true ? 1f : 0.5f;
        _animator.SetFloat("PosX", _direction.x * offset);
        _animator.SetFloat("PosZ", _direction.z*offset);
        #endregion
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    private void SetMoveDir(Vector2 movedir)
    {
        _direction = new Vector3(movedir.x, 0.0f, movedir.y);
        float offset = _isSprint == true ? 1f : 0.5f;
        _animator.SetFloat("PosX", _direction.x*offset);
        _animator.SetFloat("PosZ", _direction.z*offset);
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
        _direction = Vector2.zero;
       
    }
    public void OnAttackAnimationEvent(int damage)
    {
        attackarea.gameObject.SetActive(true);
        attackarea.SetDamage(damage*_mycharacter.Level+_mycharacter.Attack);
    }
    private bool IsGrounded() => _characterController.isGrounded;
}
