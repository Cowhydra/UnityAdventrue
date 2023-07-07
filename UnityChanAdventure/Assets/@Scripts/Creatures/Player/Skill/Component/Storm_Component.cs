using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm_Component : MonoBehaviour
{
    private Transform _Owner;
    public Transform Owner
    {
        get { return _Owner; }
        set
        {
            _Owner = value;
            StartStorm();
        }
    }

    private Vector3 movedir;
    [SerializeField]
    private float movespeed = 5.0f;


    public Vector3 MoveDir
    {
        get { return movedir; }
        set
        {
            movedir = value;
        }
    }

    private void StartStorm()
    {
        gameObject.transform.position = Owner.transform.position;
        StartCoroutine(nameof(LifeCycle_co));
    }

    void Update()
    {
        transform.position += movedir * movespeed * Time.deltaTime;
    }

    IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(5.0f);
        Managers.Resource.Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Owner.gameObject.layer == (int)Define.LayerMask.Player)
        {
            if (other.gameObject.layer == (int)Define.LayerMask.Enemy)
            {
                if (other.TryGetComponent(out IDamage idamage))
                {
                    idamage.OnDamage(150);
                }

            }
        }
        else if (Owner.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            if (other.gameObject.layer == (int)Define.LayerMask.Player)
            {
                if (other.TryGetComponent(out IDamage idamage2))
                {
                    idamage2.OnDamage(150);
                }

            }
        }
    }
}
