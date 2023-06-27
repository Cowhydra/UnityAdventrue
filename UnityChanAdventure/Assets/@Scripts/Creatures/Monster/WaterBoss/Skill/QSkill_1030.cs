using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QSkill_1030 : MonoBehaviour
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
    private Direction myDir = Direction.Up;
    private bool EffectOn = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage targetobject))
        {
            if (!EffectOn)
            {
                StartCoroutine(nameof(Attack_co), targetobject);
            }
        }
    }
    private void Update()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        switch (myDir)
        {
            case Direction.Up:
                transform.Translate(5 * Vector3.forward * Time.deltaTime);
                break;
            case Direction.Down:
                transform.Translate(5 * Vector3.back * Time.deltaTime);
                break;
            case Direction.Left:
                transform.Translate(5 * Vector3.left * Time.deltaTime);
                break;
            case Direction.Right:
                transform.Translate(5 * Vector3.right * Time.deltaTime);
                break;
        }
    }

    private void OnEnable()
    {
        int randNum = Random.Range(0, 10);
        if (randNum % 4 == 0)
        {
            myDir = Direction.Up;
        }
        else if (randNum % 4 == 1)
        {
            myDir = Direction.Down;
        }
        else if (randNum % 4 == 2)
        {
            myDir = Direction.Left;
        }
        else if (randNum % 4 == 3)
        {
            myDir = Direction.Right;
        }
        StartCoroutine(nameof(Attack_Time));
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
        yield return new WaitForSeconds(2);
        Managers.Resource.Instantiate($"WSkill_{gameObject.transform.parent.name}", transform);
        yield return new WaitForSeconds(1);
        Managers.Resource.Destroy(gameObject);
    }

}
