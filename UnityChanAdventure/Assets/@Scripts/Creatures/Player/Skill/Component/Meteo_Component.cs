using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo_Component : MonoBehaviour
{
    private Transform _Owner;
    public Transform Owner
    {
        get { return _Owner; }
        set
        {
            _Owner = value;
            StartMeteo();
        }
    }
    [SerializeField]
    private float movespeed = 5.0f;
    GameObject Target;
    private int EnemyLayer;
    private IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(10.0f);
        Managers.Resource.Destroy(gameObject);
    }
    // Start is called before the first frame update
    void StartMeteo()
    {
        if (Owner.gameObject.layer == (int)Define.LayerMask.Player)
        {
            EnemyLayer = (int)Define.LayerMask.Enemy;
        }
        else if (Owner.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            EnemyLayer = (int)Define.LayerMask.Player;
        }
        Collider[] colliders = Physics.OverlapSphere(
              transform.position, 50, 1 << EnemyLayer);
        if (colliders.Length > 0)
        {
            Collider closestCollider = null;
            float closestDistance = Mathf.Infinity;


            //가장 가까운 적 -> 적 (몬스터 입장에서는 비용이 들지 않음 )
            foreach (Collider collider in colliders)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestCollider = collider;
                    closestDistance = distance;
                }
            }

            if (closestCollider == null)
            {
                Target = closestCollider.gameObject;
            }
        }
        else
        {
            Target = GameObject.FindObjectOfType<PlayerController>().gameObject;
        }

        if(Target!= null)
        {
            gameObject.transform.position = Target.transform.position + Vector3.up * 18;
        }
        else
        {
            gameObject.transform.position = Owner.transform.position + Vector3.up * 18;
        }
     

        StartCoroutine(nameof(LifeCycle_co));
    }
    private void Update()
    {
        transform.position = transform.position - Vector3.up * movespeed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (Owner.gameObject.layer == (int)Define.LayerMask.Player)
        {
            if (other.gameObject.layer == (int)Define.LayerMask.Enemy)
            {
                if (other.transform.root.TryGetComponent(out IDamage idamage))
                {
                    idamage.OnDamage(300);
                }

            }
        }
        else if (Owner.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            if (other.gameObject.layer == (int)Define.LayerMask.Player)
            {
                if (other.transform.root.TryGetComponent(out IDamage idamage2))
                {
                    idamage2.OnDamage(300);
                }

            }
        }
    }

}
