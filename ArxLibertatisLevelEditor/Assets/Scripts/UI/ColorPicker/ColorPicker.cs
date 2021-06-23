using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class ColorPicker : MonoBehaviour
    {
        public RawImage SVAreaImage;
        private Material SVAreaImageMaterial;

        float h, s, v;

        private void Start()
        {
            SVAreaImageMaterial = Instantiate(SVAreaImage.material);
            SVAreaImage.material = SVAreaImageMaterial;
        }

        public void UpdateSV(float s, float v)
        {
            this.s = s;
            this.v = v;
        }

        public void UpdateS(float s)
        {
            this.s = s;
        }

        public void UpdateV(float v)
        {
            this.v = v;
        }

        public void UpdateH(float h)
        {
            this.h = h;
            UpdateSVMaterial();
        }

        private void UpdateSVMaterial()
        {
            SVAreaImageMaterial.SetFloat("_Hue", h);
        }

        private void UpdateColor()
        {

        }
    }
}
