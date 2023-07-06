using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESkill_1030 : MonoBehaviour
{

    IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(8.0f);
        Managers.Resource.Destroy(gameObject);
    }
    private void OnEnable()
    {
        gameObject.transform.position = GameObject.FindObjectOfType<WaterBoss_BT>().gameObject.transform.position+Vector3.up*25;
        gameObject.SetRandomPositionSphere(1, 30);
        StartCoroutine(nameof(LifeCycle_co));
    }
    private void Update()
    {
        transform.position = transform.position - 3.5f*Vector3.up*Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage targetojbect))
        {
            targetojbect.OnDamage(100);
        }
       
    }


}
