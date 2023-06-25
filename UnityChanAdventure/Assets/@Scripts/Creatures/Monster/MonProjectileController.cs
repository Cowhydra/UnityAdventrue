using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonProjectileController : MonoBehaviour
{
    [SerializeField]
    private float speed=5f;
    [SerializeField]
    GameObject _target;
    Vector3 moveDir;
    int damage = 10;
    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player");

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }
    public void SetProjectile(Vector3 mypos,int damage=10)
    {

        gameObject.transform.position = mypos;
        this.damage = damage;
        moveDir = (_target.transform.position+0.5f*Vector3.up - gameObject.transform.position).normalized;
        StartCoroutine(nameof(DestoryObject_co));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamage>().OnDamage(damage);
            Managers.Resource.Destroy(gameObject);
        }

    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator DestoryObject_co()
    {
        yield return new WaitForSeconds(3.0f);
        Managers.Resource.Destroy(gameObject);
    }
}
