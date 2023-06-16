using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaterialConvert : MonoBehaviour
{
    private Material targetMaterial;
    public Texture newBaseMapTexture;
    public int itemcode = -1;
    void Start()
    {
        if (itemcode == -1) return;
        targetMaterial = GetComponentInChildren<Renderer>().material;
        //newBaseMapTexture�� DatamAnager���� �ڵ� �޾Ƽ� ���� 
        // Base Map�� �����մϴ�.
        if (newBaseMapTexture != null)
        {
            targetMaterial.SetTexture("_BaseMap", newBaseMapTexture);
        }
    }
}
