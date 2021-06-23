using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.ColorPicker
{
    public class ColorPicker :MonoBehaviour
    {
        public SVPicker svPicker;
        public GameObject svArea;

        private RectTransform svPickerRectTransform;
        private RectTransform svAreaRectTransform;

        float h, s, v;

        private void Start()
        {
            svPickerRectTransform = svPicker.GetComponent<RectTransform>();
            svAreaRectTransform = svArea.GetComponent<RectTransform>();
        }


        public void UpdateSV(float s, float v)
        {
            

        }

        private void UpdateSVMaterial()
        {

        }
    }
}
