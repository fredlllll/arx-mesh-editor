using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class EditableVertexInfo
    {
        public Vector3 position;
        public Vector2 uv;
        public Vector3 normal;
        public Color color;

        public EditableVertexInfo(Vector3 pos, Vector2 uv, Vector3 norm, Color color)
        {
            this.position = pos;
            this.uv = uv;
            this.normal = norm;
            this.color = color;
        }

        public EditableVertexInfo Copy()
        {
            return new EditableVertexInfo(position, uv, normal, color);
        }
    }
}
