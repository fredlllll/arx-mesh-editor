using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.ColorPicker
{
    public class SVPicker : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Canvas canvas;
        private RectTransform rectTransform;
        private RectTransform parentRectTransform;

        public ColorPicker picker;

        void Start()
        {
            canvas = this.GetComponentInParent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            parentRectTransform = GetComponentInParent<RectTransform>();
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

            picker.UpdateSV(s, v);
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void SetS(float s)
        {
            float parentWidth = parentRectTransform.sizeDelta.x;

            Vector2 newPos = rectTransform.anchoredPosition;
            newPos.x = Mathf.Clamp(s * parentWidth, 0, parentWidth);
            rectTransform.anchoredPosition = newPos;

            picker.UpdateS(s);
        }

        public void SetV(float v)
        {
            float parentHeight = parentRectTransform.sizeDelta.y;

            Vector2 newPos = rectTransform.anchoredPosition;
            newPos.y = Mathf.Clamp(v * parentHeight, 0, parentHeight);
            rectTransform.anchoredPosition = newPos;

            picker.UpdateV(v);
        }
    }
}
