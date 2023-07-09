using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGravity : MonoBehaviour
{

    public float Gravity = -9.8f;
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers = 1 << (int)Define.LayerMask.Enviroment;
    CharacterController _controller;
    private float _verticalVelocity;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _controller.Move( new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        GroundedCheck();
        JumpAndGravity();
       
    }
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character

    }
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            _verticalVelocity = -2f; // 지면에 닿아있을 때의 속도
        }
        else
        {
            _verticalVelocity += Gravity * Time.deltaTime; // 중력에 따라 속도 감소
        }

        Vector3 moveDirection = new Vector3(0.0f, _verticalVelocity, 0.0f);
        _controller.Move(moveDirection * Time.deltaTime);
    }
}
