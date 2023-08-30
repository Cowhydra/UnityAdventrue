using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//#region Test
//[RequireComponent(typeof(CanvasScaler))]
//public class JCUICanvasScalerAddon : MonoBehaviour
//{
//    #region Static members

//    public static List<JCUICanvasScalerAddon> Currents { get; private set; } = new List<JCUICanvasScalerAddon>();

//    public static void SetDirtyAll()
//    {
//        foreach (var scaler in Currents)
//        {
//            if (scaler == null)
//                continue;
//            scaler.SetDirty();
//        }
//    }

//    #endregion

//    public bool EnableMatchSwapOnRectScreen = true;
//    public bool UseScaleChange = false;

//    public CanvasScaler Target
//    {
//        get
//        {
//            if (object.ReferenceEquals(_Target, null))
//                _Target = GetComponent<CanvasScaler>();
//            return _Target;
//        }
//    }

//    private CanvasScaler _Target;
//    private float _CachedMatchValue;
//    private Vector2 _CachedRefResolution;

//    private Rect _CachedSafeArea;


//    private void Awake()
//    {
//        CachingValues();

//        SetDirty();

//        //CUrrents를 통해 JCUICanvasScalerAddon 컴포넌트를 가진 모든 오브젝트들을 등록해둠 --> 해상도 변경 같은거 진행될 경우 일괄적용을 위해서?...
//        if (!Currents.Contains(this))
//            Currents.Add(this);
//    }

//    private void OnDestroy()
//    {
//        if (Currents.Contains(this))
//            Currents.Remove(this);
//    }

//    private void OnEnable()
//    {
//        RegisterListeners(JCMsnLRT.ADD_LISTENER);

//        //처음 시작할 때 
//        if (_CachedSafeArea != JCResolutionUtil.SafeArea)
//            SetDirty();
//    }

//    private void OnDisable()
//    {
//        //등록된 이벤트 제거 
//        RegisterListeners(JCMsnLRT.REMOVE_LISTENER);
//    }

//    private void RegisterListeners(JCMsnLRT rt)
//    {
//        //스케일이 변하면..? 변경시킬 때?.. 실행되는 함수등록하는 것 같습니다.
//        JCMessenger<float>.RegisterListener(rt, JCMsgID.UI_OPT_SCALE_FACTOR_CHANGED, SetScaleFactor);
//    }

//    private void CachingValues()
//    {
//        // Cached 하는것 ( CanvasScaler 가 ScaleWithScreenSize일 경우
//        // matchWidthOrHeight  -> 캔버스 영역의 너비 혹은 높이 중 어떤 것을 참고하여 할지
//        //mathwidth or height ->  0일 경우 Expand(수평,수직으로 확장)와 동일, 1 일 경우 Shrink(수평,수직으로 잘라냄)와 같다.
//        //0일 경우 가로기준.. (가로폭이 모두 보이도록  설정 ->새로 방향 빈공간 생성
//        // 1일 경우 세로기준 ( 세로가 다 보이며, 가로에 여백 생길 수 있음( ㅁ ㅣㅣㅁ) 느낌

//        //보통 가로가 더 길 경우 Match 값을 0 세로가 더 길 경우 Macth 값을 1로 설정함 아니면 0.5로 설정 가능

//        // referenceResolution 값 -> 기준이 되는 해상도...
//        _CachedMatchValue = Target.matchWidthOrHeight;
//        _CachedRefResolution = Target.referenceResolution;
//    }

//    public void SetDirty()
//    {
//        if (!enabled)
//            return;

//        StartCoroutine(SetDirtyInternal());
//    }

//    private IEnumerator SetDirtyInternal()
//    {
//        SwapMatch();
//        AdditionalScalingForSafeArea();

//        if (UseScaleChange)
//        {
//            yield return null;
//            SetScaleFactor(JCAppOption.graphicsPreference.UIScaleFactor);
//        }
//    }

//    private void SwapMatch()
//    {

//        //Match 값 을 변경하는 건데.. 굳이 변경을 왜?...
//        // 와이드 화면이거나, Swap 옵션을 사용하지 않으면 그냥 넘김 
//        if (JCResolutionUtil.IsWideScreen() || !EnableMatchSwapOnRectScreen)
//            return;
//        //보통 화면에서 가로가 더 길기 때문 Default 값으로 mathwidhtOrheight 값을 0으로 해두는 경우가 많음....
//        //아마도 WIde화면의 경우 가로가 더 길기 때문에 그냥 넘기는듯?..
//        //만약 세로가 더 긴 화면ㅇ 비율이라면 -> 세로 화면 보존을 위해.. 1-_CachedMatchValue 하는듯?..

//        Target.matchWidthOrHeight = 1f - _CachedMatchValue;
//    }

//    private void AdditionalScalingForSafeArea()
//    {
//        if (!JCResolutionUtil.SafeAreaEnabled)
//            return;

//        _CachedSafeArea = JCResolutionUtil.SafeArea;

//        // SafeArea 적용시 SafeArea영역이 Scaler의 레퍼런스 해상도보다 작아졌을 경우 UI가 뭉개지는 걸 방지하기 위해 추가적으로 스케일을 조절
//        // UI디자인이 16:9비율을 기준으로 하고있기 때문에 현재 해상도를 16:9로 맞춘 해상도로 판단한다.
//        var refRes = JCResolutionUtil.GetBaseReferanceResolution();

//        //여기서 뭉개진다는 뜻은 안보인다는 것이 아니라 겹쳐진다는 뜻 같습니다.

//        float scale = 1f;

//        //항상 safeArea.y 는 refRes.y 보다 작지 않나?.. x도 마찬가지


//        if (Target.matchWidthOrHeight >= 0.5f && JCResolutionUtil.SafeArea.y < refRes.y)
//            scale = JCResolutionUtil.RenderScreenSize.y / JCResolutionUtil.SafeArea.height;
    

//        //가로기준에서
//        else if (Target.matchWidthOrHeight < 0.5f && JCResolutionUtil.SafeArea.x < refRes.x)
//            scale = JCResolutionUtil.RenderScreenSize.x / JCResolutionUtil.SafeArea.width;


//        //scale은 비율인데
//        //결국 기존 Device 전체 화면에서 Ui를 배치 해야 하는 부분이
//        //safeArea영역 내로 UI를 배치하는 것으로 바뀜 그로 인해 생기는 간극을 완화시켜주기 위한 코드로 보입니다.
//        //SafeArea로 적용되면 기존화면 영역이 작아짐 ->
//        //?... 아무 튼 그래서  설정에 따라서 UI가 작아지나 봅니다. 그래서 작아진 UI를 작아진 비율에 맞게 키워주는 과정이라고 생각됩니다.

//        // RenderScreenSize/ SafeAre 의 값은 1보다 클 수밖에 없음->
//        // 이렇게 해서 scale 값을 적용해서 referenceResolution 값을 크게 해준다면  Scale With ScreenSize 모드를 사용할 때
//        // (0,0) - (referenceResolution.x, referenceResolution.y) 값을 가지게되는 좌표계에서 local scale을 이용해 자동으로 유니티가 보정해주었던 
//        //부분들이 존재하게 됩니다.
//        //이 때 넓은 곳( safe Area를 적용하기 전에서  작은곳으로 몰리게 되어 UI 크기가 커 기존에 겹쳐지게 되었던 UI들이 localscale이 작아짐에 따라서
//        //UI 크기가 작게 변화되어 뭉게지지 않게 변경될 것이다.


//        Target.referenceResolution = _CachedRefResolution * scale;

//        //referenceResolution 값은 비율 -> ( AnchoredPosX / referenceResolution.x

//    }

//    private void SetScaleFactor(float scaleFactor)
//    {
//        if (Target == null)
//            return;
//        if (!UseScaleChange)
//            return;
//        //render 모드가 ConstantPixelSize 일떄만 적용되는 부분으로 봐야될 것 같습니다
//        if (Target.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize)
//            Target.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        
//        var refRes = JCResolutionUtil.GetBaseReferanceResolution();
//        bool isWide = JCResolutionUtil.IsWideScreen();
//        float scale = 1f;


//        //ConstantPixelSize 인 경우에는
//        //결국 이것 또한 화면이 줄어들지만, 크기는 동일하게 가져가기 떄문에 UI들이 뒤엉킬수 있게 되어,
//        // 적절한 값을 곱해줘 UI들의 뒤엉킴을 방지하는 것 

//        if (isWide && refRes.y != JCResolutionUtil.REF_RESOLUTION_HEIGHT)
//            scale = refRes.y / JCResolutionUtil.REF_RESOLUTION_HEIGHT;
//        else if (!isWide && refRes.x != JCResolutionUtil.REF_RESOLUTION_WIDTH)
//            scale = refRes.x / JCResolutionUtil.REF_RESOLUTION_WIDTH;


//        scaleFactor *= scale;
//        Target.scaleFactor = scaleFactor;
//    }

//    private float GetScale(int width, int height, Vector2 scalerReferenceResolution, float scalerMatchWidthOrHeight)
//    {
//        return Mathf.Pow(width / scalerReferenceResolution.x, 1f - scalerMatchWidthOrHeight) *
//               Mathf.Pow(height / scalerReferenceResolution.y, scalerMatchWidthOrHeight);
//    }

//}


//#endregion