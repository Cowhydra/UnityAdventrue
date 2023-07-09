using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectMyCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach (Transform transforom in gameObject.GetComponentInChildren<Transform>())
        {
            Managers.Resource.Destroy(transforom.gameObject);
        }
        foreach (var i in Managers.Data.CharacterDataDict.Keys)
        {
            MyCharacter_SelectScene Character = Managers.UI.ShowWorldUI<MyCharacter_SelectScene>();
            Character.transform.SetParent(gameObject.transform);
            Character.Charcode = i;
        }
    }
}
