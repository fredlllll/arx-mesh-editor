using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class SSlider : MonoBehaviour
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
            slider.onValueChanged.AddListener(OnValChanged);

            background.material = backgroundMaterial = Instantiate(background.material);

            picker.HChanged.AddListener(OnHChanged);
            picker.SChanged.AddListener(OnSChanged);
            picker.VChanged.AddListener(OnVChanged);
            OnHChanged(picker.H);
            OnSChanged(picker.S);
            OnVChanged(picker.V);
        }

        private void OnHChanged(float h)
        {
            if (receiveEvents)
            {
                backgroundMaterial.SetFloat("_Hue", h);
            }
        }

        private void OnSChanged(float s)
        {
            if (receiveEvents)
            {
                slider.value = s;
            }
        }

        private void OnVChanged(float v)
        {
            if (receiveEvents)
            {
                backgroundMaterial.SetFloat("_Value", v);
            }
        }

        private void OnValChanged(float val)
        {
            receiveEvents = false;
            picker.S = val;
            if (valueLabel != null)
            {
                valueLabel.text = ((int)(val * 255)).ToString();
            }
            receiveEvents = true;
        }
    }
}
