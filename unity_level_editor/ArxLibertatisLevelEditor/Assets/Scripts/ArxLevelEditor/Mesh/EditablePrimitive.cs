using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public abstract class EditablePrimitive
    {
        public readonly EditableVertex[] vertices;

        //additional fields that need to be kept track of
        public Vector3 norm;
        public Vector3 norm2;
        public float area;
        public short room;
        public short paddy;

        public EditablePrimitive(byte size)
        {
            vertices = new EditableVertex[size];
        }
    }
}
