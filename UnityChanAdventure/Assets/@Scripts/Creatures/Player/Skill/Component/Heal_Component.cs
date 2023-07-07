using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal_Component : MonoBehaviour
{
    private Transform _Owner;
    public Transform Owner
    {
        get { return _Owner; }
        set {
            _Owner = value;
            Heal();
            isinit = true;
        }
    }
    private bool isinit = false;
    // Start is called before the first frame update
    private void Heal()
    {
        if (Owner.gameObject.layer == (int)Define.LayerMask.Player)
        {
            Owner.GetComponent<MyCharacter>().Hp += Mathf.RoundToInt(Owner.GetComponent<MyCharacter>().MaxHp * 0.5f);
        }
        else if (Owner.gameObject.layer == (int)Define.LayerMask.Enemy)
        {
            Owner.GetComponent<Monster>().Hp += Mathf.RoundToInt(Owner.GetComponent<Monster>().MaxHp * 0.5f);

        }
        StartCoroutine(nameof(LifeCycle_co));
    }
 
    private IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(4.0f);
        Managers.Resource.Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (isinit)
        {
            gameObject.transform.position = Owner.transform.position;
        }

    }
}
