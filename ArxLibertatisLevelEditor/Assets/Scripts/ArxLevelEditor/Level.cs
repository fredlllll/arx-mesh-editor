using ArxLibertatisEditorIO.MediumIO;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.ArxLevelLoading;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor
{
    public class Level
    {
        public string Name
        {
            get;
            private set;
        }

        public MediumArxLevel MediumArxLevel
        {
            get;
            private set;
        }

        public Vector3 LevelOffset
        {
            get { return LevelObject.transform.localPosition; }
            set { LevelObject.transform.localPosition = value; }
        }

        /// <summary>
        /// object containing the other stuff of the level
        /// </summary>
        public GameObject LevelObject { get; private set; }
        public GameObject LevelMeshObject { get; private set; }
        public GameObject LevelLightsObject { get; private set; }
        public GameObject LevelPortalsObject { get; private set; }
        public GameObject LevelIntersObject { get; private set; }
        public GameObject LevelNavGridObject { get; private set; }

        public EditableLevelMesh EditableLevelMesh { get; private set; }// { return LevelMeshObject.GetComponent<EditableLevelMesh>(); } }

        private GameObject GetLevelGameObject(string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(LevelObject.transform);
            return go;
        }

        public Level(string name, MediumArxLevel arxLevelNative)
        {
            Name = name;
            MediumArxLevel = arxLevelNative;

            LevelObject = new GameObject("Level");
            LevelMeshObject = GetLevelGameObject("Mesh");
            LevelLightsObject = GetLevelGameObject("Lights");
            LevelPortalsObject = GetLevelGameObject("Portals");
            LevelIntersObject = GetLevelGameObject("Inters");
            LevelNavGridObject = GetLevelGameObject("NavGrid");

            EditableLevelMesh = LevelMeshObject.AddComponent<EditableLevelMesh>();
            LevelMeshObject.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f); //1 unit is 1 cm in arx, so scale down so one unit is one meter (at least perceived)
        }

        public void Unload()
        {
            UnityEngine.Object.Destroy(LevelObject);
        }
    }
}
