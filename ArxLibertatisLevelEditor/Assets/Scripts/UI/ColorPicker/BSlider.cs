using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class BSlider : MonoBehaviour
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
                backgroundMaterial.SetFloat("_R", col.r);
                backgroundMaterial.SetFloat("_G", col.g);
                slider.value = col.b;
            }
        }

        private void OnValChanged(float val)
        {
            receiveEvents = false;
            picker.B = val;
            UpdateValueLabel();
            receiveEvents = true;
        }
    }
}
