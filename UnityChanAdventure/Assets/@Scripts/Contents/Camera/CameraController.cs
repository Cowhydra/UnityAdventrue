using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera camera;
    GameObject player;
    private Vector3 _direction;

    private void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindGameObjectWithTag("Player");
        _direction = (gameObject.transform.position - player.transform.position).normalized;

    }
    private void Update()
    {
        //RaycastHit raycastHit;
        //if(Physics.Raycast(gameObject.transform.position,_direction*10,1<< (int)Define.LayerMask.Enviroment,out raycastHit))
        //{

        //}
    }
}
