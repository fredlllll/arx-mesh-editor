using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevel
{
    public class SubMeshData
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
}
