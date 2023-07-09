using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESkill_1010 : MonoBehaviour
{
    private void Update()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = 2* Vector3.one;
    }
    private void Start()
    {
        StartCoroutine(nameof(LifeCycle));

    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage targetojbect))
        {
            targetojbect.OnDamage(20);
        }
    }
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(1.5f);
        Managers.Resource.Destroy(gameObject);
    }
}
