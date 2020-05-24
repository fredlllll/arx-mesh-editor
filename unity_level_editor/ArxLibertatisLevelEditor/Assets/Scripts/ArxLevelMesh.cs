using Assets.Scripts.Data;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ArxLevelMesh
    {
        ArxLevel level;

        public ArxLevelMesh(ArxLevel level)
        {
            this.level = level;
        }

        public void CreateMesh(FTS_IO.FTS_IO fts, LLF_IO.LLF_IO llf)
        {
            var levelMat = MaterialsDatabase.ArxLevelBackground; //TODO: make one for each texture used...

            List<GameObject> cells = new List<GameObject>();
            int lightIndex = 0;
            foreach (var cell in fts.cells)
            {
                List<Vector3> verts = new List<Vector3>();
                List<Vector3> norms = new List<Vector3>();
                List<Color> colors = new List<Color>();
                List<int> indices = new List<int>();
                Mesh m = new Mesh();

                foreach (var poly in cell.polygons)
                {
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

                m.vertices = verts.ToArray();
                m.triangles = indices.ToArray();
                m.normals = norms.ToArray();
                m.colors = colors.ToArray();

                //SubMeshDescriptor smd = new SubMeshDescriptor(0, indices.Count, MeshTopology.Quads);
                //m.SetSubMesh(0, smd);

                //m.RecalculateNormals();
                m.RecalculateBounds();
                m.RecalculateTangents();
                m.Optimize();

                GameObject chunk = new GameObject();
                var mf = chunk.AddComponent<MeshFilter>();
                mf.sharedMesh = m;
                var mr = chunk.AddComponent<MeshRenderer>();

                mr.sharedMaterial = levelMat; //TODO: depending on poly.tex

                cells.Add(chunk);
            }
            GameObject lvl = new GameObject(level.Name + "_mesh");

            foreach (var c in cells)
            {
                c.transform.SetParent(lvl.transform);
            }

            lvl.transform.SetParent(level.LevelObject.transform);
            lvl.transform.localPosition = fts.sceneHeader.Mscenepos.ToVector3();
            lvl.transform.localScale = new Vector3(1,-1,1); //y-1 cause its flipped otherwise??
        }
    }
}
