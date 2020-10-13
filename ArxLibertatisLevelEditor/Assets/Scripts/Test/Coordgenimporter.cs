using Assets.Scripts.ArxNative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Test
{
    public class Coordgenimporter : MonoBehaviour
    {
        [Serializable]
        public class Vec3
        {
            public float x, y, z;

            public Vector3 ToVector3()
            {
                return new Vector3(x, y, z);
            }
        }

        [Serializable]
        public class Col
        {
            public byte b, g, r, a;

            public Color ToColor()
            {
                return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            }
        }

        [Serializable]
        public class Vert
        {
            public float posX, posY, posZ, texU, texV;
            public Col color;

            public Vector3 GetPos()
            {
                return new Vector3(posX, posY, posZ);
            }

            public Vector2 GetUV()
            {
                return new Vector2(texU, 1 - texV);
            }
        }

        [Serializable]
        public class Poly
        {
            public Vert[] vertices;
            public int tex;
            public Vec3 norm;
            public Vec3 norm2;
            public Vec3[] normals;
            public float transval;
            public float area;
            public PolyType type;
            public int room;
            public int paddy;
        }

        [Serializable]
        public class Coordgenlist
        {
            public Poly[] polys;
        }

        [Multiline(10)]
        public string json;

        private void Start()
        {
            string tmpjson = "{\"polys\":" + json + "}";

            Debug.Log(tmpjson);
            //TODO: import stuff
            var input = JsonUtility.FromJson<Coordgenlist>(tmpjson);

            var filter = gameObject.AddComponent<MeshFilter>();
            var renderer = gameObject.AddComponent<MeshRenderer>();
            var mesh = new Mesh();

            List<Vector3> positions = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<Color> colors = new List<Color>();
            List<int> indices = new List<int>();

            foreach (var p in input.polys)
            {
                int firstVert = positions.Count;
                for (int i = 0; i < 4; i++)
                {
                    var v = p.vertices[i];
                    positions.Add(v.GetPos());
                    uvs.Add(v.GetUV());
                    normals.Add(p.normals[i].ToVector3());
                    colors.Add(v.color.ToColor());
                }


                indices.Add(firstVert);
                indices.Add(firstVert + 1);
                indices.Add(firstVert + 2);
                indices.Add(firstVert + 2);
                indices.Add(firstVert + 1);
                indices.Add(firstVert + 3);
            }

            mesh.vertices = positions.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.normals = normals.ToArray();
            mesh.colors = colors.ToArray();
            mesh.triangles = indices.ToArray();

            filter.sharedMesh = mesh;

            renderer.material = MaterialsDatabase.ArxMat;
        }
    }
}
