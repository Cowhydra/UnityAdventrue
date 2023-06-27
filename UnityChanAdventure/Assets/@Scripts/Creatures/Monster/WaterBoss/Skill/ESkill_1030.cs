using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESkill_1030 : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(nameof(LifeCycle));
    }
    private float currentScale=1;
    private void Update()
    {
        currentScale += Time.deltaTime;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * currentScale;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage targetojbect))
        {
            targetojbect.OnDamage(100);
        }
    }
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(15);
        Managers.Resource.Destroy(gameObject);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
