using Assets.Scripts.ArxLevelEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class MaterialMesh : MonoBehaviour
    {
        UnityEngine.Mesh mesh;
        UnityEngine.Mesh collisionMesh;
        MeshFilter filter;
        MeshRenderer meshRenderer;
        MeshCollider meshCollider;
        EditorMaterial mat;

        public EditorMaterial EditorMaterial
        {
            get { return mat; }
            set
            {
                mat = value;
                if (value == null)
                {
                    gameObject.name = "NULL";
                    meshRenderer.sharedMaterial = null;
                }
                else
                {
                    gameObject.name = value.Material.name;
                    meshRenderer.sharedMaterial = value.Material;
                }
            }
        }

        public int PrimitiveCount
        {
            get
            {
                return primitives.Count;
            }
        }

        public IEnumerable<EditablePrimitiveInfo> Primitives
        {
            get { return primitives; }
        }

        readonly List<EditablePrimitiveInfo> primitives = new List<EditablePrimitiveInfo>();

        readonly List<Vector3> verts = new List<Vector3>();
        readonly List<Vector2> uvs = new List<Vector2>();
        readonly List<Vector3> normals = new List<Vector3>();
        readonly List<Color> colors = new List<Color>();
        readonly List<int> indices = new List<int>();
        readonly Dictionary<int, EditablePrimitiveInfo> triangleIndexToPrimitive = new Dictionary<int, EditablePrimitiveInfo>();

        private void Awake()
        {
            mesh = new UnityEngine.Mesh();
            collisionMesh = new UnityEngine.Mesh();
            filter = gameObject.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = collisionMesh;
        }

        public void AddPrimitive(EditablePrimitiveInfo primitive)
        {
            primitives.Add(primitive);
        }

        public void RemovePrimitive(EditablePrimitiveInfo primitive)
        {
            primitives.Remove(primitive);
        }

        private void ProcessPrimitive(EditablePrimitiveInfo primitive)
        {
            int firstVert = verts.Count;
            int vertCount = primitive.vertexCount;
            for (int i = 0; i < vertCount; i++)
            {
                var vert = primitive.vertices[i];
                verts.Add(vert.position);
                uvs.Add(vert.uv);
                normals.Add(vert.normal);
                colors.Add(vert.color);
            }

            int triIndex = indices.Count / 3;
            triangleIndexToPrimitive[triIndex++] = primitive;
            indices.Add(firstVert);
            indices.Add(firstVert + 1);
            indices.Add(firstVert + 2);
            if (vertCount > 3)
            {
                triangleIndexToPrimitive[triIndex] = primitive;
                indices.Add(firstVert + 2);
                indices.Add(firstVert + 1);
                indices.Add(firstVert + 3);
            }
        }

        public void UpdateMesh()
        {
            verts.Clear();
            uvs.Clear();
            normals.Clear();
            colors.Clear();
            indices.Clear();
            triangleIndexToPrimitive.Clear();

            foreach (var ep in primitives)
            {
                ProcessPrimitive(ep);
            }

            mesh.triangles = null; //to prevent out of bounds issues when vertices is smaller than before
            mesh.vertices = verts.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.normals = normals.ToArray();
            mesh.colors = colors.ToArray();
            mesh.triangles = indices.ToArray();

            var meshTris = mesh.triangles; //the getter of triangles is slow, cache its result here
            int[] normalPlusReverseIndices = new int[meshTris.Length * 2];
            Array.Copy(meshTris, normalPlusReverseIndices, meshTris.Length);
            for (int i = meshTris.Length, j = meshTris.Length - 1; i < normalPlusReverseIndices.Length; i++, j--)
            {
                normalPlusReverseIndices[i] = meshTris[j];
            }

            collisionMesh.triangles = null;
            collisionMesh.vertices = mesh.vertices;
            collisionMesh.triangles = normalPlusReverseIndices;
            meshCollider.sharedMesh = collisionMesh;
        }

        public EditablePrimitiveInfo GetByTriangleIndex(int triangleIndex)
        {
            int triCount = indices.Count / 3;
            if (triangleIndex >= triCount)
            {
                //backface collision
                triangleIndex = triCount - (triangleIndex - triCount);
            }
            if (!triangleIndexToPrimitive.TryGetValue(triangleIndex, out EditablePrimitiveInfo retval))
            {
                return null;
            }
            return retval;
        }
    }
}
