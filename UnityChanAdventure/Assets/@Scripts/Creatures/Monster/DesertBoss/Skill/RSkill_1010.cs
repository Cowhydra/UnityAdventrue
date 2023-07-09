using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSkill_1010 : MonoBehaviour
{
    private Vector3 _movedir=Vector3.zero;
    private float _speed;
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        StartCoroutine(nameof(LifeCycle));
    }
    private void Update()
    {
        transform.localScale = 5*Vector3.one;
        transform.localPosition = new Vector3(transform.localPosition.x, 0.4f, transform.localPosition.z);
        transform.Translate(_movedir * Time.deltaTime * _speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage targetojbect))
        {
            targetojbect.OnDamage(5);
        }
        else if (other.gameObject.layer == 1 << (int)Define.LayerMask.DestoryableEnv)
        {
            Managers.Resource.Destroy(other.gameObject);
        }
    }
    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(6);
        Managers.Resource.Destroy(gameObject);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void SetDir(Vector3 dir, float speed)
    {
        _movedir = dir;
        _speed = speed;
    }
}
