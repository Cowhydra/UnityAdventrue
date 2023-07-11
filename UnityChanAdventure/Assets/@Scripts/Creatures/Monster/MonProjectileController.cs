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
    //몬스터의 프로젝타일이니 타켓 = 플레이어 
    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player");

    }
    // Update is called once per frame
    private void OnEnable()
    {
        Util.LifeCycle_co(gameObject, 4.0f);
        StartCoroutine(nameof(MoveDir_Check));
    }
    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }
    public void SetProjectile(Vector3 mypos,int damage=10)
    {

        gameObject.transform.position = mypos;
        this.damage = damage;
        moveDir = (_target.transform.position + 0.15f * Vector3.up - gameObject.transform.position).normalized;
    }
    private IEnumerator MoveDir_Check()
    {
        moveDir = (_target.transform.position + 0.15f * Vector3.up - gameObject.transform.position).normalized;
        yield return new WaitForSeconds(5.0f);
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
}
