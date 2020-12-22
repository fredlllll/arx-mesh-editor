using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public static class GizmoCreator
    {
        public static UnityEngine.Mesh CreateMove(GameObject moveGameObject, UnityEngine.Material gizmoMaterial)
        {
            var move = new UnityEngine.Mesh();

            List<Vector3> positions = new List<Vector3>();
            List<Color> colors = new List<Color>();
            List<int> indices = new List<int>();

            //x
            //main arrow part
            positions.Add(Vector3.zero);
            positions.Add(Vector3.right);

            //arrow tip
            positions.Add(Vector3.right);
            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.up);
            positions.Add(Vector3.right);
            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.down);
            positions.Add(Vector3.right);
            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(Vector3.right);
            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.back);

            int vertsPerArrow = positions.Count;
            for (int i = 0; i < vertsPerArrow; i++)
            {
                colors.Add(Color.red);
            }

            //y
            //main arrow part
            positions.Add(Vector3.zero);
            positions.Add(Vector3.up);

            //arrow tip
            positions.Add(Vector3.up);
            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(Vector3.up);
            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.left);
            positions.Add(Vector3.up);
            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(Vector3.up);
            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.back);


            for (int i = 0; i < vertsPerArrow; i++)
            {
                colors.Add(Color.green);
            }

            //z
            //main arrow part
            positions.Add(Vector3.zero);
            positions.Add(Vector3.forward);

            //arrow tip
            positions.Add(Vector3.forward);
            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.up);
            positions.Add(Vector3.forward);
            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.down);
            positions.Add(Vector3.forward);
            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.right);
            positions.Add(Vector3.forward);
            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.left);

            for (int i = 0; i < vertsPerArrow; i++)
            {
                colors.Add(Color.blue);
            }

            //its a simple gizmo, lets not go overboard with the index list till we have way too much time and can actually be smart about it
            for (int i = 0; i < positions.Count; i++)
            {
                indices.Add(i);
            }

            move.vertices = positions.ToArray();
            move.colors = colors.ToArray();
            move.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);


            var mf = moveGameObject.AddComponent<MeshFilter>();
            mf.sharedMesh = move;
            var mr = moveGameObject.AddComponent<MeshRenderer>();
            mr.sharedMaterial = gizmoMaterial;

            return move;
        }
        public static UnityEngine.Mesh CreateRotate(GameObject rotateGameObject, UnityEngine.Material gizmoMaterial)
        {
            var rotate = new UnityEngine.Mesh();

            List<Vector3> positions = new List<Vector3>();
            List<Color> colors = new List<Color>();
            List<int> indices = new List<int>();
            int circleSteps = 90;

            //x circle
            Vector3 currentPos = Vector3.up;
            for (int i = 1; i <= circleSteps; i++)
            {
                float perc = (float)i / circleSteps;

                float y = Mathf.Cos(perc * Mathf.PI * 2);
                float z = Mathf.Sin(perc * Mathf.PI * 2);
                indices.Add(positions.Count);
                positions.Add(currentPos);
                indices.Add(positions.Count);
                positions.Add(currentPos = new Vector3(0, y, z));
                colors.Add(Color.red);
                colors.Add(Color.red);
            }

            //y circle
            currentPos = Vector3.right;
            for (int i = 1; i <= circleSteps; i++)
            {
                float perc = (float)i / circleSteps;

                float x = Mathf.Cos(perc * Mathf.PI * 2);
                float z = Mathf.Sin(perc * Mathf.PI * 2);
                indices.Add(positions.Count);
                positions.Add(currentPos);
                indices.Add(positions.Count);
                positions.Add(currentPos = new Vector3(x, 0, z));
                colors.Add(Color.green);
                colors.Add(Color.green);
            }

            //z circle
            currentPos = Vector3.up;
            for (int i = 1; i <= circleSteps; i++)
            {
                float perc = (float)i / circleSteps;

                float y = Mathf.Cos(perc * Mathf.PI * 2);
                float x = Mathf.Sin(perc * Mathf.PI * 2);
                indices.Add(positions.Count);
                positions.Add(currentPos);
                indices.Add(positions.Count);
                positions.Add(currentPos = new Vector3(x, y, 0));
                colors.Add(Color.blue);
                colors.Add(Color.blue);
            }

            rotate.vertices = positions.ToArray();
            rotate.colors = colors.ToArray();
            rotate.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);


            var mf = rotateGameObject.AddComponent<MeshFilter>();
            mf.sharedMesh = rotate;
            var mr = rotateGameObject.AddComponent<MeshRenderer>();
            mr.sharedMaterial = gizmoMaterial;
            return rotate;
        }
        public static UnityEngine.Mesh CreateScale(GameObject scaleGameObject, UnityEngine.Material gizmoMaterial)
        {
            var scale = new UnityEngine.Mesh();

            List<Vector3> positions = new List<Vector3>();
            List<Color> colors = new List<Color>();
            List<int> indices = new List<int>();

            //x
            positions.Add(Vector3.zero);
            positions.Add(Vector3.right);

            //box
            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right - 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right - 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right - 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right - 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.up + 0.1f * Vector3.forward);

            positions.Add(1.1f * Vector3.right + 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right - 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right - 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right - 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right - 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right + 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right + 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right + 0.1f * Vector3.up + 0.1f * Vector3.forward);

            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right + 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right - 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right - 0.1f * Vector3.up + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right - 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right - 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.right + 0.1f * Vector3.up - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.right + 0.1f * Vector3.up - 0.1f * Vector3.forward);

            int vertsPerArrow = positions.Count;
            for (int i = 0; i < vertsPerArrow; i++)
            {
                colors.Add(Color.red);
            }

            //y
            positions.Add(Vector3.zero);
            positions.Add(Vector3.up);

            //box
            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up - 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up - 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up - 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up - 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.right + 0.1f * Vector3.forward);

            positions.Add(1.1f * Vector3.up + 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up - 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up - 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up - 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up - 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up + 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up + 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up + 0.1f * Vector3.right + 0.1f * Vector3.forward);

            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up + 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up - 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up - 0.1f * Vector3.right + 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up - 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up - 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(0.9f * Vector3.up + 0.1f * Vector3.right - 0.1f * Vector3.forward);
            positions.Add(1.1f * Vector3.up + 0.1f * Vector3.right - 0.1f * Vector3.forward);

            for (int i = 0; i < vertsPerArrow; i++)
            {
                colors.Add(Color.green);
            }

            //z
            positions.Add(Vector3.zero);
            positions.Add(Vector3.forward);

            //box
            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward - 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward - 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward - 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward - 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.up + 0.1f * Vector3.right);

            positions.Add(1.1f * Vector3.forward + 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward - 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward - 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward - 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward - 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward + 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward + 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward + 0.1f * Vector3.up + 0.1f * Vector3.right);

            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward + 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward - 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward - 0.1f * Vector3.up + 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward - 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward - 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(0.9f * Vector3.forward + 0.1f * Vector3.up - 0.1f * Vector3.right);
            positions.Add(1.1f * Vector3.forward + 0.1f * Vector3.up - 0.1f * Vector3.right);

            for (int i = 0; i < vertsPerArrow; i++)
            {
                colors.Add(Color.blue);
            }

            //its a simple gizmo, lets not go overboard with the index list till we have way too much time and can actually be smart about it
            for (int i = 0; i < positions.Count; i++)
            {
                indices.Add(i);
            }

            scale.vertices = positions.ToArray();
            scale.colors = colors.ToArray();
            scale.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);

            var mf = scaleGameObject.AddComponent<MeshFilter>();
            mf.sharedMesh = scale;
            var mr = scaleGameObject.AddComponent<MeshRenderer>();
            mr.sharedMaterial = gizmoMaterial;
            return scale;
        }
    }
}
