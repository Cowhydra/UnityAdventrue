using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESkill_1010 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(nameof(LifeCycle));
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
        DeserBoss_BT.IsAttackChange?.Invoke(false);
        this.enabled = false;
    }
}
