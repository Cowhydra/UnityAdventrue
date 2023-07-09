using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ܼ��� ������ ����� ��, Material �ٲٴ� ���� 
//������ �� ���� �𵨸��� ���ϱ� ���� �ڽ��� �ְ� �ڽ� �ȿ� ���õǴ� ��¥ �������� �־� ��� ���������� Ȯ�� ���� 
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
        //newBaseMapTexture�� DatamAnager���� �ڵ� �޾Ƽ� ���� 
        // Base Map�� �����մϴ�.
        if (newBaseMapTexture != null)
        {
            targetMaterial.SetTexture("_BaseMap", newBaseMapTexture);
        }
    }
}
