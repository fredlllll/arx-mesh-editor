using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class EditableVertexEvent : UnityEvent<EditableVertex> { }

    public class EditableVertex : MonoBehaviour
    {
        public EditablePrimitive primitive;
        public int vertIndex;

        private void Update()
        {
            Vector3 pos = transform.localPosition;
            if (pos != primitive.info.vertices[vertIndex].position)
            {
                primitive.info.vertices[vertIndex].position = pos;
                primitive.UpdateMesh();
            }
        }
    }
}
