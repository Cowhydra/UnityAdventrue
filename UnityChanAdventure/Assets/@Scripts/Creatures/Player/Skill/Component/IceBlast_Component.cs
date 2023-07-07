using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlast_Component : MonoBehaviour
{
    private Transform _Owner;
    public Transform Owner
    {
        get { return _Owner; }
        set
        {
            _Owner = value;
            IceAttack();
           
        }
    }
    [SerializeField]
    private float lifeCycle = 5.0f;
    // Start is called before the first frame update
    private void IceAttack()
    {
        if (Owner.gameObject.layer == (int)Define.LayerMask.Player)
        {
            gameObject.transform.position = GameObject.FindAnyObjectByType<PlayerController>().transform.position;
            gameObject.transform.rotation = GameObject.FindAnyObjectByType<PlayerController>().transform.rotation;
        }
        else if(Owner.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            gameObject.transform.position = GameObject.FindAnyObjectByType<Monster>().transform.position;
            gameObject.transform.rotation = GameObject.FindAnyObjectByType<Monster>().transform.rotation;

        }


        StartCoroutine(nameof(LifeCycle_co));
    }
    

    IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(lifeCycle);
        Managers.Resource.Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (Owner.gameObject.layer == (int)Define.LayerMask.Player)
        {
            if (other.gameObject.layer == (int)Define.LayerMask.Enemy)
            {
                if(other.TryGetComponent(out IDamage idamage))
                {
                    idamage.OnDamage(100);
                }
              
            }
        }
        else if (Owner.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            if (other.gameObject.layer == (int)Define.LayerMask.Player)
            {
                if (other.TryGetComponent(out IDamage idamage2))
                {
                    idamage2.OnDamage(100);
                }

            }
        }
    }
}
