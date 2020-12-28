using Assets.Scripts.Util;
using System;
using UnityEngine;

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
            Vector3 posLocal = EditWindow.MouseGlobalToLocal(Input.mousePosition);
            for (int i = 0; i < lastPosition.Length; i++)
            {
                lastPosition[i] = posLocal;
            }
        }

        private void FireEvent<T1, T2>(PriorityList<Func<T1, T2, bool>> prioList, T1 t1, T2 t2)
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

        private void FireEvent<T1, T2, T3>(PriorityList<Func<T1, T2, T3, bool>> prioList, T1 t1, T2 t2, T3 t3)
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

        private void FireEvent<T1, T2, T3, T4>(PriorityList<Func<T1, T2, T3, T4, bool>> prioList, T1 t1, T2 t2, T3 t3, T4 t4)
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

        private void FireMouseDown(Vector3 mousePosLocal, int mouseButton)
        {
            FireEvent(mouseDownHandlers, mousePosLocal, mouseButton);
        }

        private void FireMouseUp(Vector3 mousePosLocal, int mouseButton, bool insideEditWindow)
        {
            FireEvent(mouseUpHandlers, mousePosLocal, mouseButton, insideEditWindow);
        }

        private void FireClick(Vector3 mousePosLocal, int mouseButton)
        {
            FireEvent(clickHandlers, mousePosLocal, mouseButton);
        }

        private void FireBeginDrag(Vector3 mousePosLocal, int mouseButton)
        {
            FireEvent(beginDragHandlers, mousePosLocal, mouseButton);
        }

        private void FireDrag(Vector3 lastPos, Vector3 nowPos, Vector3 offset, int mouseButton)
        {
            FireEvent(dragHandlers, lastPos, nowPos, offset, mouseButton);
        }

        private void FireEndDrag(Vector3 mousePosLocal, int mouseButton)
        {
            FireEvent(endDragHandlers, mousePosLocal, mouseButton);
        }

        private void UpdateButton(int btn)
        {
            var mousePosLocal = EditWindow.MouseGlobalToLocal(Input.mousePosition);
            if (EditWindow.MouseInEditWindow)
            {
                if (Input.GetMouseButtonDown(btn))
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    FireMouseDown(mousePosLocal, btn);
                    mouseButtonDown[btn] = true;
                    mouseButtonMoved[btn] = false;
                    mouseDownPosition[btn] = mousePosLocal;
                }
            }
            if (Input.GetMouseButtonUp(btn))
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

            if (Input.GetMouseButton(btn))
            {
                if (isDragging[btn])
                {
                    var offset = mousePosLocal - lastPosition[btn];
                    FireDrag(lastPosition[btn], mousePosLocal, offset, btn);
                }
                else if (EditWindow.MouseInEditWindow) //TODO: should check if mousedown pos is in edit window, but this is at max 3 pixels where you cant drag so no magic for now
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
            UpdateButton(BTN_PRIMARY);
            UpdateButton(BTN_SECONDARY);
            UpdateButton(BTN_MIDDLE);
        }
    }
}
