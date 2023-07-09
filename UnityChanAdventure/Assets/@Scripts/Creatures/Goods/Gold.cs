using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    GameObject player;
    private float _moveSpeed = 5.0f;
    private int _value;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Managers.Game.GoldChange(_value);
            Managers.Resource.Destroy(gameObject);
        }
    }
    public void SetValue(int _level, Transform transform)
    {
        _value = _level * 20 + Random.Range(50, 100);
        gameObject.transform.position = transform.position;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        Vector3 movement = direction.normalized * _moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
}
