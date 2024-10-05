using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class ColorPreview : MonoBehaviour
    {
        public ColorPicker picker;
        public Image image;

        private void Start()
        {
            picker.HChanged.AddListener(OnHSVChanged);
            picker.SChanged.AddListener(OnHSVChanged);
            picker.VChanged.AddListener(OnHSVChanged);
        }

        private void OnHSVChanged(float _)
        {
            image.color = picker.PickerColor;
        }
    }
}
