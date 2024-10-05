using ArxLibertatisEditorIO.RawIO.FTL;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class FTLExtensions
    {
        public static Mesh CreateMesh(this FTL_IO ftl)
        {
            Mesh m = new Mesh();

            if (ftl.has3DDataSection)
            {
                Vector3[] verts = new Vector3[ftl._3DDataSection.vertexList.Length];
                Vector3[] norms = new Vector3[verts.Length];
                Vector2[] uvs = new Vector2[verts.Length];
                Color[] colors = new Color[verts.Length]; //blender plugin says always 0, skip for now cause i dunno if its argb, or rgba

                //TODO: basically this is using the faces list to create seperate faces. i have to copy the vertex for the faces because the normals & uv are redefined in every face

                List<int> indices = new List<int>();

                for (int i = 0; i < verts.Length; i++)
                {
                    var vert = ftl._3DDataSection.vertexList[i];
                    verts[i] = vert.vert.ToVector3().ToUnity();
                    norms[i] = vert.norm.ToVector3().ToUnity();
                }

                foreach (var face in ftl._3DDataSection.faceList)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        ushort vertIndex = face.vid[i];

                        uvs[vertIndex] = new Vector2(face.u[i], 1 - face.v[i]);
                        indices.Add(vertIndex);
                    }
                }

                m.vertices = verts;
                m.triangles = indices.ToArray();
                m.normals = norms;
                m.uv = uvs;

                m.RecalculateBounds();
                m.RecalculateTangents();
                m.Optimize();
            }

            return m;
        }
    }
}
