using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaterialConvert : MonoBehaviour
{
    private Material targetMaterial;
    private Texture newBaseMapTexture;
    public int itemcode = -1;
    void Start()
    {
        if (itemcode == -1) return;
        targetMaterial = GetComponentInChildren<Renderer>().material;
        newBaseMapTexture = Managers.Resource.Load<Texture>($"{Managers.Data.ItemDataDict[itemcode].iconPath}");
        //newBaseMapTexture는 DatamAnager에서 코드 받아서 변경 
        // Base Map을 변경합니다.
        if (newBaseMapTexture != null)
        {
            targetMaterial.SetTexture("_BaseMap", newBaseMapTexture);
        }
    }
}
