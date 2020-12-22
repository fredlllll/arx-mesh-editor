using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class EditWindowState : MonoBehaviour
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

        public static Ray GetRayFromMousePosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x -= X;
            mousePos.y -= Y;

            mousePos.x /= Width;
            mousePos.y /= Height;

            return LevelEditor.EditorCamera.ViewportPointToRay(mousePos);
        }

        private void Start()
        {
            WindowTransform = GetComponent<RectTransform>();
        }

        public void EventMouseEnter()
        {
            MouseInEditWindow = true;
        }

        public void EventMouseLeave()
        {
            MouseInEditWindow = true;
        }
    }
}
