using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class EditWindowState :MonoBehaviour
    {
        public static bool MouseInEditWindow
        {
            get;private set;
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
