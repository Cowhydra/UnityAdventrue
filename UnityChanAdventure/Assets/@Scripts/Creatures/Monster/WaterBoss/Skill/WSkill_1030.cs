using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSkill_1030 : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localPosition = Vector3.up;
        
    }
    private void Update()
    {
        transform.localScale = 2 * Vector3.one;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage targetojbect))
        {
            targetojbect.OnDamage(100);
        }
        else if (other.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            Managers.Resource.Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
