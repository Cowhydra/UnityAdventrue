using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class QSkill_1030 : MonoBehaviour
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
    private Rigidbody rigid;
    private Direction myDir = Direction.Up;
    private bool EffectOn = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!EffectOn)
            {
                StartCoroutine(nameof(Attack_co), other.TryGetComponent(out IDamage damage));
            }
        }
    }
    private void Awake()
    {
        rigid=GetComponent<Rigidbody>();
    }
    private void AddForce()
    {
        int randValue = Random.Range(0, 4);
        myDir = (Direction)randValue;
        switch (myDir)
        {
            case Direction.Up:
                rigid.AddForce(30 * Vector3.forward);
                break;
            case Direction.Down:
                rigid.AddForce(30 * Vector3.back);
                break;
            case Direction.Left:
                rigid.AddForce(30 * Vector3.left);
                break;
            case Direction.Right:
                rigid.AddForce(30 * Vector3.right);
                break;
        }
    }

    private void OnEnable()
    {
        gameObject.transform.position = GameObject.FindAnyObjectByType<Monster>().transform.position+Vector3.up*6;
        StartCoroutine(nameof(Attack_Time));
        StartCoroutine(nameof(AddForceCo));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator Attack_co(IDamage _object)
    {
        EffectOn = true;
        _object.OnDamage(10);
        yield return new WaitForSeconds(1.2f);
        EffectOn = false;
    }
    IEnumerator Attack_Time()
    {
        yield return new WaitForSeconds(5);
        Managers.Resource.Destroy(gameObject);
    }
    private IEnumerator AddForceCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            AddForce();
        }

    }

}
