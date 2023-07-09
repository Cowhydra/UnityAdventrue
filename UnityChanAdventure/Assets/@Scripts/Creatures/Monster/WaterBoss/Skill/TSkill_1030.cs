using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TSkill_1030 : MonoBehaviour
{
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        gameObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        gameObject.SetRandomPositionSphere(15, 25);
    }
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
    private Rigidbody rigid;
    private Direction myDir = Direction.Up;

    private void AddForce()
    {
        int randValue = Random.Range(0, 4);
        myDir = (Direction)randValue;
        switch (myDir)
        {
            case Direction.Up:
                rigid.AddForce(10 * Vector3.forward);
                break;
            case Direction.Down:
                rigid.AddForce(10 * Vector3.back);
                break;
            case Direction.Left:
                rigid.AddForce(10 * Vector3.left);
                break;
            case Direction.Right:
                rigid.AddForce(10 * Vector3.right);
                break;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(nameof(AddForceCo));
        StartCoroutine(nameof(LifeCycle_co));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator AddForceCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            AddForce();
        }

    }
    private IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(8.0f);
        Managers.Resource.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<MyCharacter>().OnDamage(15);
        }
    }
}
