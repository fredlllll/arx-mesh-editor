using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class EditWindow : MonoBehaviour
    {
        public static RectTransform WindowTransform
        {
            get; private set;
        }

        public static int X
        {
            get { return (int)WindowTransform.offsetMin.x; }
        }

        public static int Y
        {
            get { return (int)WindowTransform.offsetMin.y; }
        }

        public static int Width
        {
            get { return (int)WindowTransform.rect.width; }
        }

        public static int Height
        {
            get { return (int)WindowTransform.rect.height; }
        }

        public static bool MouseInEditWindow
        {
            get; private set;
        }

        public static Vector3 MouseGlobalToLocal(Vector3 globalPos)
        {
            globalPos.x -= X;
            globalPos.y -= Y;
            return globalPos;
        }

        public static Ray GetRayFromMousePosition(Vector3 localMousePos)
        {
            localMousePos.x /= Width;
            localMousePos.y /= Height;

            return LevelEditor.EditorCamera.ViewportPointToRay(localMousePos);
        }

        public static Ray GetRayFromMousePosition()
        {
            return GetRayFromMousePosition(MouseGlobalToLocal(Input.mousePosition));
        }

        public static bool IsInEditWindow(Vector3 localMousePos)
        {
            return localMousePos.x >= 0 && localMousePos.y >= 0 && localMousePos.x < Width && localMousePos.y < Height;
        }

        public static bool IsInEditWindowGlobal(Vector3 globalMousePos)
        {
            return IsInEditWindow(MouseGlobalToLocal(globalMousePos));
        }

        private void Awake()
        {
            WindowTransform = GetComponent<RectTransform>();
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
