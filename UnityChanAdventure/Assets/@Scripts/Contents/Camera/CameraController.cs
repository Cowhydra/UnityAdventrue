using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera camera;
    public GameObject player;

    private CinemachineFramingTransposer framingTransposer;

    private void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 cameraPosition = transform.position;
        Vector3 playerPosition = player.transform.position;
        Vector3 direction = (playerPosition - cameraPosition).normalized;

        if (Physics.Raycast(cameraPosition, direction, out hit, Mathf.Infinity, 1<<(int)Define.LayerMask.Enviroment))
        {
            float distanceToWall = hit.distance;
            float targetDistance = Mathf.Clamp(distanceToWall - 0.5f, 3, 10);

            // ī�޶� �Ÿ� ����
            framingTransposer.m_CameraDistance = Mathf.Lerp(framingTransposer.m_CameraDistance, targetDistance, Time.deltaTime);
        }
        else
        {
            // ���� ī�޶� �Ÿ��� ����
            float originalDistance = 10f; // ���ϴ� �ʱ� ī�޶� �Ÿ� ����
            framingTransposer.m_CameraDistance = Mathf.Lerp(framingTransposer.m_CameraDistance, originalDistance, Time.deltaTime);
        }
    }
}
