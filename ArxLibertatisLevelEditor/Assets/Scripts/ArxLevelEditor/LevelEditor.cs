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

    public class LevelEditor : MonoBehaviour
    {
        public static Camera EditorCamera { get; set; }

        public static Level CurrentLevel { get; set; }

        public static TextureDatabase TextureDatabase { get; } = new TextureDatabase();

        public static EditState EditState { get; set; } = EditState.Polygons;

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