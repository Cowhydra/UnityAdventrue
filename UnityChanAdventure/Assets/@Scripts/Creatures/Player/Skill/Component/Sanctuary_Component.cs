using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanctuary_Component : MonoBehaviour
{
    private bool isinit = false;
    private Transform _Owner;
    public Transform Owner
    {
        get { return _Owner; }
        set
        {
            _Owner = value;
            isinit = true;
 
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == Owner.gameObject.layer)
        {
            StartCoroutine(nameof(OnHeal_Cooltime));
        }
       
    }
    private bool ishealing = false;
    private IEnumerator OnHeal_Cooltime()
    {
        if (ishealing)
        {
            yield break;
        }
        else
        {
            ishealing = true;
            if (Owner.gameObject.layer == (int)Define.LayerMask.Player)
            {
                Owner.GetComponent<MyCharacter>().Hp += 30;
            }
            else if (Owner.gameObject.layer == (int)Define.LayerMask.Enemy)
            {
                Owner.GetComponent<Monster>().Hp += 30;

            }
            yield return new WaitForSeconds(2.0f);
            ishealing = false;
        }
    }
    private IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(12.0f);
        Managers.Resource.Destroy(gameObject);
    }
 
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    void Update()
    {
        if (isinit)
        {
            gameObject.transform.position = Owner.transform.position+0.5f*Vector3.up;
        }

    }
}
