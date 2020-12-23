using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class EditableVertex :MonoBehaviour
    {
        public EditablePrimitive primitive;
        public int vertIndex;

        private void Update()
        {
            Vector3 pos = transform.localPosition;
            if(pos != primitive.info.vertices[vertIndex].position)
            {
                primitive.info.vertices[vertIndex].position = pos;
                primitive.UpdateMesh();
            }
        }
    }
}
