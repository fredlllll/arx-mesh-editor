using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class MaterialMesh : MonoBehaviour
    {
        UnityEngine.Mesh mesh;
        MeshFilter filter;
        MeshRenderer meshRenderer;

        public UnityEngine.Material Material
        {
            get { return meshRenderer.sharedMaterial; }
            set
            {
                meshRenderer.sharedMaterial = value;
                if (value != null) { gameObject.name = value.name; }
            }
        }

        readonly List<EditablePrimitive> primitives = new List<EditablePrimitive>();

        readonly List<Vector3> verts = new List<Vector3>();
        readonly List<Vector2> uvs = new List<Vector2>();
        readonly List<Vector3> normals = new List<Vector3>();
        readonly List<Color> colors = new List<Color>();
        readonly List<int> indices = new List<int>();
        readonly Dictionary<int, EditablePrimitive> indexToPrimitive = new Dictionary<int, EditablePrimitive>();

        private void Awake()
        {
            mesh = new UnityEngine.Mesh();
            filter = gameObject.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        public void AddPrimitive(EditablePrimitive primitive)
        {
            primitives.Add(primitive);
        }

        public void RemovePrimitive(EditablePrimitive primitive)
        {
            primitives.Remove(primitive);
        }

        private void ProcessPrimitive(EditablePrimitive primitive)
        {
            int firstVert = verts.Count;
            for (int i = 0; i < primitive.vertices.Length; i++)
            {
                var vert = primitive.vertices[i];
                verts.Add(vert.position);
                uvs.Add(vert.uv);
                normals.Add(vert.normal);
                colors.Add(vert.color);
            }

            int firstIndex = indices.Count;

            indices.Add(firstVert);
            indexToPrimitive[firstIndex++] = primitive;
            indices.Add(firstVert + 1);
            indexToPrimitive[firstIndex++] = primitive;
            indices.Add(firstVert + 2);
            indexToPrimitive[firstIndex++] = primitive;
            if (primitive.vertices.Length > 3)
            {
                indices.Add(firstVert + 2);
                indexToPrimitive[firstIndex++] = primitive;
                indices.Add(firstVert + 1);
                indexToPrimitive[firstIndex++] = primitive;
                indices.Add(firstVert + 3);
                indexToPrimitive[firstIndex++] = primitive;
            }
        }

        public void UpdateMesh()
        {
            verts.Clear();
            uvs.Clear();
            normals.Clear();
            colors.Clear();
            indices.Clear();
            indexToPrimitive.Clear();

            foreach (var ep in primitives)
            {
                ProcessPrimitive(ep);
            }

            mesh.vertices = verts.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.normals = normals.ToArray();
            mesh.colors = colors.ToArray();
            mesh.triangles = indices.ToArray();
        }
    }
}
