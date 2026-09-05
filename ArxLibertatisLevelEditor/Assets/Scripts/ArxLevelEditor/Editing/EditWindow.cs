using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class EditWindow : MonoBehaviour
    {
        public static int X
        {
            get { return 0; }
        }

        public static int Y
        {
            get { return 0; }
        }

        public static VisualElement ViewportElement
        {
            get; set;
        }

        public static float Width
        {
            get
            {
                var element = ViewportElement;
                return element != null && !float.IsNaN(element.resolvedStyle.width) ? element.resolvedStyle.width : 0;
            }
        }

        public static float Height
        {
            get
            {
                var element = ViewportElement;
                return element != null && !float.IsNaN(element.resolvedStyle.height) ? element.resolvedStyle.height : 0;
            }
        }

        public static bool MouseInEditWindow
        {
            get; set;
        }

        public static Vector3 MouseGlobalToLocal(Vector3 globalPos)
        {
            globalPos.x -= X;
            globalPos.y -= Y;
            return globalPos;
        }

        public static Ray GetRayFromMousePosition(Vector3 localMousePos)
        {
            float width = Width;
            float height = Height;
            if (width <= 0 || height <= 0)
            {
                return new Ray();
            }

            localMousePos.x /= width;
            localMousePos.y /= height;

            var camera = EditorContext.EditorCamera;
            return camera != null ? camera.ViewportPointToRay(localMousePos) : new Ray();
        }

        public static Ray GetRayFromMousePosition()
        {
            return GetRayFromMousePosition(MouseGlobalToLocal(Mouse.current.position.ReadValue()));
        }

        public static bool IsInEditWindow(Vector3 localMousePos)
        {
            return localMousePos.x >= 0 && localMousePos.y >= 0 && localMousePos.x < Width && localMousePos.y < Height;
        }

        public static bool IsInEditWindowGlobal(Vector3 globalMousePos)
        {
            return IsInEditWindow(MouseGlobalToLocal(globalMousePos));
        }

        public void EventMouseEnter()
        {
            MouseInEditWindow = true;
        }

        public void EventMouseLeave()
        {
            MouseInEditWindow = false;
        }
    }
}