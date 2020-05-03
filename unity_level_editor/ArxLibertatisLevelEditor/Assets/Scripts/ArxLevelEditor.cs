using Assets.Scripts.DLF;
using Assets.Scripts.FTS;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class ArxLevelEditor : MonoBehaviour
{
    public Material testMaterial;

    public void OpenLevel()
    {
        var dlf = new DLF();
        using (FileStream fs = new FileStream(@"F:\Program Files\Arx Libertatis\paks\graph\levels\level0\level0.dlf", FileMode.Open, FileAccess.Read))
        {
            dlf.LoadFrom(fs);
        }

        var fts = new FTS();
        using (FileStream fs = new FileStream(@"F:\Program Files\Arx Libertatis\paks\game\graph\levels\level0\fast.fts", FileMode.Open, FileAccess.Read))
        {
            fts.LoadFrom(fs);
        }

        //DEBUG: making a mesh from fts

        List<GameObject> chunks = new List<GameObject>();
        foreach (var sceneChunk in fts.fepss)
        {
            
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> norms = new List<Vector3>();
            List<int> indices = new List<int>();
            Mesh m = new Mesh();

            foreach (var poly in sceneChunk)
            {
                int firstVert = verts.Count;
                if ((poly.type & PolyType.POLY_QUAD) != 0)
                {
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
                    /*indices.Add(firstVert);
                       indices.Add(firstVert + 1);
                       indices.Add(firstVert + 2);
                       indices.Add(firstVert + 3);*/
                }
                else
                {
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

            chunks.Add(chunk);
            /*if (cnt > 30)
            {
                break; //DEBUG: only 20 chunks for now
            }*/
        }
        GameObject lvl = new GameObject();

        foreach (var c in chunks)
        {
            c.transform.SetParent(lvl.transform);
        }

        lvl.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f);
    }
}
