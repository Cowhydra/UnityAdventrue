using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDieEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Destory_co));   
    }
    private IEnumerator Destory_co()
    {
        yield return new WaitForSeconds(1.5f);
        Managers.Resource.Destroy(gameObject);
    }
    public void SetPosition(Transform transform)
    {
        gameObject.transform.position = transform.position;
    }
}
