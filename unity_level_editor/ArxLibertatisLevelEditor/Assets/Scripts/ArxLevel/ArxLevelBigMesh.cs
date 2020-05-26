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

            Material[] materials = new Material[fts.textureContainers.Length];
            Dictionary<int, int> tcToIndex = new Dictionary<int, int>();
            for (int i = 0; i < materials.Length; i++)
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

            List<Vector3>[] subMeshVerts = new List<Vector3>[fts.textureContainers.Length];
            List<Vector3>[] subMeshNorms = new List<Vector3>[subMeshVerts.Length];
            List<Color>[] subMeshColors = new List<Color>[subMeshVerts.Length];
            List<int>[] subMeshIndices = new List<int>[subMeshVerts.Length];
            for (int i = 0; i < subMeshVerts.Length; i++)
            {
                subMeshVerts[i] = new List<Vector3>();
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
                    }

                    var verts = subMeshVerts[ind];
                    var norms = subMeshNorms[ind];
                    var colors = subMeshColors[ind];
                    var indices = subMeshIndices[ind];

                    int firstVert = verts.Count;
                    if ((poly.type & PolyType.POLY_QUAD) != 0)
                    { //QUAD
                        for (int i = 0; i < 4; i++)
                        {
                            verts.Add(new Vector3(poly.vertices[i].posX, poly.vertices[i].posY, poly.vertices[i].posZ));
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
                            verts.Add(new Vector3(poly.vertices[i].posX, poly.vertices[i].posY, poly.vertices[i].posZ));
                            norms.Add(poly.normals[i].ToVector3());
                            colors.Add(ArxIOHelper.FromBGRA(llf.lightColors[lightIndex++]));
                        }

                        indices.Add(firstVert);
                        indices.Add(firstVert + 1);
                        indices.Add(firstVert + 2);
                    }
                }
            }

            Mesh m = new Mesh();
            List<Vector3> meshVerts = new List<Vector3>();
            List<Vector3> meshNorms = new List<Vector3>();
            List<Color> meshColors = new List<Color>();
            List<int> meshIndices = new List<int>();
            m.subMeshCount = subMeshVerts.Length;
            for (int i = 0; i < subMeshVerts.Length; i++)
            {
                int indexOffset = meshVerts.Count;

                var verts = subMeshVerts[i];
                var norms = subMeshNorms[i];
                var colors = subMeshColors[i];
                var indices = subMeshIndices[i];

                SubMeshDescriptor smd = new SubMeshDescriptor(meshIndices.Count, indices.Count, MeshTopology.Triangles);
                //m.SetSubMesh(i, smd);

                for (int j = 0; j < verts.Count; j++)
                {
                    meshVerts.Add(verts[j]);
                    meshNorms.Add(norms[j]);
                    meshColors.Add(colors[j]);
                    meshIndices.Add(indices[j] + indexOffset);
                }
            }

            m.vertices = meshVerts.ToArray();
            m.normals = meshNorms.ToArray();
            m.colors = meshColors.ToArray();
            m.triangles = meshIndices.ToArray();

            GameObject lvl = new GameObject(level.Name + "_mesh");
            var mf = lvl.AddComponent<MeshFilter>();
            mf.sharedMesh = m;
            var mr = lvl.AddComponent<MeshRenderer>();
            mr.sharedMaterials = materials;
            m.RecalculateBounds();
            m.RecalculateTangents();
            m.Optimize();

            lvl.transform.SetParent(level.LevelObject.transform);
            lvl.transform.localPosition = Vector3.zero;
            lvl.transform.localEulerAngles = Vector3.zero;
            lvl.transform.localScale = Vector3.one;
        }
    }
}
