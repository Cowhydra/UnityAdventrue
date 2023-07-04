using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public struct Speaker
{
	public Image characterimage;       // 캐릭터 이미지 (청자/화자 알파값 제어)
	public Image imageDialog;       // 대화창 Image UI
	public TextMeshProUGUI textName;            // 현재 대사중인 캐릭터 이름 출력 Text UI
	public TextMeshProUGUI textDialogue;        // 현재 대사 출력 Text UI
	public GameObject objectArrow;      // 대사가 완료되었을 때 출력되는 커서 오브젝트
}
public struct DialogData
{
	public int speakerIndex;    // 이름과 대사를 출력할 현재 DialogSystem의 speakers 배열 순번
	public string name;         // 캐릭터 이름
	[TextArea(3, 5)]
	public string dialogue;     // 대사
}


//임시 DialogSystem
//추후 체계적으로 한다면 excel, csv 파일 등을 통해 해당 값을 불러오도록 해야함

public class DialogSystem : UI_Popup
{

	//대화 관련 기본 설정
	private Speaker[] speakers;                 // 대화에 참여하는 캐릭터들의 UI 배열
	private List<DialogData> dialogs;   // 현재 분기의 대사 목록 배열
	[SerializeField]
	Define.Npc_Type _TalkType;
	public Define.Npc_Type TalkType
    {
        get
        {
			return _TalkType;
        }
        set
        {
			_TalkType = value;
			Init();
			Managers.Event.PostNotification(Define.EVENT_TYPE.DialogOpen, this);
			StartCoroutine(nameof(ChatingStart));
			//setting
		}
    }


	[SerializeField]
	private bool isAutoStart = true;            // 자동 시작 여부
	private bool isFirst = true;                // 최초 1회만 호출하기 위한 변수
	private int currentDialogIndex = -1;    // 현재 대사 순번
	private int currentSpeakerIndex = 0;    // 현재 말을 하는 화자(Speaker)의 speakers 배열 순번
	private float typingSpeed = 0.1f;           // 텍스트 타이핑 효과의 재생 속도
	private bool isTypingEffect = false;        // 텍스트 타이핑 효과를 재생중인지
	private bool isButtonClicked = false;
    #region UIBind
    enum Texts
    {
		PlayerName_Text,
		PlayerContents_Text,

		NpcName_Text,
		NpcContents_Text,

	}
	enum Images
    {
		Player,
		Npc,
		NpcDialog,
		PlayerDialog,
	}
	enum GameObjects
    {
		PlayerDilogArrow,
		NpcDilogArrow,
	}
	enum Buttons
    {
		CancelButton,
		AcceptionButton,
	}
    #endregion
    private void SetSpeaker(int nthspeaker,Image speakerImage,Image dialogimage,TextMeshProUGUI speakername,TextMeshProUGUI Dialog,GameObject Arrow)
    {
		
		speakers[nthspeaker].characterimage = speakerImage;       // 캐릭터 이미지 (청자/화자 알파값 제어)
		speakers[nthspeaker].imageDialog= dialogimage;      // 대화창 Image UI
		speakers[nthspeaker].textName = speakername;           // 현재 대사중인 캐릭터 이름 출력 Text UI
		speakers[nthspeaker].textDialogue = Dialog;   // 현재 대사 출력 Text UI
		speakers[nthspeaker].objectArrow = Arrow;      // 대사가 완료되었을 때 출력되는 커서 오브젝트
    }
	private void SetDiaLog(int nthspeaker,string name,string dialog)
	{   
		DialogData data= new DialogData();
		data.speakerIndex = nthspeaker;
		data.name = name;
		data.dialogue = dialog;
		dialogs.Add(data);
	}
    public override void Init()
    {
        base.Init();

		GetComponent<Canvas>().sortingOrder = (int)Define.SortingOrder.DialogSystem;
		isButtonClicked = false;


	    speakers = new Speaker[2];
		dialogs = new List<DialogData>();


	    Bind<TextMeshProUGUI>(typeof(Texts));
		Bind<Image>(typeof(Images));
		Bind<GameObject>(typeof(GameObjects));
		Bind<Button>(typeof(Buttons));

		SetSpeakerAndData();
		Setup();


		GetButton((int)Buttons.AcceptionButton).gameObject.SetActive(false);
		GetButton((int)Buttons.CancelButton).gameObject.SetActive(false);

	}
	private void ShowNextUI()
    {
        switch (TalkType)
        {
            case Define.Npc_Type.None:
				Debug.Log("버그");
				isButtonClicked = true;
				Managers.UI.ClosePopupUI();
				break;
            case Define.Npc_Type.QuestNpc:
				Debug.Log("퀘스트창 보여주기");
				isButtonClicked = true;
				Managers.UI.ClosePopupUI();
				Managers.UI.ShowPopupUI<QuestUI>();
				break;
            case Define.Npc_Type.ShopNpc:
				isButtonClicked = true;
				Managers.UI.ClosePopupUI();
				Managers.Event.PostNotification(Define.EVENT_TYPE.ShopOpen, this);
				break;
            case Define.Npc_Type.EnhanceNpc:
				Debug.Log("강화창 보여주기");
				isButtonClicked = true;
				Managers.UI.ClosePopupUI();
				break;
            case Define.Npc_Type.TuotorialNpc:
				Debug.Log("보여줄게 없음");
				isButtonClicked = true;
				Managers.UI.ClosePopupUI();
				break;
            case Define.Npc_Type.Boss:
				Debug.Log("보여줄게 없음");
				isButtonClicked = true;
				Managers.UI.ClosePopupUI();
				break;
        }
    }
	private void ShowLastDiagonalButton()
    {
        switch (TalkType)
        {
            case Define.Npc_Type.None:

                break;
            case Define.Npc_Type.QuestNpc:
				GetButton((int)Buttons.AcceptionButton).gameObject.SetActive(true);
				GetButton((int)Buttons.CancelButton).gameObject.SetActive(true);
				break;
            case Define.Npc_Type.ShopNpc:
				GetButton((int)Buttons.AcceptionButton).gameObject.SetActive(true);
				GetButton((int)Buttons.CancelButton).gameObject.SetActive(true);
				break;
            case Define.Npc_Type.EnhanceNpc:
				GetButton((int)Buttons.AcceptionButton).gameObject.SetActive(true);
				GetButton((int)Buttons.CancelButton).gameObject.SetActive(true);
				break;
            case Define.Npc_Type.TuotorialNpc:
				GetButton((int)Buttons.AcceptionButton).gameObject.SetActive(true);
				GetButton((int)Buttons.CancelButton).gameObject.SetActive(false);
				break;
			case Define.Npc_Type.Boss:
				GetImage((int)Images.NpcDialog).gameObject
					.BindEvent((PointerEventData data) => ShowNextUI());
				GetImage((int)Images.PlayerDialog).gameObject
					.BindEvent((PointerEventData data) => ShowNextUI());
				break;
			case Define.Npc_Type.Dungeon:
				GetImage((int)Images.NpcDialog).gameObject
					.BindEvent((PointerEventData data) => ShowNextUI());
				GetImage((int)Images.PlayerDialog).gameObject
					.BindEvent((PointerEventData data) => ShowNextUI());
				break;
        }
    }
    private void SetSpeakerAndData()
    {
		SetSpeaker(0, GetImage((int)Images.Player), GetImage((int)Images.PlayerDialog), GetText((int)Texts.PlayerName_Text), GetText((int)Texts.PlayerContents_Text), GetObject((int)GameObjects.PlayerDilogArrow));
		SetSpeaker(1, GetImage((int)Images.Npc), GetImage((int)Images.NpcDialog), GetText((int)Texts.NpcName_Text), GetText((int)Texts.NpcContents_Text), GetObject((int)GameObjects.NpcDilogArrow));
        switch (TalkType)
        {
            case Define.Npc_Type.None:
                break;
            case Define.Npc_Type.QuestNpc:
				dialogs.Clear();
				SetDiaLog(1, "도와줘", "어서오세요!!");
				SetDiaLog(1, "도와줘", "마을이 심각한 어려움에 빠졌습니다..");
				SetDiaLog(1, "도와줘", "도움을 주셨으면 좋겠습니다...");
				break;
            case Define.Npc_Type.ShopNpc:
				dialogs.Clear();
				SetDiaLog(1, "상점주인", "어서오세요!!!");
				SetDiaLog(1, "상점주인", "원하시는 상품이 있으시다면 말씀해주세요...");

				break;
            case Define.Npc_Type.EnhanceNpc:
				dialogs.Clear();
				SetDiaLog(1, "강화창 주인", "어서오세요!!");
				SetDiaLog(1, "강화창 주인", "강화를 하시겠습니까??");

				break;
            case Define.Npc_Type.TuotorialNpc:
				dialogs.Clear();
				SetDiaLog(1, "???", "어서오세요!!");
				SetDiaLog(1, "???", "이 세계를 구해주세요!!!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "...........");
				SetDiaLog(1, "???", "이 세계를 구해주세요!!!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "................");
				SetDiaLog(1, "???", ".................................;;");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "네..");
				SetDiaLog(1, "???", "우선 마을을 찾아 주민들을 도와주세요");
				break;
			case Define.Npc_Type.Boss:
				dialogs.Clear();
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "심상치 않은 분위기가 느껴져!!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "잠깐 저기 뭐가 있는 것 같아!!");
				SetDiaLog(1, $"{Managers.Game.CharacterName}", "크아아아아앙!!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "적이 공격해온다!! ");
	
				switch ((Define.Scene)Enum.Parse(typeof(Define.Scene), Managers.Scene.CurrentScene.gameObject.name))
                {
                    case Define.Scene.LavaScene:
						GetImage((int)Images.Npc).sprite = Managers.Resource.Load<Sprite>("LavaBoss");
					    break;
                    case Define.Scene.DesertScene:
						GetImage((int)Images.Npc).sprite = Managers.Resource.Load<Sprite>("DesertBoss");
						break;
                    case Define.Scene.WaterScene:
						GetImage((int)Images.Npc).sprite = Managers.Resource.Load<Sprite>("WaterBoss");
						break;
                    case Define.Scene.FightScene:
						break;
                }
                break;
			case Define.Npc_Type.Dungeon:
				dialogs.Clear();
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "여기가 던전인가?? ");
				SetDiaLog(1, $"{Managers.Game.CharacterName}", "그런거 같아!");
				SetDiaLog(1, $"{Managers.Game.CharacterName}", "어서 적들을 물리치고 좋은 장비를 습득하자!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "그래!! ");
				break;
        }
        GetButton((int)Buttons.AcceptionButton).gameObject
	       .BindEvent((PointerEventData data) => ShowNextUI());
		GetButton((int)Buttons.CancelButton).gameObject
			.BindEvent((PointerEventData data) => Managers.UI.ClosePopupUI());
    }
    private void Setup()
	{
		// 모든 대화 관련 게임오브젝트 비활성화
		for (int i = 0; i < speakers.Length; ++i)
		{
			SetActiveObjects(speakers[i], false);
			// 캐릭터 이미지는 보이도록 설정
			speakers[i].characterimage.gameObject.SetActive(true);
		}
	}

	public bool UpdateDialog()
	{
		// 대사 분기가 시작될 때 1회만 호출
		if (isFirst == true)
		{
			// 초기화. 캐릭터 이미지는 활성화하고, 대사 관련 UI는 모두 비활성화
			Setup();

			// 자동 재생(isAutoStart=true)으로 설정되어 있으면 첫 번째 대사 재생
			if (isAutoStart) SetNextDialog();

			isFirst = false;
		}

		if (Input.GetMouseButtonDown(0))
		{
			// 텍스트 타이핑 효과를 재생중일때 마우스 왼쪽 클릭하면 타이핑 효과 종료
			if (isTypingEffect == true)
			{
				isTypingEffect = false;

				// 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
				StopCoroutine("OnTypingText");
				speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
				// 대사가 완료되었을 때 출력되는 커서 활성화
				speakers[currentSpeakerIndex].objectArrow.SetActive(true);

				return false;
			}

			// 대사가 남아있을 경우 다음 대사 진행
			if (dialogs.Count > currentDialogIndex + 1)
			{
				SetNextDialog();
			}
			// 대사가 더 이상 없을 경우 모든 오브젝트를 비활성화하고 true 반환
			else
			{
				//비활성화

				ShowLastDiagonalButton();

				if (isButtonClicked)
                {
					Managers.Event.PostNotification(Define.EVENT_TYPE.DialogClose, this);
					return true;
				}
				return false;
			}
		}

		return false;
	}
    private void OnDestroy()
    {
		Managers.Event.PostNotification(Define.EVENT_TYPE.DialogClose, this);
		StopAllCoroutines();
	}
    private void SetOffDialog()
    {
		for (int i = 0; i < speakers.Length; ++i)
		{
			SetActiveObjects(speakers[i], false);
			// SetActiveObjects()에 캐릭터 이미지를 보이지 않게 하는 부분이 없기 때문에 별도로 호출
			speakers[i].characterimage.gameObject.SetActive(false);
		}
	}
	private void SetNextDialog()
	{
		// 이전 화자의 대화 관련 오브젝트 비활성화
		SetActiveObjects(speakers[currentSpeakerIndex], false);

		// 다음 대사를 진행하도록 
		currentDialogIndex++;

		// 현재 화자 순번 설정
		currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;

		// 현재 화자의 대화 관련 오브젝트 활성화
		SetActiveObjects(speakers[currentSpeakerIndex], true);
		// 현재 화자 이름 텍스트 설정
		speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
		// 현재 화자의 대사 텍스트 설정
		//speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
		StartCoroutine("OnTypingText");
	}

	private void SetActiveObjects(Speaker speaker, bool visible)
	{
		speaker.imageDialog.gameObject.SetActive(visible);
		speaker.textName.gameObject.SetActive(visible);
		speaker.textDialogue.gameObject.SetActive(visible);

		// 화살표는 대사가 종료되었을 때만 활성화하기 때문에 항상 false
		speaker.objectArrow.SetActive(false);

		// 캐릭터 알파 값 변경
		Color color = speaker.characterimage.color;
		color.a = visible == true ? 1 : 0.2f;
		speaker.characterimage.color = color;
	}

	private IEnumerator OnTypingText()
	{
		int index = 0;

		isTypingEffect = true;

		// 텍스트를 한글자씩 타이핑치듯 재생
		while (index < dialogs[currentDialogIndex].dialogue.Length)
		{
			speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue.Substring(0, index);

			index++;

			yield return new WaitForSeconds(typingSpeed);
		}

		isTypingEffect = false;

		// 대사가 완료되었을 때 출력되는 커서 활성화
		speakers[currentSpeakerIndex].objectArrow.SetActive(true);
	}
	IEnumerator ChatingStart()
    {
		yield return new WaitUntil(()=>UpdateDialog());
    }


}


