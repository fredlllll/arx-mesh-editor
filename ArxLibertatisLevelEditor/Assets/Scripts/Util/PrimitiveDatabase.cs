using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class PrimitiveDatabase : MonoBehaviour
    {
        public static Mesh Plane { get; private set; }
        public static Mesh Cube { get; private set; }
        public static Mesh Sphere { get; private set; }
        public static Mesh Capsule { get; private set; }
        public static Mesh Cylinder { get; private set; }

        private void Awake()
        {
            GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Plane = tmp.GetComponent<MeshFilter>().sharedMesh;
            Destroy(tmp);

            tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Cube = tmp.GetComponent<MeshFilter>().sharedMesh;
            Destroy(tmp);

            tmp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Sphere = tmp.GetComponent<MeshFilter>().sharedMesh;
            Destroy(tmp);

            tmp = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Capsule = tmp.GetComponent<MeshFilter>().sharedMesh;
            Destroy(tmp);

            tmp = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Cylinder = tmp.GetComponent<MeshFilter>().sharedMesh;
            Destroy(tmp);
        }
    }
}
