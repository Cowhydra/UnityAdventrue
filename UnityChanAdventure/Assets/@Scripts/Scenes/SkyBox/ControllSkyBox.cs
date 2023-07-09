using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllSkyBox : MonoBehaviour
{
    [SerializeField]
    private Define.Scene _sceneType;
    [SerializeField]
    Material[] materials;
    private void Start()
    {
        _sceneType = Managers.Scene.CurrentScene.SceneType;
        RenderSettings.skybox = materials[(int)_sceneType];
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.5f);
    }


}
