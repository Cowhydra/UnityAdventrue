using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Component : MonoBehaviour
{
    private Transform _Owner;
    public Transform Owner
    {
        get { return _Owner; }
        set
        {
            _Owner = value;
            StartFireBall();
        }
    }

    [SerializeField] private float movespeed = 5.0f;
    PlayerController playerController;
    GameObject target;
    Vector3 movedir;

    void Update()
    {
       gameObject.transform.RotateAround(Owner.transform.position+Vector3.up,Vector3.up,movespeed*Time.fixedDeltaTime);
    }

    private void StartFireBall()
    {
        float angle = 10;
        float radius = 350;   
        movedir = Vector3.zero;
        movespeed = Random.Range(10, 25);
        angle *= Random.Range(1, 37);

        transform.position = Owner.transform.position + new Vector3(radius * Mathf.Cos(angle) * Mathf.PI / 180.0f, 0, radius * Mathf.Sin(angle) * Mathf.PI / 180.0f);
        StartCoroutine(nameof(LifeCycle_co));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Owner.gameObject.layer == (int)Define.LayerMask.Player)
        {
            if (other.gameObject.layer == (int)Define.LayerMask.Enemy)
            {
                if (other.transform.root.TryGetComponent(out IDamage idamage))
                {
                    idamage.OnDamage(100);
                }

            }
        }
        else if (Owner.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            if (other.gameObject.layer == (int)Define.LayerMask.Player)
            {
                if (other.transform.root.TryGetComponent(out IDamage idamage2))
                {
                    idamage2.OnDamage(100);
                }

            }
        }
    }

    private IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(10);
        Managers.Resource.Destroy(gameObject);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
