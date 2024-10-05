using ArxLibertatisEditorIO.MediumIO;
using Assets.Scripts.ArxLevelEditor.Mesh;
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

        public EditableLevelMesh EditableLevelMesh { get { return LevelMeshObject.GetComponent<EditableLevelMesh>(); } }

        public Level(string name, MediumArxLevel arxLevelNative)
        {
            Name = name;
            MediumArxLevel = arxLevelNative;

            LevelObject = new GameObject(name);
            LevelMeshObject = new GameObject(name + "Mesh");
            LevelMeshObject.AddComponent<EditableLevelMesh>();
            LevelMeshObject.transform.SetParent(LevelObject.transform);
            LevelObject.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f); //1 unit is 1 cm in arx, so scale down so one unit is one meter (at least perceived)
        }
    }
}
