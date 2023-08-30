#if UNITY_EDITOR
#define DEBUG_LOG
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//해상도(720p , 1080p) 변경과, 그 변경에 따른 SafeArea 관리 
public static class JCResolutionUtil
{
    #region Definitions 

    //해상도 기본값 ( 720p)
    public const int REF_RESOLUTION_WIDTH = 1280;
    public const int REF_RESOLUTION_HEIGHT = 720;

    // 해상도 비율 ( 16:9 -> 가로/세로)  와이드 여부를 해당 비율 로 판단
    private const float REFERANCE_ASPECT = 16f / 9f;

    // 해상도를 나타내는 구조체
    public struct RefResolution
    {
        //각각 비율,너비(가로),높이(세로)
        public float Aspect { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        //구조체 생성자 로 속성들 정의
        public RefResolution(int w, int h)
        {
            Width = w; Height = h;
            Aspect = (float)w / h;
        }
        //현재 너비 높이를 Vector2로 반환
        public Vector2Int ToVector2Int()
        {
            return new Vector2Int(Width, Height);
        }
    }

    // 지원하는 해상도 프리셋 Enum 으로 설정
    public enum ResolutionPreset
    {
        None,
        Res720P,
        Res1080P,
    }

    // 해상도 프리셋, 실제 해상도 크기를 Dictionary에 저장  ===> 720 (1280*720) , 1080 (1920*1080)
    public readonly static Dictionary<ResolutionPreset, RefResolution> PRESETS = new Dictionary<ResolutionPreset, RefResolution>() {
        {ResolutionPreset.Res720P, new RefResolution(REF_RESOLUTION_WIDTH, REF_RESOLUTION_HEIGHT) },
        {ResolutionPreset.Res1080P, new RefResolution(1920, 1080) },
    };

    #endregion

    #region Cache values

    // UGUI SafeArea 처리를 위한 변수
    private static Rect _PrevCheckedSafeArea; //캐시용?? -> 이전  프레임 SafeArea
    private static Vector2 C_RTMinAnchor;
    private static Vector2 C_RTMaxAnchor;       //앵커 값 조절 으로 SafeArea 

    private static bool _IsInitialized = false;
    private static Vector2Int? _BaseResolution; //기반이 되는 해상도?? [ 게임이 어떠한 해상도로 제작되어 있는지??? 기본 해상도 유니티 첫 시작시 설정? ]

    #endregion

    #region Properties

    // 현재 게임 화면 해상도 및 Safe Area 정보를 제공하는 속성들
    public static Vector2Int RenderScreenSize { get; private set; } = new Vector2Int(Screen.width, Screen.height); //현재 스크린 사이즈 반환
    //현재 적용 되어야 하는 SafeArea의 크기를 반환 (기본적으로 그냥 Screen.SafeArea를 쓰면 되지만, 양쪽 모두 적용이 필요한 경우 때문에 Cutout 함수를 적용
    public static Rect SafeArea { get; private set; } = CutoutSafeAreaBothSide(Screen.safeArea);

    //랜더링 스케일 과 실제 화면 해상도 사이의 비율?.. -> 게임 해상도가 실제 화면 해상도와 다를 때 적용?..
    //왜 쓰는지 일단 모름
    public static float RenderScale { get; private set; } = 1f;

    //현 스크린 크기? 디바이스 크기? 구하는데 쓰는듯 값을 그냥 Screen.width, Screen.height만 구해서 사용합니다.
    public static Vector2Int DeviceScreenSize { get; private set; }
    //이것도 Device 가 제공하는 safre Area 크기를 저장해두는 용도?
    public static Rect DeviceSafeArea { get; private set; }
    public static ResolutionPreset CurrentResolutionPreset { get; private set; } = ResolutionPreset.Res720P;
    //현재 기본으로 설정해둔 (CurrentResolutionPreset 에 따른 기본 해상도 값 불러오는 함수 입니다.
    public static Vector2Int BaseResolution
    {
        get
        {
            if (_BaseResolution == null)
            {
                if (!PRESETS.TryGetValue(CurrentResolutionPreset, out var value))
                    value = new RefResolution(REF_RESOLUTION_WIDTH, REF_RESOLUTION_HEIGHT);
                _BaseResolution = new Vector2Int?(new Vector2Int(value.Width, value.Height));
            }
            return _BaseResolution.Value;
        }
    }

    #endregion

    // 초기화 메서드
    public static void Initialize()
    {
        if (_IsInitialized)
            return;
        //기본적으로 초기화 할때에는 설정해둔 기본 스크린 사이즈, 디바이스 기본의 세이프 에어리어 를 가져옵니다.
        DeviceScreenSize = new Vector2Int(Screen.width, Screen.height);
        DeviceSafeArea = new Rect(Screen.safeArea);
        _IsInitialized = true;

        TestLog($"Initialize> DeviceScreenSize{DeviceScreenSize} DeviceSafeArea{DeviceSafeArea}");
    }

#if UNITY_EDITOR
    /// <summary>
    /// 에디터에서 테스트 중에 UI가 꼬였을 때 사용하는 용도임. 
    /// 절대 인게임에서 호출하지 말아야 합니다!
    /// </summary>
    public static void ResetDeviceValues()
    {
        //이거는 summary에 있듯이 다시 초기 세팅으로 되돌리는 함수 같습니다.
        DeviceScreenSize = new Vector2Int(Screen.width, Screen.height);
        DeviceSafeArea = new Rect(Screen.safeArea);
        ResetRenderScreenSize();
    }
#endif

    [System.Diagnostics.Conditional("DEBUG_LOG")]
    private static void TestLog(string message)
    {
        Debug.Log($"#Resolution# {message}");
    }

    // 해상도 변경을 위한 코루틴
    public static IEnumerator ChangeRenderResolutionCoroutine()
    {
        //테스트용 함수인 것 같습니다.-> 아래 ChangeRenderResolutionCoroutine 이 코루틴을 preset 그냥 720으로 해서 실행하는 것
        yield return ChangeRenderResolutionCoroutine(ResolutionPreset.Res720P);
    }

    //preset을 이용해 (변경해야할 화면의 크기를 가져와서 -> 실제로 변경 )
    public static IEnumerator ChangeRenderResolutionCoroutine(ResolutionPreset preset)
    {
        //초기화 되지 않았으면 우선 초기화 먼저 
        if (!_IsInitialized)
        {
            Debug.LogError($"#Resolution# JCResolutionUtil> Call Initialize() first!");
            Initialize();
        }
        //Preset을 일단 저장해둠 (바뀌에서 현재를 나타내는..)
        CurrentResolutionPreset = preset;
        //계산된 해상도 값 resolution
        Vector2Int resolution = CalcRenderResolution(preset);
        Debug.Log($"!!!!!!Result ReSolution is ({resolution.x},{resolution.y})");
        Debug.Log($"!!!!!CurrDevice ReSolution is ({RenderScreenSize.x},{RenderScreenSize.y})");
        //이 값을 이용해  ChangeRenderResolutionCoroutine 을 이용해 실제 화면 계산??
        yield return ChangeRenderResolutionCoroutine(resolution);
    }

    public static IEnumerator ChangeRenderResolutionCoroutine(Vector2Int resolution)
    {
        if (!_IsInitialized)
        {
            Debug.LogError($"#Resolution# JCResolutionUtil> Call Initialize() first!");
            Initialize();
        }

        //변경할 해상도와, 현재 랜더러 스크린 사이즈 ( 기본값 Screen 사이즈 ) 와 다르면 변경 실행
        if (resolution != RenderScreenSize)
        {
            SetRenderResolution(resolution.x, resolution.y);
            yield return null;
        }

        // 해상도 변경이 실제로 일어날 때까지 잠시 대기 (최대 3프레임)
        byte loop = 3;

        do
        {
            //여기서 Reset을 이용해 얻을 수 있는 값은 -> RenderScale 값  SetRenderResolution 한 해상도와, 기존 해상도 사이의 값 비교를 통해 
            //RenderScale 값을 추출한다.
            Debug.Log($"변경 전 사이즈 : {RenderScale}");
            ResetRenderScreenSize();
            Debug.Log($"변경 후 사이즈 : {RenderScale}");

            --loop;
            yield return null;
        } while (resolution != RenderScreenSize && 0 < loop);
        //그래서 해상도가 변경된 이후 -> 변경된 해상도에 따른 RenderScale을 진행하는 작업이었습니다. do 를 쓴 이유는 명확하게 한번 은 실행하기 위해서??


        ResetSafeArea();
        //ResetSafeArea를 이용해 변경된 해상도에 따른 SafeArea 재계산 
        //SafeArea
        CalcSafeAreaAnchor(SafeArea, out C_RTMinAnchor, out C_RTMaxAnchor);

        // 모든 Canvas의 Scaler를 조정하는 건가?.. 근데 밑의 ApplySafeArea 라는 함수가 존재
      //  JCUICanvasScalerAddon.SetDirtyAll();

        TestLog($"<b>Target{resolution}\nCurrentResolution({Screen.width}, {Screen.height})\nRenderScreenSize{RenderScreenSize}\nJCSafeArea({SafeArea}) RenderScale({RenderScale})</b> loop({loop})");
    }

    #region Calc screen resolution

    /// <summary>
    /// 현재 해상도에 맞는 16:9 비율 해상도 계산
    /// </summary>
    public static Vector2 GetBaseReferanceResolution()
    {
        if (IsWideScreen())
            return new Vector2(RenderScreenSize.y * REFERANCE_ASPECT, RenderScreenSize.y);
        else
            return new Vector2(RenderScreenSize.x, RenderScreenSize.x / REFERANCE_ASPECT);
    }

    ///<summary>
    /// 기기의 실제 화면 비율에 맞춰서 게임 해상도를 720P로 변경
    ///</summary>
    // 아이패드의 경우엔 가로를 기준으로 1280에 맞춤..
    // 실제 해상도가 기본 해상도보다 작을 때는 실제 해상도를 기준으로...
    public static Vector2Int CalcRenderResolution()
    {
        bool isWide = IsWideScreen(out float aspect);

        // 결과값(UNITY 해상도의 각 축의 값은 int형이다)
        Vector2Int result;

        // 와이드 화면일 때
        if (isWide)
        {
            int h = Mathf.Min(DeviceScreenSize.y, REF_RESOLUTION_HEIGHT);
            int w = Mathf.RoundToInt(h * aspect);
            result = new Vector2Int(w, h);
        }
        // 아이패드 혹은 서피스 같이 와이드 비율이 아닐 때
        else
        {
            int w = Mathf.Min(DeviceScreenSize.x, REF_RESOLUTION_WIDTH);
            int h = Mathf.RoundToInt(w / aspect);
            result = new Vector2Int(w, h);
        }

        TestLog($"CalcRenderResolution> result{result}");

        return result;
    }

    ///<summary>
    /// 기기의 실제 화면 비율에 맞춰서 게임 해상도를 프리셋 값으로 변경
    ///</summary>
    // 아이패드의 경우엔 가로를 기준으로 1280에 맞춤..
    // 실제 해상도가 기본 해상도보다 작을 때는 실제 해상도를 기준으로...
    public static Vector2Int CalcRenderResolution(ResolutionPreset preset)
    {
        //현재 PRESET 딕셔너리에 저장된 preset의 RefResolution 값을 가져오는데
        //만약 없는 프리셋이면 -> 720p로 설정한 값을 가져옴
        if (!PRESETS.TryGetValue(preset, out RefResolution refRes))
            refRes = new RefResolution(REF_RESOLUTION_WIDTH, REF_RESOLUTION_HEIGHT);

        //와이드 인지 확인 -> 기준은 16:9로 잡혀있음 
        //out을 통해 aspect를 정의함과 동시에 가져옴
        bool isWide = IsWideScreen(out float aspect);
  
        
        // 결과값(UNITY 해상도의 각 축의 값은 int형이다)
        Vector2Int result;

        // 와이드 화면일 때
        if (isWide)
        {
            //현재 디바이스의 세로 화면 크기 (해상도 크기와 화면 크기 중 작은 값)
            int h = Mathf.Min(DeviceScreenSize.y, refRes.Height);
            //가로 화면 크기를 세로 크기를 기준으로 계산한다.
            int w = Mathf.RoundToInt(h * aspect);

            result = new Vector2Int(w, h);
        }
        // 아이패드 혹은 서피스 같이 와이드 비율이 아닐 때
        //반대로 가로를 기준으로 세로 크기를 설정 
        else
        {
            
            int w = Mathf.Min(DeviceScreenSize.x, refRes.Width);
            int h = Mathf.RoundToInt(w / aspect);
            result = new Vector2Int(w, h);
        }

        TestLog($"CalcRenderResolution> result{result}");

        //계산된 해상도 값? 을 반환한다.
        return result;
    }

    // 기기의 화면 비율이 와이드 스크린인지 여부 확인
    public static bool IsWideScreen()
    {
        return IsWideScreen(out _);
    }

    public static bool IsWideScreen(out float aspect)
    {
        // 기기 화면 비율
        // 16:9 비율을 기준으로 세로가 길어지는 aspect 값은 1.6 이하이다.
        // 5:4 -> 1.25
        // 4:3 -> 1.33..(iPad)
        // 3:2 -> 1.5(iPhone 4 이전 등)
        // 16:10 -> 1.6(안드로이드 태블릿)
        // 16:9 -> 1.77..(일반 와이드 스크린 비율)
        // 18.5:9 -> 2.055..(갤럭시 S8/S9)
        // 20:9 -> 2.22..(갤럭시 S20)
        aspect = DeviceScreenSize.x / (float)DeviceScreenSize.y;
        return REFERANCE_ASPECT <= aspect;
    }

    public static void SetRenderResolution(Vector2 renderSize)
    {
        SetRenderResolution((int)renderSize.x, (int)renderSize.y);
    }

    public static void SetRenderResolution(int width, int height)
    {
        TestLog($"SetRenderResolution> Resolution change from ({Screen.width}x{Screen.height}) to ({width}x{height})");
        //SetResolution 은 유니티 내부에서 현재 랜더링 해상도를 변경하기 위해 사용되는 메서드 
        //설정해둔 높이,너비 를 이용해 변경 -> true의 경우 전체 화면인가??를 나타냅니다.
        Screen.SetResolution(width, height, true);
    }

    public static void ResetRenderScreenSize()
    {
        RenderScreenSize = new Vector2Int(Screen.width, Screen.height);


        //Render ScreenSize의 경우  --> 현재 해상도의 Screen의 width와 height를 가져옴
        // 이 현재 해상도는 바로 윗 단에서  SetRenderResolution(resolution.x, resolution.y); 을 진행한 이후의 값임

        //DeviceScreenSize 의 경우 초기에 설정한  ->  Screen.width 와 Screen.height을 가져옴
        // initialize 후에 추가적인 변경 없음 -> 기기 기본값

        //wide인 경우 y값에 맞추어 RenderScale 설정
        //아닌경우 x 값에 맞추어 RednerScale 결정 
        //이 기존의 값에 비해서 새롭게 그려진 RenderScreenSize 가 얼마만큼의 비율로 변하였는지 그 값을 [RenderScale]로 가져갑니다.
        if (IsWideScreen())
        {
            float dy = DeviceScreenSize.y;
            float ry = RenderScreenSize.y;
            RenderScale = ry / dy;
        }
        else
        {
            float dx = DeviceScreenSize.x;
            float rx = RenderScreenSize.x;
            RenderScale = rx / dx;
        }

        TestLog($"ResetRenderScreenSize>\nDevice Resolution({DeviceScreenSize})\nTarget Resolution({RenderScreenSize})\nSafeArea({SafeArea})\nRenderScale({RenderScale})");
    }

    #endregion

    #region SafeArea

    // SafeArea를 사용할 지 여부를 결정하는 속성
    private static bool UseSafeArea
    {
        get
        {
#if DISABLE_SAFE_AREA
            return false;
#elif UNITY_EDITOR && UNITY_2017_2_OR_NEWER
            return true;
#elif UNITY_IOS && UNITY_2017_2_OR_NEWER
            return true;
#elif UNITY_ANDROID && UNITY_2018_3_OR_NEWER
            return true;
#else
            return false;
#endif
        }
    }

    /// <summary>
    /// 가로 모드에서 SafeArea가 한쪽만 적용될 경우 양쪽 다 적용하기 위한 재계산
    /// </summary>
    public static Rect CutoutSafeAreaBothSide(Rect safeArea)
    {
        // 양쪽 모두 SafeArea를 적용해야 하는 설정이라면
        if (true)//JCAppDef.UI_SAFE_AREA_BOTH_SIDE_CUTOUT
        {
            //현재 safearea 넓이와, 기본 스크린의 x 값(가로 길이)가 같지 않을 경우에만 처리
            if (safeArea.width != RenderScreenSize.x)
            {
                //safeArea.position -> SafeArea가 화면에서 어디에 위치하는지 표시
                //(50,100) 이면 좌측상단 모서리에서 오른쪽으로 50픽셀, 왼쪽으로 100픽셀 떨어지는 곳에서 시작


                //게임 화면의 넓이 - safreArea의 시작 x점( 좌측 상단 부터 x 만큼 떨어짐 )
                //(safeArea가 화면 왼쪾으로 얼만큼 떨어져 있는지( 1080-20 -> 1060 값이 화면 시작점 ) 여기다가 safreArea.width를 빼서 
                //safreArea의 끝점을 확인할 수 있음 ->  현 가정이 safeArea가 양쪽 끝에 존재하는 것  -> safeArea우측 에서 얼만큼 떨어졌나
                float r = RenderScreenSize.x - safeArea.x - safeArea.width;
                float cut = Mathf.Max(safeArea.x, r);
                //cut 값에 왼쪽 , 오른쪽 두개 중 더 멀리 떨어진 곳을 선택한다.
                //이렇게 해서 safeArea를 적용하면  좌우 양쪽이 적용 되겠죠?..


                safeArea.x = cut;
                safeArea.width = RenderScreenSize.x - (cut * 2);
                //saeArea 의 시작점 을 cut 로, 총 넓이를 -> 양쪽 cut 값 뺸 싸이즈  총 화면 크기를 설정
            }
        }
        return safeArea;
    }

    public static void ResetSafeArea()
    {
        // SafeArea를 사용하지 않는 상황이라면 그냥 스크린 사이즈를 넘김
        if (!UseSafeArea)
        {
            SafeArea = new Rect(Vector2.zero, RenderScreenSize);
            return;
        }

        Rect safe = DeviceSafeArea;
        //safe를 정의해서 내 기기의 safe 에어리어를 가져옴

        //만약 RenderScale이 변경되었다면  현재 screensize에 맡는 safreArea 영역을 다시 계산한다.
        if (RenderScale != 1.0f)
            safe = new Rect(Vector2Int.RoundToInt(safe.position * RenderScale), Vector2Int.RoundToInt(safe.size * RenderScale));
        //그리고 가로 모드에서 적용할 것이라면 그에 해당하는 safe Area를 다시 설정
        safe = CutoutSafeAreaBothSide(safe);
        //최종적으로 SafeArea를 갱신 -> 해상도가 변경되었을 경우
        SafeArea = safe;

        TestLog($"Calculate device <b>SafeArea</b>. \nUnitySafeArea({Screen.safeArea}) DeviceSafeArea({DeviceSafeArea}) \nJCSafeArea({safe})\nRenderScale({RenderScale})");
    }

    // SafeArea가 활성화되어 있는지 여부를 반환
    public static bool SafeAreaEnabled
    {
        get
        {
            if (!UseSafeArea) return false;

            Rect safe = SafeArea;
            var saSize = new Vector2(safe.width, safe.height);
            return saSize != RenderScreenSize;
        }
    }

    // RectTransform에 SafeArea를 적용하는 메서드
    //만약 해상도를 변경하지 않는다면 그냥 이 ApplySafeArea만 사용하면 될듯 싶네요,.
    //해상도가 변경되면 자동적으로 모든 SafeArea를 재계산 하는 과정
    //ChangeRenderResolutionCoroutine << 여기 있네요 


    //SafeArea 필요 없는 버전 (현재 SafeArea와 기존에 저장해둔 SafeArea의 차이가 있을 경우 실행됨)
    public static void ApplySafeArea(RectTransform rectTransform)
    {
        if (CalcSafeAreaAnchor(out Vector2 minAnchor, out Vector2 maxAnchor))
        {
            //Mathf.Lerp 선형보갼 (a,b,t) 일때 a+(b-a)*t 값을 추출 
            //여기서 선형보간 하는 이유?.. 
            //min(max)Anchor -> SafeARea 범위 , (rectTransform) 현재 UI 요소앵커 
            //즉 minX 값을 무조건 적으로 minANchor 와 maxAnchor 사이에 두는건데  음... rectTransform.anchor를 가져와서 쓰는 이유는 모르겠음
            // 최솟값 + (최대-최소)*rectTransform.anchorMin.x 값인데 anchorMin.x  값에 따라 최소쪽으로 붙는지, 최대쪽으로 붙는지 결정하는데
            //의미가 있나?...
            float minX = Mathf.Lerp(minAnchor.x, maxAnchor.x, rectTransform.anchorMin.x);
            float minY = Mathf.Lerp(minAnchor.y, maxAnchor.y, rectTransform.anchorMin.y);
            float maxX = Mathf.Lerp(minAnchor.x, maxAnchor.x, rectTransform.anchorMax.x);
            float maxY = Mathf.Lerp(minAnchor.y, maxAnchor.y, rectTransform.anchorMax.y);

            rectTransform.anchorMin = new Vector2(minX, minY);
            rectTransform.anchorMax = new Vector2(maxX, maxY);
        }
    }

    // RectTransform에 SafeArea를 적용하는 메서드 (지정한 SafeArea를 기준으로)
    public static void ApplySafeArea(RectTransform rectTransform, Rect safeArea)
    {
        //SafeArea를 계산하고,
        if (CalcSafeAreaAnchor(safeArea, out Vector2 minAnchor, out Vector2 maxAnchor))
        {
            float minX = Mathf.Lerp(minAnchor.x, maxAnchor.x, rectTransform.anchorMin.x);
            float minY = Mathf.Lerp(minAnchor.y, maxAnchor.y, rectTransform.anchorMin.y);
            float maxX = Mathf.Lerp(minAnchor.x, maxAnchor.x, rectTransform.anchorMax.x);
            float maxY = Mathf.Lerp(minAnchor.y, maxAnchor.y, rectTransform.anchorMax.y);
          
            rectTransform.anchorMin = new Vector2(minX, minY);
            rectTransform.anchorMax = new Vector2(maxX, maxY);
        }
    }

    // SafeArea의 앵커 값을 계산하는 메서드
    //(그냥 사용하게 되면 변경되지 않은 디폴트 ?? 지금 기억하고 있는 SafeArea값에 따른
    //(CalcSafeAreaAnchor) 메서드를 이용해minAnchor, maxAnchor 를 계산함. 계산만 함 
    public static bool CalcSafeAreaAnchor(out Vector2 minAnchor, out Vector2 maxAnchor)
    {
        bool res = SafeAreaEnabled;
        // SafeArea가 변경된 경우에 재계산 
        // 변경되지 않은 경우 기존 값 사용 (C_RTM Anchor)
        if (_PrevCheckedSafeArea != SafeArea)
            res = CalcSafeAreaAnchor(SafeArea, out C_RTMinAnchor, out C_RTMaxAnchor);

        minAnchor = C_RTMinAnchor;
        maxAnchor = C_RTMaxAnchor;
  
        _PrevCheckedSafeArea = SafeArea;

        return res;
    }

    public static bool CalcSafeAreaAnchor(Rect safeArea, out Vector2 minAnchor, out Vector2 maxAnchor)
    {
        //SafeArea를 사용하지 않는다면 -> 기존과 똑같음
        if (!SafeAreaEnabled)
        {
            minAnchor = Vector2.zero;
            maxAnchor = Vector2.one;
            return false;
        }

        //safeArea.Position -> 왼쪽 상단  (Rect 가 이럼)
        //최소 및 최대 앵커자리 확인
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        // 각 앵커들의 값을 (0,1) 사이 값으로 변환하는데 ( RenderScreenSzie를 통해  그려지는 화면 비율을 확인 가능)
        // UI가 시작하는 지점 -> minAnchor,x/RenderScreenSize.x 가 0.1 인 경우 safe area가 가로크기의 0.1인 지점 시작
        // maxAnchor.x = Mathf.Clamp01(maxAnchor.x / RenderScreenSize.x); 0.9 이면 safe area가 끝나는 지점이 0.9
        // 비율로 생각해서 safearea 가 그려지는 시작점을 ancher로 ㅇㅇ 
        minAnchor.x = Mathf.Clamp01(minAnchor.x / RenderScreenSize.x);
        minAnchor.y = Mathf.Clamp01(minAnchor.y / RenderScreenSize.y);
        maxAnchor.x = Mathf.Clamp01(maxAnchor.x / RenderScreenSize.x);
        maxAnchor.y = Mathf.Clamp01(maxAnchor.y / RenderScreenSize.y);

        TestLog($"MinAnchor{minAnchor} MaxAnchor{maxAnchor}\nSafeArea{SafeArea}");

        return true;
    }

    #endregion
//#if !UNITY_EDITOR
//    // RectTransform를 Rect 값으로 변환 
//    public static Rect RectTransformToScreenSpace(RectTransform transform)
//    {
//        Vector2 pos, size;
//        if (JCUIScene.Current._MainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
//        {
//            pos = (Vector2)transform.position;
//            //Vector2.Scale은 두개의 Vector값을 곱한 결과 -> (현재 자신의 크기(width,height) * localScale(지역)을 곱합 -> 실제 크기?..
//            size = Vector2.Scale(transform.rect.size, transform.lossyScale);
//        }
//        else
//        {
//            //월드좌표 -> 스크린 좌표
//            pos = JCUIScene.Current.CanvasCamera.WorldToScreenPoint(transform.position);
//            float scale = 1f;
//            if (JCUIScene.Current.TryGetComponent<CanvasScaler>(out var scaler))
//            {
//                Vector2 uiRef = scaler.referenceResolution;
//                if (IsWideScreen())
//                    scale = RenderScreenSize.y / uiRef.y;
//                else
//                    scale = RenderScreenSize.x / uiRef.x;
//            }
//            size = transform.rect.size * scale; //비율을 구해서 그 비율에 맞게 size를 설정
//        }
//        //중심점 에서 size의 반만큼 이동 -> 왼쪽 상단 
//        //즉 transform 위치(x,y)를 중심으로 해서, 어디가 시작점이고 그 크기 는 어떻게 되는지 나옴..
//        //시작점 : (x,y)= ( pos- size*0.5f)      ( x 부터 x+size.x 까지 가로 y부터 y+size.y 까지 세로?)
//        return new Rect(pos - (size * 0.5f), size);
//    }
//#endif
    // 두 개의 해상도를 비교하여 정렬 순서를 반환하는 메서드
    public static int CompareResolution(Vector2Int a, Vector2Int b)
    {
        return CompareResolution(a.x, a.y, b.x, b.y);
    }

    public static int CompareResolution(int aWidth, int aHeight, int bWidth, int bHeight)
    {
        if (IsWideScreen())
            return aHeight.CompareTo(bHeight);
        else
            return aWidth.CompareTo(bWidth);
    }
}
