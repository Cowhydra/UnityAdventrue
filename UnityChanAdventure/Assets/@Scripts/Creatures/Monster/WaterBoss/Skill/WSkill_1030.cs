using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSkill_1030 : MonoBehaviour
{
    [SerializeField]
    private float downforce=4;
    GameObject Player;
    Vector3 movedir;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnEnable()
    {
        movedir = Player.transform.position - transform.position;
       
        SetPos(Player.transform);
        gameObject.SetRandomPositionSphere(15, 30);
    }
    private void Update()
    {
        transform.localScale = 2 * Vector3.one;
        movedir = Player.transform.position - transform.position;
        transform.Translate(movedir * 1.5f* Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage targetojbect))
        {
            targetojbect.OnDamage(100);
            Managers.Resource.Destroy(gameObject);
        }
        else if (other.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            Managers.Resource.Destroy(gameObject);
        }
    }
    public void SetPos(Transform transform)
    {
        this.transform.position=transform.position +Vector3.up;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
