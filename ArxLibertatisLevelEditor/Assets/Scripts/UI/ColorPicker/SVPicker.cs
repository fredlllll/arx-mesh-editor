using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.ColorPicker
{
    public class SVPicker : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Canvas canvas;
        private RectTransform rectTransform;
        private RectTransform parentRectTransform;
        private Material svMaterial;

        [SerializeField]
        private ColorPicker picker;
        [SerializeField]
        private RawImage svImage;

        private bool receiveEvents = true;


        void Start()
        {
            canvas = this.GetComponentInParent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            parentRectTransform = rectTransform.parent as RectTransform;

            svImage.material = svMaterial = Instantiate(svImage.material);

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
                svMaterial.SetFloat("_Hue", h);
            }
        }
        private void OnSChanged(float s)
        {
            if (receiveEvents)
            {
                float parentWidth = parentRectTransform.sizeDelta.x;
                Vector2 newPos = rectTransform.anchoredPosition;
                newPos.x = Mathf.Clamp(s * parentWidth, 0, parentWidth);
                rectTransform.anchoredPosition = newPos;
            }
        }
        private void OnVChanged(float v)
        {
            if (receiveEvents)
            {
                float parentHeight = parentRectTransform.sizeDelta.y;
                Vector2 newPos = rectTransform.anchoredPosition;
                newPos.y = Mathf.Clamp(v * parentHeight, 0, parentHeight);
                rectTransform.anchoredPosition = newPos;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            //TODO: this is behaving weirdly when you move the mouse too fast or go outside the area, but its good enough for a proof of concept
            var delta = eventData.delta / canvas.scaleFactor;

            float parentWidth = parentRectTransform.sizeDelta.x;
            float parentHeight = parentRectTransform.sizeDelta.y;

            Vector2 newPos = rectTransform.anchoredPosition + delta;
            newPos.x = Mathf.Clamp(newPos.x, 0, parentWidth);
            newPos.y = Mathf.Clamp(newPos.y, 0, parentHeight);
            rectTransform.anchoredPosition = newPos;

            float s = newPos.x / parentRectTransform.sizeDelta.x;
            float v = newPos.y / parentRectTransform.sizeDelta.y;

            receiveEvents = false;
            picker.S = s;
            picker.V = v;
            receiveEvents = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}
