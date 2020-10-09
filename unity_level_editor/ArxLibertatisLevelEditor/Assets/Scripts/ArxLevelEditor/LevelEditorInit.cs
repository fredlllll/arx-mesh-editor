using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor
{
    public class LevelEditorInit : MonoBehaviour
    {
        private void Start()
        {
            LevelEditor.EditorCamera = Camera.main;
        }
    }
}
