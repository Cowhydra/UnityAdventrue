using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public struct Speaker
{
	public Image characterimage;       // ĳ���� �̹��� (û��/ȭ�� ���İ� ����)
	public Image imageDialog;       // ��ȭâ Image UI
	public TextMeshProUGUI textName;            // ���� ������� ĳ���� �̸� ��� Text UI
	public TextMeshProUGUI textDialogue;        // ���� ��� ��� Text UI
	public GameObject objectArrow;      // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� ������Ʈ
}
public struct DialogData
{
	public int speakerIndex;    // �̸��� ��縦 ����� ���� DialogSystem�� speakers �迭 ����
	public string name;         // ĳ���� �̸�
	[TextArea(3, 5)]
	public string dialogue;     // ���
}


//�ӽ� DialogSystem
//���� ü�������� �Ѵٸ� excel, csv ���� ���� ���� �ش� ���� �ҷ������� �ؾ���

public class DialogSystem : UI_Popup
{

	//��ȭ ���� �⺻ ����
	private Speaker[] speakers;                 // ��ȭ�� �����ϴ� ĳ���͵��� UI �迭
	private List<DialogData> dialogs;   // ���� �б��� ��� ��� �迭
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
	private bool isAutoStart = true;            // �ڵ� ���� ����
	private bool isFirst = true;                // ���� 1ȸ�� ȣ���ϱ� ���� ����
	private int currentDialogIndex = -1;    // ���� ��� ����
	private int currentSpeakerIndex = 0;    // ���� ���� �ϴ� ȭ��(Speaker)�� speakers �迭 ����
	private float typingSpeed = 0.1f;           // �ؽ�Ʈ Ÿ���� ȿ���� ��� �ӵ�
	private bool isTypingEffect = false;        // �ؽ�Ʈ Ÿ���� ȿ���� ���������
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
		
		speakers[nthspeaker].characterimage = speakerImage;       // ĳ���� �̹��� (û��/ȭ�� ���İ� ����)
		speakers[nthspeaker].imageDialog= dialogimage;      // ��ȭâ Image UI
		speakers[nthspeaker].textName = speakername;           // ���� ������� ĳ���� �̸� ��� Text UI
		speakers[nthspeaker].textDialogue = Dialog;   // ���� ��� ��� Text UI
		speakers[nthspeaker].objectArrow = Arrow;      // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� ������Ʈ
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
				Debug.Log("����");
				isButtonClicked = true;
				Managers.UI.ClosePopupUI();
				break;
            case Define.Npc_Type.QuestNpc:
				Debug.Log("����Ʈâ �����ֱ�");
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
				Debug.Log("��ȭâ �����ֱ�");
				isButtonClicked = true;
				Managers.UI.ClosePopupUI();
				break;
            case Define.Npc_Type.TuotorialNpc:
				Debug.Log("�����ٰ� ����");
				isButtonClicked = true;
				Managers.UI.ClosePopupUI();
				break;
            case Define.Npc_Type.Boss:
				Debug.Log("�����ٰ� ����");
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
				SetDiaLog(1, "������", "�������!!");
				SetDiaLog(1, "������", "������ �ɰ��� ����� �������ϴ�..");
				SetDiaLog(1, "������", "������ �ּ����� ���ڽ��ϴ�...");
				break;
            case Define.Npc_Type.ShopNpc:
				dialogs.Clear();
				SetDiaLog(1, "��������", "�������!!!");
				SetDiaLog(1, "��������", "���Ͻô� ��ǰ�� �����ôٸ� �������ּ���...");

				break;
            case Define.Npc_Type.EnhanceNpc:
				dialogs.Clear();
				SetDiaLog(1, "��ȭâ ����", "�������!!");
				SetDiaLog(1, "��ȭâ ����", "��ȭ�� �Ͻðڽ��ϱ�??");

				break;
            case Define.Npc_Type.TuotorialNpc:
				dialogs.Clear();
				SetDiaLog(1, "???", "�������!!");
				SetDiaLog(1, "???", "�� ���踦 �����ּ���!!!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "...........");
				SetDiaLog(1, "???", "�� ���踦 �����ּ���!!!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "................");
				SetDiaLog(1, "???", ".................................;;");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "��..");
				SetDiaLog(1, "???", "�켱 ������ ã�� �ֹε��� �����ּ���");
				break;
			case Define.Npc_Type.Boss:
				dialogs.Clear();
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "�ɻ�ġ ���� �����Ⱑ ������!!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "��� ���� ���� �ִ� �� ����!!");
				SetDiaLog(1, $"{Managers.Game.CharacterName}", "ũ�ƾƾƾƾ�!!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "���� �����ؿ´�!! ");
	
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
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "���Ⱑ �����ΰ�?? ");
				SetDiaLog(1, $"{Managers.Game.CharacterName}", "�׷��� ����!");
				SetDiaLog(1, $"{Managers.Game.CharacterName}", "� ������ ����ġ�� ���� ��� ��������!");
				SetDiaLog(0, $"{Managers.Game.CharacterName}", "�׷�!! ");
				break;
        }
        GetButton((int)Buttons.AcceptionButton).gameObject
	       .BindEvent((PointerEventData data) => ShowNextUI());
		GetButton((int)Buttons.CancelButton).gameObject
			.BindEvent((PointerEventData data) => Managers.UI.ClosePopupUI());
    }
    private void Setup()
	{
		// ��� ��ȭ ���� ���ӿ�����Ʈ ��Ȱ��ȭ
		for (int i = 0; i < speakers.Length; ++i)
		{
			SetActiveObjects(speakers[i], false);
			// ĳ���� �̹����� ���̵��� ����
			speakers[i].characterimage.gameObject.SetActive(true);
		}
	}

	public bool UpdateDialog()
	{
		// ��� �бⰡ ���۵� �� 1ȸ�� ȣ��
		if (isFirst == true)
		{
			// �ʱ�ȭ. ĳ���� �̹����� Ȱ��ȭ�ϰ�, ��� ���� UI�� ��� ��Ȱ��ȭ
			Setup();

			// �ڵ� ���(isAutoStart=true)���� �����Ǿ� ������ ù ��° ��� ���
			if (isAutoStart) SetNextDialog();

			isFirst = false;
		}

		if (Input.GetMouseButtonDown(0))
		{
			// �ؽ�Ʈ Ÿ���� ȿ���� ������϶� ���콺 ���� Ŭ���ϸ� Ÿ���� ȿ�� ����
			if (isTypingEffect == true)
			{
				isTypingEffect = false;

				// Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�
				StopCoroutine("OnTypingText");
				speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
				// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
				speakers[currentSpeakerIndex].objectArrow.SetActive(true);

				return false;
			}

			// ��簡 �������� ��� ���� ��� ����
			if (dialogs.Count > currentDialogIndex + 1)
			{
				SetNextDialog();
			}
			// ��簡 �� �̻� ���� ��� ��� ������Ʈ�� ��Ȱ��ȭ�ϰ� true ��ȯ
			else
			{
				//��Ȱ��ȭ

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
			// SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
			speakers[i].characterimage.gameObject.SetActive(false);
		}
	}
	private void SetNextDialog()
	{
		// ���� ȭ���� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], false);

		// ���� ��縦 �����ϵ��� 
		currentDialogIndex++;

		// ���� ȭ�� ���� ����
		currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;

		// ���� ȭ���� ��ȭ ���� ������Ʈ Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], true);
		// ���� ȭ�� �̸� �ؽ�Ʈ ����
		speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
		// ���� ȭ���� ��� �ؽ�Ʈ ����
		//speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
		StartCoroutine("OnTypingText");
	}

	private void SetActiveObjects(Speaker speaker, bool visible)
	{
		speaker.imageDialog.gameObject.SetActive(visible);
		speaker.textName.gameObject.SetActive(visible);
		speaker.textDialogue.gameObject.SetActive(visible);

		// ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ�ϱ� ������ �׻� false
		speaker.objectArrow.SetActive(false);

		// ĳ���� ���� �� ����
		Color color = speaker.characterimage.color;
		color.a = visible == true ? 1 : 0.2f;
		speaker.characterimage.color = color;
	}

	private IEnumerator OnTypingText()
	{
		int index = 0;

		isTypingEffect = true;

		// �ؽ�Ʈ�� �ѱ��ھ� Ÿ����ġ�� ���
		while (index < dialogs[currentDialogIndex].dialogue.Length)
		{
			speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue.Substring(0, index);

			index++;

			yield return new WaitForSeconds(typingSpeed);
		}

		isTypingEffect = false;

		// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
		speakers[currentSpeakerIndex].objectArrow.SetActive(true);
	}
	IEnumerator ChatingStart()
    {
		yield return new WaitUntil(()=>UpdateDialog());
    }


}


