using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSkill_1010 : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(nameof(LifeCycle));
    }
    private void Update()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = 100*Vector3.one;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamage targetojbect))
        {
            targetojbect.OnDamage(100);
        }
    }
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(3);
        Managers.Resource.Destroy(gameObject);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
