using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class Selectable : MonoBehaviour
    {
        public Component target;

        public void Selected()
        {
            target.SendMessage("OnSelected");
        }

        public void Deselected()
        {
            target.SendMessage("OnDeselected");
        }
    }
}
