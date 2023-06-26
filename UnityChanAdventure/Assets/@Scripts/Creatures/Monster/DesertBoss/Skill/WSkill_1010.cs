using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSkill_1010 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(nameof(LifeCycle));
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
        yield return new WaitForSeconds(2);
        DeserBoss_BT.IsAttackChange?.Invoke(false);
        this.enabled=false;
    }
}
