using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemType : MonoBehaviour
{
    //상점 타입에 따라서 아이템 다르게 설정 
    //동적으로 해주지 않았음; 만약 동적으로 했다면 이 위의 Shop 등에서ㅗ
    //ShopItemType을 ItemType 개수만큼, 혹은 파는 아이템 만큼 생성해서 할당 했을 것
    // 귀찮음 + 나중에 어드레서블 안대면 뭐 추후 수정 
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
