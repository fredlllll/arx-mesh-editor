using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    [Flags]
    public enum SnapAxis
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4,
        All = X | Y | Z,
    }

    public enum SnapMode
    {
        //numbers are important so it can directly convert from index in snap mode dropdown
        None = 0,
        Grid = 1,
        Vertex = 2,
    }

    public class SnapManager
    {
        SnapGrid snapGrid = new SnapGrid();
        public float SnapGridSize
        {
            get { return snapGrid.Size; }
            set
            {
                snapGrid.Size = Math.Max(0.00001f, value);//no negative and no 0 
            }
        }

        public SnapMode SnapMode { get; set; } = SnapMode.None;

        private Vector3 SnapVertex(Vector3 pos)
        {
            var colliders = Physics.OverlapSphere(pos, 0.1f, PolygonSelector.PolygonsLayerMask);
            if (colliders.Length > 0)
            {
                float closestDistance = float.MaxValue;
                Vector3 closestVertex = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                foreach (var col in colliders)
                {
                    var vertices = col.gameObject.GetComponent<MeshFilter>().sharedMesh.vertices;
                    var mat = col.transform.localToWorldMatrix;
                    foreach (var v in vertices)
                    {
                        Vector3 vert = mat.MultiplyPoint(v);
                        float dist = Vector3.Distance(vert, pos);
                        if (dist < closestDistance)
                        {
                            closestDistance = dist;
                            closestVertex = vert;
                        }
                    }
                }
                if (closestDistance < 0.1f)
                {
                    return closestVertex;
                }
            }
            return pos;
        }

        public Vector3 Snap(Vector3 worldPos, SnapAxis snapAxis = SnapAxis.All)
        {
            Vector3 snapped = worldPos;
            switch (SnapMode)
            {
                case SnapMode.Grid:
                    snapped = snapGrid.Snap(worldPos);
                    break;
                case SnapMode.Vertex:
                    snapped = SnapVertex(worldPos);
                    snapAxis = SnapAxis.All; //vertices are 3d positions so it doesnt make sense to snap to only one axis
                    break;
            }


            if (snapAxis.HasFlag(SnapAxis.X))
            {
                worldPos.x = snapped.x;
            }
            if (snapAxis.HasFlag(SnapAxis.Y))
            {
                worldPos.y = snapped.y;
            }
            if (snapAxis.HasFlag(SnapAxis.Z))
            {
                worldPos.z = snapped.z;
            }

            return worldPos;
        }
    }
}
