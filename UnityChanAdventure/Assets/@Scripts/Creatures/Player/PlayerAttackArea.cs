using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : MonoBehaviour
{
    [SerializeField]
    private float attacktime=0.1f;
    [SerializeField]
    private int baseAttackdamage = 15;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            if(other.TryGetComponent(out IDamage damage))
            {
                damage.OnDamage(baseAttackdamage);
            }
            else
            {
                Debug.Log("IDmage ±¸ÇöX");
            }
        }
    }
    private void OnEnable()
    {
        StartCoroutine(nameof(OnAttack_co));
    }
    private IEnumerator OnAttack_co()
    {
        yield return new WaitForSeconds(attacktime);
       gameObject.SetActive(false);
    }
    public void SetDamage(int damage)
    {
        baseAttackdamage= damage;
    }
    
}
