using Assets.Scripts.ArxNative;

namespace Assets.Scripts.ArxLevelEditor.Material
{
    public abstract class EditorMaterialBase
    {
        public string TexturePath
        {
            get;
            protected set;
        }

        public PolyType PolygonType
        {
            get;
            protected set;
        }

        public float TransVal
        {
            get;
            protected set;
        }
    }
}
