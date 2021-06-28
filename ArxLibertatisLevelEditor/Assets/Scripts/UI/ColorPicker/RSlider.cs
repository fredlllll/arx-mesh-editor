using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class RSlider : MonoBehaviour
    {
        [SerializeField]
        private ColorPicker picker;
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private Text valueLabel;
        [SerializeField]
        private Image background;
        private Material backgroundMaterial;

        private bool receiveEvents = true;

        private void Start()
        {
            background.material = backgroundMaterial = Instantiate(background.material);

            picker.HChanged.AddListener(OnHSVChanged);
            picker.SChanged.AddListener(OnHSVChanged);
            picker.VChanged.AddListener(OnHSVChanged);
            OnHSVChanged(0);
            UpdateValueLabel();

            slider.onValueChanged.AddListener(OnValChanged);
        }

        private void UpdateValueLabel()
        {
            if (valueLabel != null)
            {
                valueLabel.text = ((int)(slider.value * 255)).ToString();
            }
        }

        private void OnHSVChanged(float _)
        {
            if (receiveEvents)
            {
                var col = picker.PickerColor;
                slider.value = col.r;
                backgroundMaterial.SetFloat("_G", col.g);
                backgroundMaterial.SetFloat("_B", col.b);
            }
        }

        private void OnValChanged(float val)
        {
            receiveEvents = false;
            picker.R = val;
            UpdateValueLabel();
            receiveEvents = true;
        }
    }
}
