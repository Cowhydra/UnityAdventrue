using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//최상위 루트( 이 루트 아래에 SafeArea가 적용될 image 등이 있어야 함
//safeArea가 적용될 각종 UI들이 Anchor를 설정해야 함 그리고 그 Anchor는 safeArea밖에 위치 -> 안으로 변경
//결국 safeArea는 Anchor 위치를 조정해주는 것 뿐
//(safeArea 안쪽으로 -> 첨부터 anchor가 safeArea 안에 있으며, 아이콘이 삐져나온 경우 원하는 방식으로 동작 안할 것임

[RequireComponent(typeof(RectTransform))]
public class JCUISafeArea : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] public bool EnableSafeArea = true;
#pragma warning restore 0649

    public RectTransform CachedRectTr { get; private set; }
    public Vector2 CachedAnchorMin { get; private set; }
    public Vector2 CachedAnchorMax { get; private set; }

    public Vector2 SafeAreaAnchorMin { get; private set; }
    public Vector2 SafeAreaAnchorMax { get; private set; }

    private Rect _CachedSafeArea;

    private bool _IsInitialized = false;

    private void OnEnable()
    {
        if (_CachedSafeArea != JCResolutionUtil.SafeArea)
            SetDirty();
    }

    private void Initialize()
    {
        if (_IsInitialized)
            return;

        CachingValues();
        AvoidMultipleCalc();

        _IsInitialized = true;
    }

    /// <summary> 중복 연산 방지 </summary>
    private void AvoidMultipleCalc()
    {
        if (!EnableSafeArea)
            return;

        if (transform.parent == null)
            return;

        //만약 부모에게 JCUISafeArea가 존재하고,EnableSafeArea
        //가 true인 경우에는 적용하지 않음 
        var parent = transform.parent.GetComponentInParent<JCUISafeArea>(true);
        if (parent != null)
        {
            if (parent.EnableSafeArea)
                this.EnableSafeArea = false;
        }
    }

    /// <summary> 기본값 캐싱 </summary>
    private void CachingValues()
    {
        CachedRectTr = transform as RectTransform;

        CachedAnchorMin = CachedRectTr.anchorMin;
        CachedAnchorMax = CachedRectTr.anchorMax;
    }

    /// <summary> SafeArea 계산 및 적용 </summary>
    public void SetDirty()
    {
        if (!enabled)
            return;

        if (!_IsInitialized)
            Initialize();

        _CachedSafeArea = JCResolutionUtil.SafeArea;

        if (!EnableSafeArea)
            return;

        if (JCResolutionUtil.CalcSafeAreaAnchor(out Vector2 minAnchor, out Vector2 maxAnchor))
        {
            SafeAreaAnchorMin = minAnchor;
            SafeAreaAnchorMax = maxAnchor;

            float minX = Mathf.Lerp(minAnchor.x, maxAnchor.x, CachedAnchorMin.x);
            float minY = Mathf.Lerp(minAnchor.y, maxAnchor.y, CachedAnchorMin.y);
            float maxX = Mathf.Lerp(minAnchor.x, maxAnchor.x, CachedAnchorMax.x);
            float maxY = Mathf.Lerp(minAnchor.y, maxAnchor.y, CachedAnchorMax.y);

            CachedRectTr.anchorMin = new Vector2(minX, minY);
            CachedRectTr.anchorMax = new Vector2(maxX, maxY);
            Debug.Log($"{CachedRectTr.gameObject.name} 조절 완료");
            Debug.Log($"(x,y) = {_CachedSafeArea.x}.{_CachedSafeArea.y}");
        }
    }
    public void ChangeScreenTo1080()
    {
        var objects = GameObject.FindObjectsByType<JCUISafeArea>(FindObjectsSortMode.None);
        foreach(var obj in objects)
        {
            obj.GoChange1080();
        }
        Debug.Log("변경 완료?..1080");
    }
    private void GoChange1080()
    {
        StartCoroutine(JCResolutionUtil.ChangeRenderResolutionCoroutine(JCResolutionUtil.ResolutionPreset.Res1080P));
    }
    public void ChangeScreenTo720()
    {
        var objects = GameObject.FindObjectsByType<JCUISafeArea>(FindObjectsSortMode.None);
        foreach (var obj in objects)
        {
            obj.GoChange720();
        }
        Debug.Log("변경 완료?..720");

    }
    private void GoChange720()
    {
        StartCoroutine(JCResolutionUtil.ChangeRenderResolutionCoroutine(JCResolutionUtil.ResolutionPreset.Res720P));
    }
    public void ReApply()
    {
        var objects = GameObject.FindObjectsByType<JCUISafeArea>(FindObjectsSortMode.None);
#if UNITY_EDITOR
        JCResolutionUtil.ResetDeviceValues();
#endif
        JCResolutionUtil.ResetSafeArea();
        Initialize();
        foreach (var obj in objects)
        {
            obj.SetDirty();
        }
    }
#if UNITY_EDITOR && false
    private void Update()
    {
        if(_CachedSafeArea != JCResolutionTool.SafeArea)
            SetDirty();
    }
#endif
}
