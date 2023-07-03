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
    private Texture newBaseMapTexture;

    private Renderer renderer;
    private Material material;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
     
    }

    private void SetNewBaseMap()
    {
        newBaseMapTexture = Managers.Resource.Load<Texture>($"{Managers.Data.ItemDataDict[_itemcode].iconPath}");
        if (newBaseMapTexture != null)
        {
            material.SetTexture("_BaseMap", newBaseMapTexture);
            renderer.material = material;
        }
        else
        {
            Debug.LogError("New BaseMap texture is not assigned.");
        }
    }
}
