using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.ArxNative;
using UnityEngine;

namespace Assets.Scripts.ArxLevel
{
    public class Level
    {
        public string Name
        {
            get;
            private set;
        }

        public ArxLevelNative ArxLevelNative
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
        public GameObject LevelMesh { get; private set; }

        public EditableLevelMesh EditableLevelMesh { get { return LevelMesh.GetComponent<EditableLevelMesh>(); } }

        public Level(string name, ArxLevelNative arxLevelNative)
        {
            Name = name;
            ArxLevelNative = arxLevelNative;

            LevelObject = new GameObject(name);
            LevelMesh = new GameObject(name + "Mesh");
            LevelMesh.AddComponent<EditableLevelMesh>();
            LevelMesh.transform.SetParent(LevelObject.transform);
            LevelObject.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f); //1 unit is 1 cm in arx, so scale down so one unit is one meter (at least perceived)
        }
    }
}
