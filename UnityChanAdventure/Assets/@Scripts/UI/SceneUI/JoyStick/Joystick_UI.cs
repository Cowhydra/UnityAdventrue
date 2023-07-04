using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick_UI : UI_Scene,IListener
{
    private GameObject _handler;
    private Vector2 _moveDir;
    private float _joystickRadius;
    private Vector2 _joystickOriginalPos;
    private Vector2 _joystickTouchPos;


    private GameObject PlayerMain_Cm;
    private PlayerController _playerController;

    [Header("Camera Setting")]
    [SerializeField]
    private GameObject _AroundTarget;
    [SerializeField]  private float lookRotateSpeed = 0.1f;
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

    private float dragStartPosition;
    
    public void StartDrag(PointerEventData data)
    {
        dragStartPosition = data.position.x;
    }
    private void DragEvent_Look(PointerEventData data)
    {
        #region 카메라 거리유지
        //float dragDeltaX = data.position.x - dragStartPosition;
        //float rotationAmount = dragDeltaX * lokRotateSpeed;

        //float newX = _CameraOriginPos.x + _CameraDistance * Mathf.Sin(rotationAmount * Mathf.Deg2Rad);
        //float newZ = _CameraOriginPos.z + _CameraDistance * Mathf.Cos(rotationAmount * Mathf.Deg2Rad);

        //Vector3 newPosition = new Vector3(newX, _CameraOriginPos.y, newZ);
        //_CameraTarget.transform.localPosition = newPosition;
        #endregion

        float dragDeltaX = data.position.x - dragStartPosition;
        float rotationAmount = dragDeltaX * lookRotateSpeed;

        Vector3 currentRotation = _AroundTarget.transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + rotationAmount, currentRotation.z);
        _AroundTarget.transform.rotation = Quaternion.Euler(newRotation);
    }
    private void EndDragEvent_JoyStick()
    {
        _moveDir = Vector2.zero;
        _handler.transform.position = _joystickOriginalPos;
        Managers.Event.MoveInputAction?.Invoke(_moveDir);
    }

    private void InitButton()
    {
        GetButton((int)Buttons.Jump_Button).gameObject.BindEvent((PointerEventData data) =>Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Jump));
        GetButton((int)Buttons.Sprint_Button).gameObject.BindEvent((PointerEventData data)=>Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Sprint));
        GetButton((int)Buttons.Attack_Button).gameObject.BindEvent((PointerEventData data)=>Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Attack));
        GetButton((int)Buttons.Auto_Button).gameObject.BindEvent((PointerEventData data) =>Managers.Event.KeyInputAction?.Invoke(Define.KeyInput.Auto));
        GetButton((int)Buttons.SkillQ_Button).gameObject.BindEvent((PointerEventData data)=>Managers.Event.SkillInputAction?.Invoke(Define.SkillType.QSKill));
        GetButton((int)Buttons.SkillW_Button).gameObject.BindEvent((PointerEventData data)=>Managers.Event.SkillInputAction?.Invoke(Define.SkillType.WSkill));
        GetButton((int)Buttons.SkillE_Button).gameObject.BindEvent((PointerEventData data)=>Managers.Event.SkillInputAction?.Invoke(Define.SkillType.ESkill));
        GetButton((int)Buttons.SkillR_Button).gameObject.BindEvent((PointerEventData data) => Managers.Event.SkillInputAction?.Invoke(Define.SkillType.RSkill));
    }

    private void OnDragEvent_JoyStick(PointerEventData data)
    {
     
        Vector2 dragPos = data.position;
        _moveDir = (dragPos - _joystickOriginalPos).normalized;
        Managers.Event.MoveInputAction?.Invoke(_moveDir);
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

    public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case Define.EVENT_TYPE.InventoryOpen:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(false);
               PlayerMain_Cm.SetActive(false);
                break;
            case Define.EVENT_TYPE.InventoryClose:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(true);
                PlayerMain_Cm.SetActive(true);
                break;
            case Define.EVENT_TYPE.ShopClose:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(true);
                PlayerMain_Cm.SetActive(true);
                break;
            case Define.EVENT_TYPE.ShopOpen:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(false);
                PlayerMain_Cm.SetActive(false);
                break;
            case Define.EVENT_TYPE.DialogOpen:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(false);
                PlayerMain_Cm.SetActive(false);
                break;
            case Define.EVENT_TYPE.DialogClose:
                GetObject((int)GameObjects.PlayerInput_Area).SetActive(true);
                PlayerMain_Cm.SetActive(true);
                break;

        }
    }
}
