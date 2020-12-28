using UnityEngine;
using Assets.Scripts.ArxLevel;
using Assets.Scripts.ArxLevelLoading;

namespace Assets.Scripts.ArxLevelEditor
{
    public enum EditState
    {
        Polygons,
        Vertices
    }

    public enum SnapMode
    {
        None = 0,
        Grid = 1,
        Vertex = 2,
    }

    public class LevelEditor : MonoBehaviour
    {
        public static Camera EditorCamera { get; set; }

        public static Level CurrentLevel { get; set; }

        public static TextureDatabase TextureDatabase { get; } = new TextureDatabase();

        public static EditState EditState { get; set; } = EditState.Polygons;

        public static SnapMode SnapMode { get; set; } = SnapMode.None;

        public static void OpenLevel(string name)
        {
            if (CurrentLevel != null)
            {
                Object.Destroy(CurrentLevel.LevelObject);
            }

            TextureDatabase.Clear();

            CurrentLevel = LevelLoader.LoadLevel(name);
        }

        private void Awake()
        {
            EditorCamera = Camera.main;
        }
    }
}