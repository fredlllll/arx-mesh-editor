using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class EditorCameraControl : MonoBehaviour
    {
        public float MouseSensitivity = 1;
        public float MoveSpeed = 1;
        public float ShiftBoost = 3;

        bool rotating = false;

        private void DoRotating()
        {
            Vector3 eulers = transform.eulerAngles;

            float rotateLeftRight = eulers.y;
            float rotateUpDown = eulers.x;

            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            rotateLeftRight += x;
            if(rotateLeftRight < 0)
            {
                rotateLeftRight += 360;
            }

            //this hack exists cause quaternions dont output negative values put in eulers. so clamp(-90,90) doesnt work properly
            rotateUpDown -= y;
            if (rotateUpDown < 0)
            {
                rotateUpDown += 360;
            }
            if(rotateUpDown > 89.99f && rotateUpDown<= 180)
            {
                rotateUpDown = 89.99f;
            }
            if(rotateUpDown > 180 && rotateUpDown < 270.01f)
            {
                rotateUpDown = 270.01f;
            }

            transform.eulerAngles = new Vector3(rotateUpDown, rotateLeftRight, 0);
        }

        private void DoMove()
        {
            Vector3 offset = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                offset += transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                offset -= transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                offset -= transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                offset += transform.right;
            }
            float moveSpeed = MoveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed *= ShiftBoost;
            }
            transform.position += offset * moveSpeed * Time.deltaTime;
        }

        public void Update()
        {
            if (EditWindowState.MouseInEditWindow)
            {
                //only capture clicks when in edit window
                if (Input.GetMouseButtonDown(1))
                {
                    rotating = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }
            }

            if (rotating)
            {
                DoRotating();
                if (Input.GetMouseButtonUp(1))
                {
                    rotating = false;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            DoMove();
        }
    }
}
