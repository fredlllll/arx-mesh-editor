using Assets.Scripts.ArxLevelEditor.Material;
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
        public EditablePrimitiveInfo info;
        public EditorMaterial material;

        GameObject[] vertObjs = new GameObject[4];

        public Vector3 Center
        {
            get;
            private set;
        }

        private void Awake()
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            mesh = new UnityEngine.Mesh();
            meshFilter.sharedMesh = mesh;

            var vertexLayer = LayerMask.NameToLayer("Vertices");

            for (int i = 0; i < vertObjs.Length; i++)
            {
                var tmp = vertObjs[i] = new GameObject();
                var col = tmp.AddComponent<SphereCollider>();
                col.radius = 0.2f;
                tmp.transform.SetParent(transform);
                tmp.layer = vertexLayer;
                var vert = tmp.AddComponent<EditableVertex>();
                vert.vertIndex = i;
                vert.primitive = this;
            }
        }

        public void UpdateMesh()
        {
            List<Vector3> verts = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<Color> colors = new List<Color>();
            List<int> indices = new List<int>();
            var center = Vector3.zero;

            int firstVert = 0;
            int vertCount = info.vertexCount;
            for (int i = 0; i < vertCount; i++)
            {
                var vert = info.vertices[i];
                center += vert.position;
                verts.Add(vert.position);
                uvs.Add(vert.uv);
                normals.Add(vert.normal);
                colors.Add(vert.color);
            }
            Center = center / vertCount;

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

        public void UpdatePrimitive(EditablePrimitiveInfo info, EditorMaterial mat)
        {
            this.info = info;
            this.material = mat;
            this.meshRenderer.sharedMaterial = mat.Material;

            int vertCount = info.vertexCount;
            for (int i = 0; i < vertCount; i++)
            {
                vertObjs[i].transform.localPosition = info.vertices[i].position;
            }

            UpdateMesh();
        }
    }
}
