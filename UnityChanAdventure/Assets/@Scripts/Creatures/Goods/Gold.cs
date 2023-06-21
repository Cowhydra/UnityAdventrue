using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    private int _value;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Managers.Game.GoldChange(_value);
        }
    }
    public void SetValue(int _level)
    {
        _value = _level * 20 + Random.Range(50, 100);
    }
}
