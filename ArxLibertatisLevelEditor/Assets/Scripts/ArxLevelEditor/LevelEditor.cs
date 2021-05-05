using UnityEngine;
using Assets.Scripts.ArxLevelLoading;
using Assets.Scripts.ArxLevelEditor.Editing;

namespace Assets.Scripts.ArxLevelEditor
{
    public enum EditState
    {
        Polygons,
        Vertices
    }

    public class LevelEditor : MonoBehaviour
    {
        public static Camera EditorCamera { get; set; }

        public static Level CurrentLevel { get; set; }

        public static TextureDatabase TextureDatabase { get; } = new TextureDatabase();

        public static EditState EditState { get; set; } = EditState.Polygons;

        public static SnapManager SnapManager { get; } = new SnapManager();

        public static void OpenLevel(string name)
        {
            if (CurrentLevel != null)
            {
                Gizmo.Detach(); //prevent gizmo from being deleted with the level
                Gizmo.Visible = false;
                Destroy(CurrentLevel.LevelObject);
            }

            TextureDatabase.Clear();

            CurrentLevel = LevelLoader.LoadLevel(name);
        }

        public static void SaveLevel()
        {
            if(CurrentLevel != null)
            {
                LevelSaver.SaveLevel(CurrentLevel, CurrentLevel.Name);
            }
        }

        private void Awake()
        {
            EditorCamera = Camera.main;
        }
    }
}