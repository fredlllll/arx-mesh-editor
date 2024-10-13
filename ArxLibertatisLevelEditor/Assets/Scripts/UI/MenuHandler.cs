using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxNative.IO;
using Assets.Scripts.ArxNative.IO.DLF;
using Assets.Scripts.ArxNative.IO.FTS;
using Assets.Scripts.ArxNative.IO.LLF;
using Assets.Scripts.ArxNative.IO.PK;
using SFB;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MenuHandler : MonoBehaviour
    {
        const string ArxDirPathName = "ArxDirPathName";

        public InputField levelName;
        public InputField arxDirPath;

        public void Awake()
        {
            //TODO: move this to a gameobject that is loaded on startup
            //TODO: check if arx folder is set, if not, TODO: show dialog box?
            var dataDir = PlayerPrefs.GetString(ArxDirPathName, null);
            try
            {
                if (string.IsNullOrEmpty(dataDir) || !Directory.Exists(dataDir))
                {
                    gameObject.SetActive(true);
                }
            }
            catch
            { //malformed path or something
                gameObject.SetActive(true);
                dataDir = null;
            }
            arxDirPath.text = dataDir ?? "";
            ArxLibertatisEditorIO.ArxPaths.DataDir = arxDirPath.text;
        }

        public void OpenLevelClicked()
        {
            if (!Directory.Exists(ArxLibertatisEditorIO.ArxPaths.DataDir))
            {
                //TODO: show error message
            }
            else
            {
                LevelEditor.OpenLevel(levelName.text);
                gameObject.SetActive(false);
            }
        }

        public void SearchDirectoryClicked()
        {
            StandaloneFileBrowser.OpenFolderPanelAsync("Find Arx Directory", "", false, this.DirectorySelected);
        }

        private void DirectorySelected(string[] folders)
        {
            if (folders.Length > 0)
            {
                arxDirPath.text = folders[0];
                OnEndEditDirectory(arxDirPath.text);
            }
        }

        public void OnEndEditDirectory(string _)
        {
            //only gets called when editing in the ui, have to manually call it when setting text field in code
            ArxLibertatisEditorIO.ArxPaths.DataDir = arxDirPath.text;
            PlayerPrefs.SetString(ArxDirPathName, arxDirPath.text);
        }
    }
}
