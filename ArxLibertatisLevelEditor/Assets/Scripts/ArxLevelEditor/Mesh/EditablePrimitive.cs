using ArxLibertatisEditorIO.Util;
using Assets.Scripts.Util;
using cakeslice;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class EditablePrimitiveEvent : UnityEvent<EditablePrimitive> { }

    public class EditablePrimitive : MonoBehaviour
    {
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        UnityEngine.Mesh mesh;
        public EditablePrimitiveInfo info;
        EditorMaterial material;
        public EditorMaterial Material
        {
            get { return material; }
            set
            {
                material = value;
                meshRenderer.sharedMaterial = value.Material;
            }
        }

        readonly GameObject[] vertObjs = new GameObject[4];

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
                col.radius = 1;
                tmp.transform.SetParent(transform);
                tmp.layer = vertexLayer;
                var vert = tmp.AddComponent<EditableVertex>();
                vert.vertIndex = i;
                vert.primitive = this;
                //TODO: sync size with distance
                tmp.transform.localScale = new Vector3(5, 5, 5);
                var mf = tmp.AddComponent<MeshFilter>();
                mf.sharedMesh = PrimitiveDatabase.Sphere;
                var mr = tmp.AddComponent<MeshRenderer>();
                mr.sharedMaterial = MaterialsDatabase.VertexMaterial;
            }

            gameObject.AddComponent<Outline>();
        }

        public void UpdateMesh()
        {
            List<Vector3> verts = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<UnityEngine.Color> colors = new List<UnityEngine.Color>();
            List<int> indices = new List<int>();
            var center = Vector3.zero;

            int firstVert = 0;
            int vertCount = info.VertexCount;
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

            mesh.triangles = null; //prevent errors if we have less vertices than triangles currently uses
            mesh.vertices = verts.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.normals = normals.ToArray();
            mesh.colors = colors.ToArray();
            mesh.triangles = indices.ToArray();
        }

        public void UpdatePrimitive(EditablePrimitiveInfo info, EditorMaterial mat)
        {
            this.info = info;
            this.Material = mat;

            for (int i = 0; i < 4; i++)
            {
                vertObjs[i].transform.localPosition = info.vertices[i].position;
            }

            UpdateMesh();
        }

        private void PolyTypeFlagChanged(PolyType flag)
        {
            if ((flag | EditorMaterial.materialPolyTypes) != PolyType.None) //if this changes any flag of the material we have to change material
            {
                Material = new EditorMaterial(Material.TexturePath, info.polyType, Material.TransVal);
            }
            if (flag == PolyType.QUAD)
            {
                UpdateMesh();
            }
        }

        public void SetPolyTypeFlag(PolyType flag)
        {
            info.polyType |= flag;

            if (flag == PolyType.QUAD)
            {
                var vert3 = info.vertices[3];
                if (Vector3.Distance(vert3.position, Vector3.zero) < 0.001f) //if last vert is basically at 0 calculate new position
                {
                    var vert0 = info.vertices[0];
                    var vert1 = info.vertices[1];
                    var vert2 = info.vertices[2];

                    vert3.position = vert1.position + (vert2.position - vert0.position);
                    vert3.uv = vert1.uv + (vert2.uv - vert0.uv);
                    vert3.normal = (vert0.normal + vert1.normal + vert2.normal) / 3;
                    vert3.color = (vert0.color + vert1.color + vert3.color) / 3;
                    vertObjs[3].transform.localPosition = vert3.position;
                }
            }

            PolyTypeFlagChanged(flag);
        }

        public void UnsetPolyTypeFlag(PolyType flag)
        {
            info.polyType &= ~flag;
            PolyTypeFlagChanged(flag);
        }
    }
}
