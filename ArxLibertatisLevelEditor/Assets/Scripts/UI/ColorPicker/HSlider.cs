using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class HSlider : MonoBehaviour
    {
        [SerializeField]
        private ColorPicker picker;
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private Text valueLabel;

        private bool receiveEvents = true;

        private void Start()
        {
            slider.onValueChanged.AddListener(OnValChanged);

            picker.HChanged.AddListener(OnHChanged);
            OnHChanged(picker.H);
        }

        private void OnHChanged(float h)
        {
            if (receiveEvents)
            {
                slider.value = h;
            }
        }

        private void OnValChanged(float val)
        {
            receiveEvents = false;
            picker.H = val;
            if (valueLabel != null)
            {
                valueLabel.text = ((int)(val * 255)).ToString();
            }
            receiveEvents = true;
        }
    }
}
