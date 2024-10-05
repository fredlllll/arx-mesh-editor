using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class ColorCode : MonoBehaviour
    {
        [SerializeField]
        private ColorPicker picker;
        [SerializeField]
        private InputField text;

        private bool receiveEvents = true;
        private bool receiveValueChanged = true;

        private void Start()
        {
            picker.HChanged.AddListener(OnHSVChanged);
            picker.SChanged.AddListener(OnHSVChanged);
            picker.VChanged.AddListener(OnHSVChanged);

            text.onValueChanged.AddListener(OnValueChanged);
            text.onEndEdit.AddListener(OnEndEdit);

            OnHSVChanged(0);
        }

        private void OnHSVChanged(float _)
        {
            if (receiveEvents)
            {
                receiveValueChanged = false;
                text.text = "#" + ColorUtility.ToHtmlStringRGB(picker.PickerColor);
                receiveValueChanged = true;
            }
        }

        private void OnValueChanged(string text)
        {
            if (receiveValueChanged)
            {
                receiveEvents = false;
                if (text.StartsWith("#"))
                {
                    //gg consistency. Parse needs the # in front, while ToHtmlString doesnt return the #. Those two are supposed to be compatible
                    //TODO: this might be a potential bug if that behaviour is ever fixed in the future
                    if (ColorUtility.TryParseHtmlString(text, out Color col))
                    {
                        picker.PickerColor = col;
                    }
                }
            }
        }

        private void OnEndEdit(string text)
        {
            receiveEvents = true;
        }
    }
}
