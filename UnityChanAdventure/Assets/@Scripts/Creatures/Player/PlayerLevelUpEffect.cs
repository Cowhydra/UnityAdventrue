using System.Buffers;
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
    private Transform Onwer;
    private void Awake()
    {
        Onwer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        gameObject.transform.position = Onwer.transform.position + 0.2f * Vector3.up;
    }
    IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(1.2f);
        Managers.Resource.Destroy(gameObject);
    }
}
