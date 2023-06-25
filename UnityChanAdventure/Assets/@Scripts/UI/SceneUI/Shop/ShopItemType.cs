using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemType : MonoBehaviour
{

    [SerializeField] private Define.ItemType _myItemType;
    private void Awake()
    {
        foreach (Transform transforom in gameObject.GetComponentInChildren<Transform>())
        {
            Managers.Resource.Destroy(transforom.gameObject);
        }

       foreach(var i in Managers.Data.ItemDataDict.Keys)
        {
            if (_myItemType == Managers.Data.ItemDataDict[i].itemType)
            {
                Shop_Item shopitem=Managers.UI.ShowSceneUI<Shop_Item>();
                shopitem.MyItemCode = i;
                shopitem.transform.SetParent(gameObject.transform);
            }
        }

    }
  

}
