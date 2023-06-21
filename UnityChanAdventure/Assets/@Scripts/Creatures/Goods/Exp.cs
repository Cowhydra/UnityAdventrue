using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    private int _value;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<MyCharacter>().Exp += _value;
        }
    }
    public void SetValue(int _level)
    {
        _value = _level*20 + Random.Range(50, 100);
    }
}
