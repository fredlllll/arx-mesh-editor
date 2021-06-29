using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class ColorComponentChangedEvent : UnityEvent<float> { }
    public class ColorChangedEvent : UnityEvent<Color> { }

    public class ColorPicker : MonoBehaviour
    {
        public ColorComponentChangedEvent HChanged { get; } = new ColorComponentChangedEvent();
        public ColorComponentChangedEvent SChanged { get; } = new ColorComponentChangedEvent();
        public ColorComponentChangedEvent VChanged { get; } = new ColorComponentChangedEvent();
        public ColorChangedEvent ColorChanged { get; } = new ColorChangedEvent();

        [SerializeField]
        float h = 0, s = 0, v = 0;
        public float H
        {
            get { return h; }
            set
            {
                h = value;
                HChanged.Invoke(value);
                ColorChanged.Invoke(PickerColor);
            }
        }

        public float S
        {
            get { return s; }
            set
            {
                s = value;
                SChanged.Invoke(value);
                ColorChanged.Invoke(PickerColor);
            }
        }

        public float V
        {
            get { return v; }
            set
            {
                v = value;
                VChanged.Invoke(value);
                ColorChanged.Invoke(PickerColor);
            }
        }

        public float R
        {
            get { return PickerColor.r; }
            set
            {
                var col = PickerColor;
                col.r = value;
                Color.RGBToHSV(col, out h, out s, out v);
                HChanged.Invoke(h);
                SChanged.Invoke(s);
                VChanged.Invoke(v);
                ColorChanged.Invoke(PickerColor);
            }
        }

        public float G
        {
            get { return PickerColor.r; }
            set
            {
                var col = PickerColor;
                col.g = value;
                Color.RGBToHSV(col, out h, out s, out v);
                HChanged.Invoke(h);
                SChanged.Invoke(s);
                VChanged.Invoke(v);
                ColorChanged.Invoke(PickerColor);
            }
        }

        public float B
        {
            get { return PickerColor.r; }
            set
            {
                var col = PickerColor;
                col.b = value;
                Color.RGBToHSV(col, out h, out s, out v);
                HChanged.Invoke(h);
                SChanged.Invoke(s);
                VChanged.Invoke(v);
                ColorChanged.Invoke(PickerColor);
            }
        }

        public Color PickerColor
        {
            get
            {
                return Color.HSVToRGB(h, s, v);
            }
            set
            {
                Color.RGBToHSV(value, out h, out s, out v);
                HChanged.Invoke(h);
                SChanged.Invoke(s);
                VChanged.Invoke(v);
                ColorChanged.Invoke(PickerColor);
            }
        }
    }
}
