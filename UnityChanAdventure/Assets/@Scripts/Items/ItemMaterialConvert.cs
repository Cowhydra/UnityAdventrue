using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//단순히 아이템 드랍할 때, Material 바꾸는 구조 
//아이템 들 전부 모델링을 구하기 힘들어서 박스에 넣고 박스 안에 투시되는 진짜 아이템을 넣어 어떠한 아이템인지 확인 가능 
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
