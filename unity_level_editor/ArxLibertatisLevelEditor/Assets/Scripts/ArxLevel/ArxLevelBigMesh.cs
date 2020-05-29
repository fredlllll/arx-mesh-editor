using Assets.Scripts.Data;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.ArxLevel
{
    /// <summary>
    /// the mesh the user will see, cause rendering all the single cells is hard on drawcalls
    /// </summary>
    public class ArxLevelBigMesh
    {
        class SubMeshData
        {
            public Material material;

            public List<Vector3> verts = new List<Vector3>();
            public List<Vector2> uvs = new List<Vector2>();
            public List<Vector3> norms = new List<Vector3>();
            public List<Color> colors = new List<Color>();
            public List<int> indices = new List<int>();

            public SubMeshData(Material material)
            {
                this.material = material;
            }
        }

        readonly ArxLevel level;

        public ArxLevelBigMesh(ArxLevel level)
        {
            this.level = level;
        }

        public void CreateMesh()
        {
            var fts = level.FTS;
            var llf = level.LLF;

            Dictionary<ArxMaterialManager.ArxMaterialKey, SubMeshData> subMeshes = new Dictionary<ArxMaterialManager.ArxMaterialKey, SubMeshData>();
            SubMeshData notFoundSubMesh = new SubMeshData(MaterialsDatabase.NotFound);
            subMeshes[new ArxMaterialManager.ArxMaterialKey("", PolyType.GLOW)] = notFoundSubMesh; //so we can use it in a for over subMeshes later

            string[] texArxPaths = new string[fts.textureContainers.Length];
            Dictionary<int, int> tcToIndex = new Dictionary<int, int>();
            for (int i = 0; i < fts.textureContainers.Length; i++)
            {
                texArxPaths[i] = ArxIOHelper.GetString(fts.textureContainers[i].fic);
                tcToIndex[fts.textureContainers[i].tc] = i;
            }


            int lightIndex = 0;
            for (int c = 0; c < fts.cells.Length; c++)
            {
                var cell = fts.cells[c];
                for (int p = 0; p < cell.polygons.Length; p++)
                {
                    var poly = cell.polygons[p];

                    SubMeshData subMesh;
                    if (tcToIndex.TryGetValue(poly.tex, out int textureIndex))
                    {
                        var key = new ArxMaterialManager.ArxMaterialKey(texArxPaths[textureIndex], poly.type);
                        if (!subMeshes.TryGetValue(key, out subMesh))
                        {
                            subMesh = new SubMeshData(ArxMaterialManager.GetArxLevelMaterial(texArxPaths[textureIndex], poly.type));
                            subMeshes[key] = subMesh;
                        }
                    }
                    else
                    {
                        if (poly.tex != 0)
                        {
                            Debug.Log("not found: " + poly.tex);
                        }
                        subMesh = notFoundSubMesh; //use not found submesh
                    }

                    var verts = subMesh.verts;
                    var uvs = subMesh.uvs;
                    var norms = subMesh.norms;
                    var colors = subMesh.colors;
                    var indices = subMesh.indices;

                    int firstVert = verts.Count;
                    if (poly.type.HasFlag(PolyType.QUAD))
                    { //QUAD
                        for (int i = 0; i < 4; i++)
                        {
                            var vert = poly.vertices[i];
                            verts.Add(new Vector3(vert.posX, vert.posY, vert.posZ));
                            uvs.Add(new Vector2(vert.texU, 1 - vert.texV));
                            norms.Add(poly.normals[i].ToVector3());
                            colors.Add(ArxIOHelper.FromBGRA(llf.lightColors[lightIndex++]));
                        }

                        indices.Add(firstVert);
                        indices.Add(firstVert + 1);
                        indices.Add(firstVert + 2);
                        indices.Add(firstVert + 2);
                        indices.Add(firstVert + 1);
                        indices.Add(firstVert + 3);
                    }
                    else
                    { //TRIANGLE
                        for (int i = 0; i < 3; i++)
                        {
                            var vert = poly.vertices[i];
                            verts.Add(new Vector3(vert.posX, vert.posY, vert.posZ));
                            uvs.Add(new Vector2(vert.texU, 1 - vert.texV));
                            norms.Add(poly.normals[i].ToVector3());
                            colors.Add(ArxIOHelper.FromBGRA(llf.lightColors[lightIndex++]));
                        }

                        indices.Add(firstVert);
                        indices.Add(firstVert + 1);
                        indices.Add(firstVert + 2);
                    }
                }
            }

            GameObject lvl = new GameObject(level.Name + "_mesh");

            /*List<Vector3> meshVerts = new List<Vector3>();
            List<Vector2> meshUvs = new List<Vector2>();
            List<Vector3> meshNorms = new List<Vector3>();
            List<Color> meshColors = new List<Color>();
            for (int i = 0; i < subMeshVerts.Length; i++)
            {
                int indexOffset = meshVerts.Count;

                var verts = subMeshVerts[i];
                var uvs = subMeshUvs[i];
                var norms = subMeshNorms[i];
                var colors = subMeshColors[i];
                var indices = subMeshIndices[i];

                for (int j = 0; j < verts.Count; j++)
                {
                    meshVerts.Add(verts[j]);
                    meshUvs.Add(uvs[j]);
                    meshNorms.Add(norms[j]);
                    meshColors.Add(colors[j]);
                    indices[j] += indexOffset;
                }
            }

            Mesh m = new Mesh();
            m.vertices = meshVerts.ToArray();
            m.uv = meshUvs.ToArray();
            m.normals = meshNorms.ToArray();
            m.colors = meshColors.ToArray();

            m.subMeshCount = materials.Length;
            for (int i = 0; i < materials.Length; i++)
            {
                var indices = subMeshIndices[i];
                m.SetIndices(indices.ToArray(), MeshTopology.Triangles, i);
            }

            var mf = lvl.AddComponent<MeshFilter>();
            mf.sharedMesh = m;
            var mr = lvl.AddComponent<MeshRenderer>();
            mr.sharedMaterials = materials;
            m.RecalculateBounds();
            m.RecalculateTangents();
            m.Optimize();
            */

            foreach (var kv in subMeshes)
            {
                var subMesh = kv.Value;

                GameObject matObj = new GameObject();

                Mesh m = new Mesh();

                m.vertices = subMesh.verts.ToArray();
                m.uv = subMesh.uvs.ToArray();
                m.normals = subMesh.norms.ToArray();
                m.colors = subMesh.colors.ToArray();
                m.triangles = subMesh.indices.ToArray();

                var mf = matObj.AddComponent<MeshFilter>();
                mf.sharedMesh = m;
                var mr = matObj.AddComponent<MeshRenderer>();
                mr.sharedMaterial = subMesh.material;

                matObj.transform.SetParent(lvl.transform);
            }

            lvl.transform.SetParent(level.LevelObject.transform);
            lvl.transform.localPosition = Vector3.zero;
            lvl.transform.localEulerAngles = Vector3.zero;
            lvl.transform.localScale = Vector3.one;
        }
    }
}
