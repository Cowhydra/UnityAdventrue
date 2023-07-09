using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemoutside : MonoBehaviour
{
    private int _itemcode;
    public int ItemCode
    {
        get { return _itemcode; }
        set
        {
            _itemcode = value;
            SetNewBaseMap();
        }
    }
    [SerializeField]
    private Texture newBaseMapTexture;
    [SerializeField]
    private Material targetMaterial;

    //수 많은 아이템 모델을 구할 수 없어서
    // 2중 박스로 구성되게 하여 , 내부 박스에 아이템 스프라이트 투영
    private void SetNewBaseMap()
    {
        if (targetMaterial == null)
            targetMaterial = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
        
        newBaseMapTexture = Managers.Resource.Load<Texture>($"{Managers.Data.ItemDataDict[_itemcode].iconPath}");
        if (targetMaterial != null && newBaseMapTexture != null)
        {
            targetMaterial.SetTexture("_BaseMap", newBaseMapTexture);
        }
        else
        {
            Debug.LogError("New BaseMap texture is not assigned.");
        }
    }
    //아이템 획득 처리 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Managers.Inven.Add(ItemCode);
            Managers.Resource.Destroy(gameObject);
        }
    }
}
