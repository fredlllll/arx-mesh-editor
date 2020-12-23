using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class EditablePrimitive : MonoBehaviour
    {
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        UnityEngine.Mesh mesh;
        EditablePrimitiveInfo info;
        UnityEngine.Material material;

        private void Awake()
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            mesh = new UnityEngine.Mesh();
            meshFilter.sharedMesh = mesh;
        }

        public void UpdatePrimitive(EditablePrimitiveInfo info, UnityEngine.Material mat)
        {
            this.info = info;
            this.material = mat;
            this.meshRenderer.sharedMaterial = mat;

            List<Vector3> verts = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<Color> colors = new List<Color>();
            List<int> indices = new List<int>();

            int firstVert = 0;
            int vertCount = info.vertexCount;
            for (int i = 0; i < vertCount; i++)
            {
                var vert = info.vertices[i];
                verts.Add(vert.position);
                uvs.Add(vert.uv);
                normals.Add(vert.normal);
                colors.Add(vert.color);
            }

            indices.Add(firstVert);
            indices.Add(firstVert + 1);
            indices.Add(firstVert + 2);
            if (vertCount > 3)
            {
                indices.Add(firstVert + 2);
                indices.Add(firstVert + 1);
                indices.Add(firstVert + 3);
            }

            mesh.vertices = verts.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.normals = normals.ToArray();
            mesh.colors = colors.ToArray();
            mesh.triangles = indices.ToArray();
        }
    }
}
