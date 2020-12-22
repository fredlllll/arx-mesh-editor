using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor
{
    public class LevelEditorInit : MonoBehaviour
    {
        private void Awake()
        {
            LevelEditor.EditorCamera = Camera.main;
        }
    }
}
