using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour,IListener
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }
        private void Start()
        {
            Managers.Event.AddListener(Define.EVENT_TYPE.InventoryOpen, this);
            Managers.Event.AddListener(Define.EVENT_TYPE.InventoryClose, this);
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
            }
        }
    }

}
