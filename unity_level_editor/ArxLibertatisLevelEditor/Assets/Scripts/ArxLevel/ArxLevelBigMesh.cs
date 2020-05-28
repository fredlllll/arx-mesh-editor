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
        readonly ArxLevel level;

        public ArxLevelBigMesh(ArxLevel level)
        {
            this.level = level;
        }

        public void CreateMesh()
        {
            var fts = level.FTS;
            var llf = level.LLF;

            Material[] materials = new Material[fts.textureContainers.Length + 1];
            Dictionary<int, int> tcToIndex = new Dictionary<int, int>();
            for (int i = 0; i < fts.textureContainers.Length; i++)
            {
                string matPath = Path.Combine(ArxDirs.DataDir, ArxIOHelper.GetString(fts.textureContainers[i].fic));
                matPath = matPath.Substring(0, matPath.Length - 3);
                if (File.Exists(matPath + "jpg"))
                {
                    matPath += "jpg";
                }
                else if (File.Exists(matPath + "bmp"))
                {
                    matPath += "bmp";
                }
                materials[i] = ArxLevelMeshMaterials.GetMaterial(matPath);
                tcToIndex[fts.textureContainers[i].tc] = i;
            }

            //add material for tex = 0
            materials[materials.Length - 1] = MaterialsDatabase.NotFound;
            tcToIndex[0] = materials.Length - 1;

            List<Vector3>[] subMeshVerts = new List<Vector3>[materials.Length];
            List<Vector2>[] subMeshUvs = new List<Vector2>[materials.Length];
            List<Vector3>[] subMeshNorms = new List<Vector3>[materials.Length];
            List<Color>[] subMeshColors = new List<Color>[materials.Length];
            List<int>[] subMeshIndices = new List<int>[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                subMeshVerts[i] = new List<Vector3>();
                subMeshUvs[i] = new List<Vector2>();
                subMeshNorms[i] = new List<Vector3>();
                subMeshColors[i] = new List<Color>();
                subMeshIndices[i] = new List<int>();
            }

            int lightIndex = 0;
            for (int c = 0; c < fts.cells.Length; c++)
            {
                var cell = fts.cells[c];
                for (int p = 0; p < cell.polygons.Length; p++)
                {
                    var poly = cell.polygons[p];

                    var ind = 0;
                    if (!tcToIndex.TryGetValue(poly.tex, out ind))
                    {
                        Debug.Log("not found: " + poly.tex);
                        ind = materials.Length - 1; //use the not found material, which is last in materials
                    }

                    var verts = subMeshVerts[ind];
                    var uvs = subMeshUvs[ind];
                    var norms = subMeshNorms[ind];
                    var colors = subMeshColors[ind];
                    var indices = subMeshIndices[ind];

                    int firstVert = verts.Count;
                    if ((poly.type & PolyType.POLY_QUAD) != 0)
                    { //QUAD
                        for (int i = 0; i < 4; i++)
                        {
                            var vert = poly.vertices[i];
                            verts.Add(new Vector3(vert.posX, vert.posY, vert.posZ));
                            uvs.Add(new Vector2(vert.texU, -vert.texV));
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
                            uvs.Add(new Vector2(vert.texU, vert.texV));
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

            for (int i = 0; i < materials.Length; i++)
            {
                GameObject matObj = new GameObject();

                Mesh m = new Mesh();

                m.vertices = subMeshVerts[i].ToArray();
                m.uv = subMeshUvs[i].ToArray();
                m.normals = subMeshNorms[i].ToArray();
                m.colors = subMeshColors[i].ToArray();
                m.triangles = subMeshIndices[i].ToArray();

                var mf = matObj.AddComponent<MeshFilter>();
                mf.sharedMesh = m;
                var mr = matObj.AddComponent<MeshRenderer>();
                mr.sharedMaterial = materials[i];

                matObj.transform.SetParent(lvl.transform);
            }

            lvl.transform.SetParent(level.LevelObject.transform);
            lvl.transform.localPosition = Vector3.zero;
            lvl.transform.localEulerAngles = Vector3.zero;
            lvl.transform.localScale = Vector3.one;
        }
    }
}
