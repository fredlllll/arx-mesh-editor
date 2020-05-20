using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ArxLevel
    {
        static DirectoryInfo ftsDir = new DirectoryInfo(@"F:\Program Files\Arx Libertatis\paks\game\graph\levels");
        static DirectoryInfo dlfLlfDir = new DirectoryInfo(@"F:\Program Files\Arx Libertatis\paks\graph\levels");

        public DLF_IO.DLF_IO DLF { get; private set; }
        public LLF_IO.LLF_IO LLF { get; private set; }
        public FTS_IO.FTS_IO FTS { get; private set; }

        string name;

        public GameObject LevelObject { get; private set; }

        void LoadFiles()
        {
            var dlf = Path.Combine(dlfLlfDir.FullName, name, name + ".dlf");
            var llf = Path.Combine(dlfLlfDir.FullName, name, name + ".llf");
            var fts = Path.Combine(ftsDir.FullName, name, "fast.fts");

            //DEBUG: use unpacked versions of files for now
            dlf += ".unpacked";
            llf += ".unpacked";
            fts += ".unpacked";

            DLF = new DLF_IO.DLF_IO();
            using (FileStream fs = new FileStream(dlf, FileMode.Open, FileAccess.Read))
            {
                DLF.LoadFrom(fs);
            }

            LLF = new LLF_IO.LLF_IO();
            using (FileStream fs = new FileStream(llf, FileMode.Open, FileAccess.Read))
            {
                LLF.LoadFrom(fs);
            }

            FTS = new FTS_IO.FTS_IO();
            using (FileStream fs = new FileStream(fts, FileMode.Open, FileAccess.Read))
            {
                FTS.LoadFrom(fs);
            }
        }

        void CreateMesh()
        {
            var testMaterial = new Material(Shader.Find("Legacy Shaders/Diffuse"));
            testMaterial.color = Color.gray;

            List<GameObject> cells = new List<GameObject>();
            foreach (var cell in FTS.cells)
            {

                List<Vector3> verts = new List<Vector3>();
                List<Vector3> norms = new List<Vector3>();
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
                        }

                        indices.Add(firstVert);
                        indices.Add(firstVert + 1);
                        indices.Add(firstVert + 2);
                    }
                }

                m.vertices = verts.ToArray();
                m.triangles = indices.ToArray();
                m.normals = norms.ToArray();
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
                mr.sharedMaterial = testMaterial;

                /*int c = 0;
                foreach(var pos in m.vertices)
                {
                    GameObject helper = new GameObject();
                    helper.transform.position = pos;
                    helper.name = c.ToString();
                    c++;
                    helper.transform.SetParent(chunk.transform);
                }*/

                cells.Add(chunk);
                /*if (cnt > 30)
                {
                    break; //DEBUG: only 20 chunks for now
                }*/
            }
            GameObject lvl = new GameObject("levelMesh");

            foreach (var c in cells)
            {
                c.transform.SetParent(lvl.transform);
            }

            lvl.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f); //DEBUG, to better inspect it in the unity editor cause its so huge

            lvl.transform.SetParent(LevelObject.transform);
        }

        public void Load(string name)
        {
            this.name = name;

            LevelObject = new GameObject(name);

            LoadFiles();

            CreateMesh();
        }
    }
}
