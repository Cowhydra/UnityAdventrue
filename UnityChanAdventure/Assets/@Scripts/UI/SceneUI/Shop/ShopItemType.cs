using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemType : MonoBehaviour
{
    enum MyItemType
    {
        Weapon,
        Boot,
        Cloth,
        Earring,
        Hat,
        Ring,

    }
    [SerializeField] private MyItemType _myItemType;
    private void Start()
    {
        foreach (Transform transforom in gameObject.GetComponentInChildren<Transform>())
        {
            Managers.Resource.Destroy(transforom.gameObject);
        }

        switch (_myItemType)
        {

        }
    }

}
