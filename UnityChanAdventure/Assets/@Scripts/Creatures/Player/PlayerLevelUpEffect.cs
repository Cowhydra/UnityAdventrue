using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUpEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(nameof(LifeCycle_co));
    }


    IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(1.2f);
        Managers.Resource.Destroy(gameObject);
    }
}
