using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public enum EditablePrimitiveType
    {
        Triangle,
        Quad,
    }

    public class EditablePrimitiveInfo
    {
        public EditablePrimitiveType type = EditablePrimitiveType.Quad;
        public readonly EditableVertexInfo[] vertices = new EditableVertexInfo[4];

        //additional fields that need to be kept track of
        public Vector3 norm;
        public Vector3 norm2;
        public float area;
        public short room;
        public short paddy;

        public int vertexCount
        {
            get { return type == EditablePrimitiveType.Quad ? 4 : 3; }
        }
    }
}
