using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class HSlider :MonoBehaviour
    {
        public ColorPicker picker;
        public Slider slider;

        private void Start()
        {
            slider.onValueChanged.AddListener(OnValChanged);
        }

        private void OnValChanged(float val)
        {
            picker.UpdateH(val);
        }

        public void SetH(float h)
        {
            slider.value = h;
        }
    }
}
