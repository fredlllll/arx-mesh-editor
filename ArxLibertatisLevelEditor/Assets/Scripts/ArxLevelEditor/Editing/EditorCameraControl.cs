using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class EditorCameraControl : MonoBehaviour
    {
        public float MouseSensitivity = 1;
        public float MoveSpeed = 1;
        public float ShiftBoost = 3;

        /// <summary>
        /// replicates the sensitivity of the legacy InputManager "Mouse X"/"Mouse Y" axes
        /// </summary>
        private const float mouseAxisScale = 0.1f;

        bool rotating = false;

        private void DoRotating()
        {
            var mouse = Mouse.current;
            if (mouse == null)
            {
                return;
            }

            Vector3 eulers = transform.eulerAngles;

            float rotateLeftRight = eulers.y;
            float rotateUpDown = eulers.x;

            float x = mouse.delta.x.ReadValue() * mouseAxisScale;
            float y = mouse.delta.y.ReadValue() * mouseAxisScale;

            rotateLeftRight += x;
            if (rotateLeftRight < 0)
            {
                rotateLeftRight += 360;
            }

            //this hack exists cause quaternions dont output negative values put in eulers. so clamp(-90,90) doesnt work properly
            rotateUpDown -= y;
            if (rotateUpDown < 0)
            {
                rotateUpDown += 360;
            }
            if (rotateUpDown > 89.99f && rotateUpDown <= 180)
            {
                rotateUpDown = 89.99f;
            }
            if (rotateUpDown > 180 && rotateUpDown < 270.01f)
            {
                rotateUpDown = 270.01f;
            }

            transform.eulerAngles = new Vector3(rotateUpDown, rotateLeftRight, 0);
        }

        private void DoMove()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            Vector3 offset = Vector3.zero;

            if (keyboard.wKey.isPressed)
            {
                offset += transform.forward;
            }
            if (keyboard.sKey.isPressed)
            {
                offset -= transform.forward;
            }
            if (keyboard.aKey.isPressed)
            {
                offset -= transform.right;
            }
            if (keyboard.dKey.isPressed)
            {
                offset += transform.right;
            }
            float moveSpeed = MoveSpeed;
            if (keyboard.leftShiftKey.isPressed)
            {
                moveSpeed *= ShiftBoost;
            }
            transform.position += offset * moveSpeed * Time.deltaTime;
        }

        public void Update()
        {
            var mouse = Mouse.current;
            if (mouse == null)
            {
                return;
            }

            if (EditWindow.MouseInEditWindow)
            {
                //only capture clicks when in edit window
                if (mouse.rightButton.wasPressedThisFrame)
                {
                    rotating = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }
            }

            if (rotating)
            {
                DoRotating();
                if (mouse.rightButton.wasReleasedThisFrame)
                {
                    rotating = false;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            DoMove();
        }
    }
}