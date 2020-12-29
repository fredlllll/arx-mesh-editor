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

        public float Snap(float val)
        {
            return Mathf.Round(val / Size) * Size;
        }

        public Vector3 Snap(Vector3 val)
        {
            float x = Snap(val.x);
            float y = Snap(val.y);
            float z = Snap(val.z);
            return new Vector3(x, y, z);
        }
    }
}
