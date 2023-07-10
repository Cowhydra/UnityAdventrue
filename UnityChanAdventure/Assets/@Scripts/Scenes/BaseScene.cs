using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.None;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";

        Managers.Sound.Play($"{gameObject.name}", Define.Sound.Bgm);
        Managers.Game.GoldChange(2000);
    }

    public abstract void Clear();
}
