using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class SnapGrid
    {
        public float Size
        {
            get;
            set;
        } = 0.05f;

        public Vector3 Snap(Vector3 val)
        {
            float x = Mathf.Round(val.x / Size) * Size;
            float y = Mathf.Round(val.y / Size) * Size;
            float z = Mathf.Round(val.z / Size) * Size;
            return new Vector3(x, y, z);
        }
    }
}
