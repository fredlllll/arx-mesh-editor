using Assets.Scripts.ArxNative;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class EditablePrimitiveInfo
    {
        public readonly EditableVertexInfo[] vertices = new EditableVertexInfo[4];

        //additional fields that need to be kept track of
        public Vector3 norm;
        public Vector3 norm2;
        public float area;
        public short room;
        public short paddy;
        public PolyType polyType;

        public int VertexCount
        {
            get { return polyType.HasFlag(PolyType.QUAD) ? 4 : 3; }
        }

        public bool IsQuad
        {
            get { return polyType.HasFlag(PolyType.QUAD); }
        }

        public bool IsTriangle
        {
            get { return !IsQuad; }
        }

        public EditablePrimitiveInfo Copy()
        {
            var copy = new EditablePrimitiveInfo
            {
                norm = norm,
                norm2 = norm2,
                area = area,
                room = room,
                paddy = paddy,
                polyType = polyType,
            };

            for (int i = 0; i < 4; i++)
            {
                copy.vertices[i] = vertices[i].Copy();
            }

            return copy;
        }
    }
}
