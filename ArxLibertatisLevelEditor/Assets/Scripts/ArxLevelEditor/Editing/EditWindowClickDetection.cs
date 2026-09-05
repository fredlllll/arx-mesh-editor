using Assets.Scripts.Util;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    using ClickHandler = Func<Vector3, int, bool>;
    using MouseDownHandler = Func<Vector3, int, bool>;
    using MouseUpHandler = Func<Vector3, int, bool, bool>;
    using BeginDragHandler = Func<Vector3, int, bool>;
    using DragHandler = Func<Vector3, Vector3, Vector3, int, bool>;
    using EndDragHandler = Func<Vector3, int, bool>;

    public class EditWindowClickDetection : MonoBehaviour
    {
        public const int BTN_PRIMARY = 0;
        public const int BTN_SECONDARY = 1;
        public const int BTN_MIDDLE = 2;

        public static EditWindowClickDetection Instance { get; private set; } = null;

        public static readonly PriorityList<ClickHandler> clickHandlers = new PriorityList<ClickHandler>();
        public static readonly PriorityList<MouseDownHandler> mouseDownHandlers = new PriorityList<MouseDownHandler>();
        public static readonly PriorityList<MouseUpHandler> mouseUpHandlers = new PriorityList<MouseUpHandler>();
        public static readonly PriorityList<BeginDragHandler> beginDragHandlers = new PriorityList<BeginDragHandler>();
        public static readonly PriorityList<DragHandler> dragHandlers = new PriorityList<DragHandler>();
        public static readonly PriorityList<EndDragHandler> endDragHandlers = new PriorityList<EndDragHandler>();

        private static readonly Vector3[] lastPosition = new Vector3[3];
        private static readonly Vector3[] mouseDownPosition = new Vector3[3];
        private static readonly bool[] mouseButtonDown = new bool[] { false, false, false };
        private static readonly bool[] mouseButtonMoved = new bool[] { false, false, false };
        private static readonly bool[] isDragging = new bool[] { false, false, false };

        public float distanceTillDrag = 3; //3 pixels of movement till it actually counts as drag

        public static float DragThreshold = 3; //3 pixels of movement till it actually counts as drag

        private static ButtonControl GetMouseButton(int btn)
        {
            var mouse = Mouse.current;
            if (mouse == null)
            {
                return null;
            }
            switch (btn)
            {
                case BTN_PRIMARY: return mouse.leftButton;
                case BTN_SECONDARY: return mouse.rightButton;
                case BTN_MIDDLE: return mouse.middleButton;
                default: return null;
            }
        }

        public static void HandlePointerDown(Vector3 localPos, int mouseButton)
        {
            if (mouseButton < 0 || mouseButton > 2)
            {
                return;
            }
            Cursor.lockState = CursorLockMode.Confined;
            FireMouseDown(localPos, mouseButton);
            mouseButtonDown[mouseButton] = true;
            mouseButtonMoved[mouseButton] = false;
            mouseDownPosition[mouseButton] = localPos;
            lastPosition[mouseButton] = localPos;
        }

        public static void HandlePointerUp(Vector3 localPos, int mouseButton)
        {
            if (mouseButton < 0 || mouseButton > 2)
            {
                return;
            }
            Cursor.lockState = CursorLockMode.None;
            FireMouseUp(localPos, mouseButton, EditWindow.MouseInEditWindow);
            if (EditWindow.MouseInEditWindow && mouseButtonDown[mouseButton] && !mouseButtonMoved[mouseButton])
            {
                FireClick(localPos, mouseButton);
            }
            mouseButtonDown[mouseButton] = false;
            if (isDragging[mouseButton])
            {
                FireEndDrag(localPos, mouseButton);
                isDragging[mouseButton] = false;
            }
            lastPosition[mouseButton] = localPos;
        }

        public static void HandlePointerMove(Vector3 localPos, int mouseButton)
        {
            if (mouseButton < 0 || mouseButton > 2)
            {
                return;
            }
            var offset = localPos - lastPosition[mouseButton];
            if (isDragging[mouseButton])
            {
                FireDrag(lastPosition[mouseButton], localPos, offset, mouseButton);
            }
            else if (mouseButtonDown[mouseButton] && EditWindow.IsInEditWindow(mouseDownPosition[mouseButton])) //only start drag if mouse down position was inside window
            {
                var offsetSinceDown = localPos - mouseDownPosition[mouseButton];
                var distanceSinceDown = offsetSinceDown.magnitude;
                if (distanceSinceDown >= DragThreshold)
                {
                    mouseButtonMoved[mouseButton] = true;
                    FireBeginDrag(mouseDownPosition[mouseButton], mouseButton);
                    isDragging[mouseButton] = true;
                    FireDrag(mouseDownPosition[mouseButton], localPos, offsetSinceDown, mouseButton);
                }
            }
            lastPosition[mouseButton] = localPos;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Only one instance of edit window click detection allowed");
            }
            Instance = this;
        }

        private void Start()
        {
            if (EditWindow.ViewportElement != null)
            {
                return;
            }
            if (Mouse.current == null)
            {
                return;
            }
            Vector3 posLocal = EditWindow.MouseGlobalToLocal(Mouse.current.position.ReadValue());
            for (int i = 0; i < lastPosition.Length; i++)
            {
                lastPosition[i] = posLocal;
            }
        }

        private static void FireEvent<T1, T2>(PriorityList<Func<T1, T2, bool>> prioList, T1 t1, T2 t2)
        {
            foreach (var prio in prioList.GetPriorities())
            {
                foreach (var handler in prioList.GetPriorityItems(prio))
                {
                    if (handler(t1, t2))
                    {
                        return;
                    }
                }
            }
        }

        private static void FireEvent<T1, T2, T3>(PriorityList<Func<T1, T2, T3, bool>> prioList, T1 t1, T2 t2, T3 t3)
        {
            foreach (var prio in prioList.GetPriorities())
            {
                foreach (var handler in prioList.GetPriorityItems(prio))
                {
                    if (handler(t1, t2, t3))
                    {
                        return;
                    }
                }
            }
        }

        private static void FireEvent<T1, T2, T3, T4>(PriorityList<Func<T1, T2, T3, T4, bool>> prioList, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            foreach (var prio in prioList.GetPriorities())
            {
                foreach (var handler in prioList.GetPriorityItems(prio))
                {
                    if (handler(t1, t2, t3, t4))
                    {
                        return;
                    }
                }
            }
        }

        private static void FireMouseDown(Vector3 mousePosLocal, int mouseButton)
        {
            FireEvent(mouseDownHandlers, mousePosLocal, mouseButton);
        }

        private static void FireMouseUp(Vector3 mousePosLocal, int mouseButton, bool insideEditWindow)
        {
            FireEvent(mouseUpHandlers, mousePosLocal, mouseButton, insideEditWindow);
        }

        private static void FireClick(Vector3 mousePosLocal, int mouseButton)
        {
            FireEvent(clickHandlers, mousePosLocal, mouseButton);
        }

        private static void FireBeginDrag(Vector3 mousePosLocal, int mouseButton)
        {
            FireEvent(beginDragHandlers, mousePosLocal, mouseButton);
        }

        private static void FireDrag(Vector3 lastPos, Vector3 nowPos, Vector3 offset, int mouseButton)
        {
            FireEvent(dragHandlers, lastPos, nowPos, offset, mouseButton);
        }

        private static void FireEndDrag(Vector3 mousePosLocal, int mouseButton)
        {
            FireEvent(endDragHandlers, mousePosLocal, mouseButton);
        }

        private void UpdateButton(int btn)
        {
            var button = GetMouseButton(btn);
            if (button == null)
            {
                return;
            }
            var mousePosLocal = EditWindow.MouseGlobalToLocal(Mouse.current.position.ReadValue());
            if (EditWindow.MouseInEditWindow)
            {
                if (button.wasPressedThisFrame)
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    FireMouseDown(mousePosLocal, btn);
                    mouseButtonDown[btn] = true;
                    mouseButtonMoved[btn] = false;
                    mouseDownPosition[btn] = mousePosLocal;
                }
            }
            if (button.wasReleasedThisFrame)
            {
                Cursor.lockState = CursorLockMode.None;
                FireMouseUp(mousePosLocal, btn, EditWindow.MouseInEditWindow);
                if (EditWindow.MouseInEditWindow && mouseButtonDown[btn] && !mouseButtonMoved[btn])
                {
                    FireClick(mousePosLocal, btn);
                }
                mouseButtonDown[btn] = false;
                if (isDragging[btn])
                {
                    FireEndDrag(mousePosLocal, btn);
                    isDragging[btn] = false;
                }
            }

            if (button.isPressed)
            {
                if (isDragging[btn])
                {
                    //var offset = new Vector3(Input.GetAxis("Mouse X") * 5, Input.GetAxis("Mouse Y") * 5, 0);
                    var offset = mousePosLocal - lastPosition[btn];
                    FireDrag(lastPosition[btn], mousePosLocal, offset, btn);
                }
                else if (mouseButtonDown[btn] && EditWindow.IsInEditWindow(mouseDownPosition[btn])) //only start drag if mouse down position was inside window
                {
                    var offsetSinceDown = mousePosLocal - mouseDownPosition[btn];
                    var distanceSinceDown = offsetSinceDown.magnitude;
                    if (distanceSinceDown >= distanceTillDrag)
                    {
                        mouseButtonMoved[btn] = true;
                        FireBeginDrag(mouseDownPosition[btn], btn);
                        isDragging[btn] = true;
                        FireDrag(mouseDownPosition[btn], mousePosLocal, offsetSinceDown, btn);
                    }
                }
            }

            lastPosition[btn] = mousePosLocal;
        }

        private void Update()
        {
            if (EditWindow.ViewportElement != null)
            {
                return;
            }
            UpdateButton(BTN_PRIMARY);
            UpdateButton(BTN_SECONDARY);
            UpdateButton(BTN_MIDDLE);
        }
    }
}
