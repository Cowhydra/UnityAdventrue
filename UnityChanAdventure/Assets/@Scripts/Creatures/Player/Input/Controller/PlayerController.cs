using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Vector2 _inputdir;
    private CharacterController _characterController;
    private Vector3 _direction;

    [SerializeField] private float speed;


    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;


    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [SerializeField] private float jumpPower;


    [SerializeField] private PlayerInput _playerinput;
 
    private void PlayerInputAction(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                Move(context);
                break;
            case "Jump":
                Jump(context);
                break;
        }
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
     

    }
 
    private void OnEnable()
    {
        if (_playerinput == null)
        {
            Debug.Log("player input is null");
        }
        _playerinput.onActionTriggered -= PlayerInputAction;
        _playerinput.onActionTriggered += PlayerInputAction;
        
    }
    private void OnDisable()
    {
        _playerinput.onActionTriggered -= PlayerInputAction;
    }
    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
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
        if (_inputdir.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _inputdir = context.ReadValue<Vector2>();
        _direction = new Vector3(_inputdir.x, 0.0f, _inputdir.y);
        Debug.Log(_direction);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded()) return;
        Debug.Log("����");
        _velocity += jumpPower;
    }

    private bool IsGrounded() => _characterController.isGrounded;
}