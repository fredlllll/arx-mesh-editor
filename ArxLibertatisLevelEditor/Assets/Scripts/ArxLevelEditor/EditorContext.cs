using Assets.Scripts.ArxLevelEditor.Editing;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor
{
    /// <summary>
    /// runtime state shared between the editor logic, the level io and the ui without creating an assembly cycle
    /// </summary>
    public static class EditorContext
    {
        public static Camera EditorCamera { get; set; }

        public static Level CurrentLevel { get; set; }

        public static TextureDatabase TextureDatabase { get; } = new TextureDatabase();

        public static EditState EditState { get; set; } = EditState.Polygons;

        public static SnapManager SnapManager { get; } = new SnapManager();

        public static void Initialize()
        {
            EditorCamera = Camera.main;
            TextureDatabase.Awake();
        }
    }
}