using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QSkill_1030 : MonoBehaviour
{
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
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
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
        yield return new WaitForSeconds(10);
        Managers.Resource.Destroy(gameObject);
    }

}
