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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Managers.Inven.Add(ItemCode);
            Managers.Resource.Destroy(gameObject);
        }
    }
}
