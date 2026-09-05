using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Editing;
using Assets.Scripts.ArxLevelLoading;
using UnityEngine;

namespace Assets.Scripts.App
{
    public class LevelEditor : MonoBehaviour
    {
        public static void OpenLevel(string name)
        {
            if (EditorContext.CurrentLevel != null)
            {
                Gizmo_OLD.Detach(); //prevent gizmo from being deleted with the level
                Gizmo_OLD.Visible = false;
                EditorContext.CurrentLevel.Unload();
            }

            EditorContext.CurrentLevel = LevelLoader.LoadLevel(name);
        }

        public static void SaveLevel()
        {
            if (EditorContext.CurrentLevel != null)
            {
                PolygonSelector.Instance.Deselect();
                LevelSaver.SaveLevel(EditorContext.CurrentLevel, EditorContext.CurrentLevel.Name);
            }
        }

        private void Awake()
        {
            EditorContext.Initialize();
        }
    }
}