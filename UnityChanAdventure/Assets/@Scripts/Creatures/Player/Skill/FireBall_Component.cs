using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Component : MonoBehaviour
{
    [SerializeField] private float movespeed = 5.0f;
    PlayerController playerController;
    GameObject target;
    Vector3 movedir;
    private void Awake()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        if (playerController.isAttacking)
        {
            SetMove();
        }
        else
        {
            gameObject.transform.RotateAround(playerController.transform.position+Vector3.up,Vector3.up,movespeed*Time.fixedDeltaTime);
        }
    }

    private void OnEnable()
    {
        float angle = 10;
        float radius = 350;   movedir = Vector3.zero;
        movespeed = Random.Range(5, 15);
        angle *= Random.Range(1, 37);

        transform.position = playerController.transform.position + new Vector3(radius * Mathf.Cos(angle) * Mathf.PI / 180.0f, 0, radius * Mathf.Sin(angle) * Mathf.PI / 180.0f);
        StartCoroutine(nameof(LifeCycle_co));
    }
    private void SetMove()
    {
        if (target == null)
        {
            target = Util.GetNbhdMonster(transform.position, (int)Define.LayerMask.Enemy, 100).gameObject;
        }
        if (target == null) return;
        movedir = target.transform.position - gameObject.transform.position;
        transform.Translate(movedir * movespeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            other.GetComponent<IDamage>().OnDamage(15);
            Managers.Resource.Destroy(gameObject);
        }
    }

    private IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(10);
        Managers.Resource.Destroy(gameObject);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
