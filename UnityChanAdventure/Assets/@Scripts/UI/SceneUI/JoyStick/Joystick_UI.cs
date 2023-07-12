using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick_UI : UI_Scene,IListener
{
    //IDEA ��ư ���� �ϴ� ������ �� ����
    //new Input System�� Ȱ���ϰ� �;����� ���� �ʾ���..
    //�ᱹ PC, ����ϸ� �Ҳ��� ȯ�濡���� �ٸ��� ���ָ� �ɵ�?..

    private GameObject _handler;
    private Vector2 _moveDir;
    private float _joystickRadius;
    private Vector2 _joystickOriginalPos;
    private Vector2 _joystickTouchPos;

    private float dragStartPosition;

    private GameObject PlayerMain_Cm;
    private PlayerController _playerController;

    [Header("Camera Setting")]
    [SerializeField]
    private GameObject _AroundTarget;
    [SerializeField]  private float lookRotateSpeed = 0.1f;
    #region UI Bind
    enum GameObjects
    {
        Move_Handle,
        Look_Area,
        Move_Area,
        PlayerInput_Area,
    }
    enum Buttons
    {
        Jump_Button,
        Sprint_Button,
        Attack_Button,
        Auto_Button,
        SkillQ_Button,
        SkillW_Button,
        SkillE_Button,
        SkillR_Button,

    }
    #endregion
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        GetComponent<Canvas>().sortingOrder = (int)Define.SortingOrder.JoyStick;
        _playerController = transform.root.GetComponentInChildren<PlayerController>();
        if(_AroundTarget == null)
        {
            _AroundTarget = GameObject.FindGameObjectWithTag("AroundTarget");
        }

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        InitButton();
        _handler = GetObject((int)GameObjects.Move_Handle);
        _joystickTouchPos = _handler.transform.position;
        _joystickRadius = GetObject((int)GameObjects.Move_Area).GetComponent<RectTransform>().sizeDelta.y / 5;

        _joystickOriginalPos = GetObject((int)GameObjects.Move_Area).transform.position;
        PlayerMain_Cm = GameObject.Find("PlayerMain_Cm");

        //���� ���� ����, ��ȭâ ���� ��  UI�� �̺�Ʈ�� ���� �� joyStick �� ȭ���� ������ ���� ����
        #region Event
        _handler.BindEvent((PointerEventData data) => OnDragEvent_JoyStick(data), Define.UIEvent.OnDrag);
        _handler.BindEvent((PointerEventData data) => EndDragEvent_JoyStick(), Define.UIEvent.OnEndDrag);
      
        GetObject((int)GameObjects.Look_Area).BindEvent((PointerEventData data) => DragEvent_Look(data),Define.UIEvent.OnDrag);
        GetObject((int)GameObjects.Look_Area).BindEvent((PointerEventData data) => StartDrag(data), Define.UIEvent.OnBeginDrag);

        Managers.Event.AddListener(Define.EVENT_TYPE.InventoryOpen, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.InventoryClose, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.ShopClose, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.ShopOpen, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.DialogOpen, this);
        Managers.Event.AddListener(Define.EVENT_TYPE.DialogClose, this);
        #endregion


    }

    
    public void StartDrag(PointerEventData data)
    {
        dragStartPosition = data.position.x;
    }
    private void DragEvent_Look(PointerEventData data)
    {
        #region ī�޶� �Ÿ����� ������ ���ٰ� �ٸ� ������� ��ȸ
        //float dragDeltaX = data.position.x - dragStartPosition;
        //float rotationAmount = dragDeltaX * lokRotateSpeed;

        //float newX = _CameraOriginPos.x + _CameraDistance * Mathf.Sin(rotationAmount * Mathf.Deg2Rad);
        //float newZ = _CameraOriginPos.z + _CameraDistance * Mathf.Cos(rotationAmount * Mathf.Deg2Rad);

        //Vector3 newPosition = new Vector3(newX, _CameraOriginPos.y, newZ);
        //_CameraTarget.transform.localPosition = newPosition;
        #endregion

        //���� �� �Ʒ� �̵��� �����ϰ�, �翷���� �̵��� ���� 

        float dragDeltaX = data.position.x - dragStartPosition;
        float rotationAmount = dragDeltaX * lookRotateSpeed;
        Debug.Log("ī�޶� �巡�� �̺�Ʈ ������~");
        Vector3 currentRotation = _AroundTarget.transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + rotationAmount, currentRotation.z);
        _AroundTarget.transform.rotation = Quaternion.Euler(newRotation);
        
    }

    //���̽�ƽ �巡�װ� ������ �̵� �̺�Ʈ �߻� ���Ѽ� -> ������ ���� ���� + joystick�� ��ġ�� ���� ��ġ�� ������
    private void EndDragEvent_JoyStick()
    {
        _moveDir = Vector2.zero;
        _handler.transform.position = _joystickOriginalPos;
        Managers.Event.MoveInputAction?.Invoke(_moveDir);
    }

    //��ư�� �̺�Ʈ �Ҵ� 
    private void InitButton()
    {
        GetButton((int)Buttons.Jump_Button).gameObject.BindEvent((PointerEventData data) =>Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Jump));
        GetButton((int)Buttons.Sprint_Button).gameObject.BindEvent((PointerEventData data)=>Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Sprint));
        GetButton((int)Buttons.Attack_Button).gameObject.BindEvent((PointerEventData data)=>Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Attack));
        GetButton((int)Buttons.Auto_Button).gameObject.BindEvent((PointerEventData data) =>Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Auto));
        GetButton((int)Buttons.SkillQ_Button).gameObject.BindEvent((PointerEventData data)=>OnSKillEvent(Define.SkillType.QSkill));
        GetButton((int)Buttons.SkillW_Button).gameObject.BindEvent((PointerEventData data)=>OnSKillEvent(Define.SkillType.WSkill));
        GetButton((int)Buttons.SkillE_Button).gameObject.BindEvent((PointerEventData data)=> OnSKillEvent(Define.SkillType.ESkill));
        GetButton((int)Buttons.SkillR_Button).gameObject.BindEvent((PointerEventData data) => OnSKillEvent(Define.SkillType.RSkill));


        GetButton((int)Buttons.SkillQ_Button).gameObject.BindEvent((PointerEventData data) => OnDropEvent_SKill(data, Define.SkillType.QSkill), Define.UIEvent.OnDrop);
        GetButton((int)Buttons.SkillW_Button).gameObject.BindEvent((PointerEventData data) =>OnDropEvent_SKill(data, Define.SkillType.WSkill), Define.UIEvent.OnDrop);
        GetButton((int)Buttons.SkillE_Button).gameObject.BindEvent((PointerEventData data) =>OnDropEvent_SKill(data, Define.SkillType.ESkill), Define.UIEvent.OnDrop);
        GetButton((int)Buttons.SkillR_Button).gameObject.BindEvent((PointerEventData data) => OnDropEvent_SKill(data, Define.SkillType.RSkill), Define.UIEvent.OnDrop);
    }
    //��ų ��ư�� ������ �̺�Ʈ�� �߼��ϰ�
    //���� �̺�Ʈ���� ( �ߵ����� �����ϴ��� Ȯ�� �� �ߵ� ������ �����̸� Animation �̺�Ʈ�� �ߵ��ǵ��� 
    // �߰� �̺�Ʈ �ߵ��ϵ��� ���� 
    private void OnSKillEvent(Define.SkillType skilltype)
    {
        switch (skilltype)
        {
            case Define.SkillType.QSkill:

                Managers.Event.SkillInputAction?.Invoke(Define.SkillType.QSkill);
                break;
            case Define.SkillType.WSkill:
                Managers.Event.SkillInputAction?.Invoke(Define.SkillType.WSkill);
                break;
            case Define.SkillType.ESkill:
                Managers.Event.SkillInputAction?.Invoke(Define.SkillType.ESkill);
                break;
            case Define.SkillType.RSkill:
                Managers.Event.SkillInputAction?.Invoke(Define.SkillType.RSkill);
                break;
        }
    }


    //��ų�Ͽ��� ��ų�� ��ư�� ����� ������ ��ų ���� 
    private void OnDropEvent_SKill(PointerEventData data,Define.SkillType skilltype)
    {
        if (data.pointerDrag == null) return;
        if(data.pointerDrag.TryGetComponent(out SkillBook_Skill skill))
        {
            switch (skilltype)
            {
                case Define.SkillType.QSkill:
                    GetButton((int)Buttons.SkillQ_Button).gameObject.GetComponent<SkillButton>().ButtonSkillcode = skill.SKillCode;
                    break;                                                                                         
                case Define.SkillType.WSkill:                                                                      
                    GetButton((int)Buttons.SkillW_Button).gameObject.GetComponent<SkillButton>().ButtonSkillcode = skill.SKillCode;
                    break;                                                                                         
                case Define.SkillType.ESkill:                                                                      
                    GetButton((int)Buttons.SkillE_Button).gameObject.GetComponent<SkillButton>().ButtonSkillcode = skill.SKillCode;
                    break;                                                                                        
                case Define.SkillType.RSkill:                                                                     
                    GetButton((int)Buttons.SkillR_Button).gameObject.GetComponent<SkillButton>().ButtonSkillcode = skill.SKillCode;
                    break;
            }

        }

    }
    private void OnDragEvent_JoyStick(PointerEventData data)
    {
     
        //�ᱹ ���̽�ƽ �巡�׸� ���ؼ� �ϴ� ���� ���⸸ ����ִ� �� 
        Vector2 dragPos = data.position;
        _moveDir = (dragPos - _joystickOriginalPos).normalized;
        Managers.Event.MoveInputAction?.Invoke(_moveDir);

        //�� �Ʒ� �κ� �ڵ�� ���̽�ƽ �ڵ��� �����̰� �ϴ� �� 
        float joystickDist = Vector2.Distance(dragPos, _joystickOriginalPos);

        Vector3 newPos;
        if (joystickDist < _joystickRadius)
        {
            newPos = dragPos;
        }
        else
        {
            newPos = _joystickOriginalPos + _moveDir * _joystickRadius;
        }
        _handler.transform.position = newPos;
    }

    //�ߵ� �Ǵ� �̺�Ʈ�� ���� ó�� (SceneMachine ī�޶� ó�� + ���̾Ʒα�, ���� ���� �� �ʿ� �̺�Ʈ ��
    //���̽�ƽ UI�� ��Ȱ�� �ʿ�
    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case Define.EVENT_TYPE.InventoryOpen:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(false);
                if (Managers.Scene.CurrentScene.SceneType == Define.Scene.TownScene)
                {
                    PlayerMain_Cm.SetActive(false);
                }
                Managers.Event.MoveInputAction?.Invoke(Vector2.zero);
                break;
            case Define.EVENT_TYPE.InventoryClose:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(true);
                if (Managers.Scene.CurrentScene.SceneType == Define.Scene.TownScene)
                {
                    PlayerMain_Cm.SetActive(true);
                }
                Managers.Event.MoveInputAction?.Invoke(Vector2.zero);

                break;
            case Define.EVENT_TYPE.ShopClose:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(true);
                PlayerMain_Cm.SetActive(true);
                Managers.Event.MoveInputAction?.Invoke(Vector2.zero);

                break;
            case Define.EVENT_TYPE.ShopOpen:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(false);
                PlayerMain_Cm.SetActive(false);
                Managers.Event.MoveInputAction?.Invoke(Vector2.zero);

                break;
            case Define.EVENT_TYPE.DialogOpen:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(false);
                PlayerMain_Cm.SetActive(false);
                Managers.Event.MoveInputAction?.Invoke(Vector2.zero);

                break;
            case Define.EVENT_TYPE.DialogClose:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(true);
                PlayerMain_Cm.SetActive(true);
                Managers.Event.MoveInputAction?.Invoke(Vector2.zero);

                break;

        }
    }
}
