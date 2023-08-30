using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(CanvasScaler))]
public class CanvasOptionTest : MonoBehaviour
{
    public CanvasScaler Target;

    // Start is called before the first frame update
    void Start()
    {
        Target = GetComponent<CanvasScaler>();
        Debug.Log($"MyName: {gameObject.name}");
        Debug.Log($"Ref Resoluton: {Target.referenceResolution}");
        Debug.Log($"match width or height {Target.matchWidthOrHeight}");

    }



}
