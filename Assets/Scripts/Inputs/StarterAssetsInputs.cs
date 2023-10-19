using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 Move;
        public Vector2 Look;
        public bool Jump;
        public bool Sprint;
        public bool Crouch;
        public bool Aim;

        [Header("Movement Settings")]
        public bool AnalogMovement;

        [Header("Mouse Cursor Settings")]
        public bool CursorLocked = true;
        public bool CursorInputForLook = true;


#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (CursorInputForLook)
            {
                lookInput(context.ReadValue<Vector2>());
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            jumpInput(context.performed);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            sprintInput(context.performed);
        }
        public void OnCrouch(InputAction.CallbackContext context)
        {
            crouchInput(context.performed);
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            aimInput(context.performed);
        }

       


#endif

        void moveInput(Vector2 newMoveDirection)
        {
            Move = newMoveDirection;
        }

        void lookInput(Vector2 newLookDirection)
        {
            Look = newLookDirection;
        }

        void jumpInput(bool newJumpState)
        {
            Jump = newJumpState;
        }

        void sprintInput(bool newSprintState)
        {
            Sprint = newSprintState;
        }
        private void crouchInput(bool performed)
        {
            Crouch = performed;
        }
        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(CursorLocked);
        }
        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
        private void aimInput(bool performed)
        {
            Aim = performed;
        }
    }

}