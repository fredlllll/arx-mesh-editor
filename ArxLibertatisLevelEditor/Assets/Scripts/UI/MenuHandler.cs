using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxNative.IO;
using Assets.Scripts.ArxNative.IO.DLF;
using Assets.Scripts.ArxNative.IO.FTS;
using Assets.Scripts.ArxNative.IO.LLF;
using Assets.Scripts.ArxNative.IO.PK;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MenuHandler : MonoBehaviour
    {
        public InputField levelName;

        public void OpenMenuClicked()
        {
            LevelEditor.OpenLevel(levelName.text);
            gameObject.SetActive(false);
        }
    }
}
