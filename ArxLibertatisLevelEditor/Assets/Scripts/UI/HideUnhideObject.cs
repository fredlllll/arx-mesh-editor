using UnityEngine;

namespace Assets.Scripts.UI
{
    public class HideUnhideObject : MonoBehaviour
    {
        public GameObject target;

        public void Hide()
        {
            target.SetActive(false);
        }

        public void Unhide()
        {
            target.SetActive(true);
        }

        public void Toggle()
        {
            target.SetActive(target.activeSelf);
        }
    }
}
