using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QSkill_1010 : MonoBehaviour
{

    private bool EffectOn = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamage targetobject))
        {
            if (!EffectOn)
            {
                StartCoroutine(nameof(Attack_co), targetobject);
            }
        }
    }
    private void Start()
    {
        StartCoroutine(nameof(Attack_Time));
    }
    IEnumerator Attack_co(IDamage _object)
    {
        EffectOn=true;
        _object.OnDamage(10);
        yield return new WaitForSeconds(1.2f);
        EffectOn = false;
    }
    IEnumerator Attack_Time()
    {
        yield return new WaitForSeconds(7);
        GetComponent<WSkill_1010>().enabled = true;
        this.enabled = false;
    }

}
