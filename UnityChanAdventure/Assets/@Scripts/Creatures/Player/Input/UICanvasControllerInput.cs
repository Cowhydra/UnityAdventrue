using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour,IListener
    {


      
        private void Start()
        {
            Managers.Event.AddListener(Define.EVENT_TYPE.InventoryOpen, this);
            Managers.Event.AddListener(Define.EVENT_TYPE.InventoryClose, this);
            Managers.Event.AddListener(Define.EVENT_TYPE.ShopClose, this);
            Managers.Event.AddListener(Define.EVENT_TYPE.ShopOpen, this);
            Managers.Event.AddListener(Define.EVENT_TYPE.DialogOpen, this);
            Managers.Event.AddListener(Define.EVENT_TYPE.DialogClose, this);
        }
        public void OnEvent(Define.EVENT_TYPE Event_Type, Component Sender, object Param = null)
        {
            switch (Event_Type)
            {
                case Define.EVENT_TYPE.InventoryOpen:
                    gameObject.SetActive(false);
                    break;
                case Define.EVENT_TYPE.InventoryClose:
                    gameObject.SetActive(true);
                    break;
                case Define.EVENT_TYPE.ShopClose:
                    gameObject.SetActive(true);
                    break;
                case Define.EVENT_TYPE.ShopOpen:
                    gameObject.SetActive(false);
                    break;
                case Define.EVENT_TYPE.DialogOpen:
                    gameObject.SetActive(false);
                    break;
                case Define.EVENT_TYPE.DialogClose:
                    gameObject.SetActive(true);
                    break;

            }
        }
    }

}
