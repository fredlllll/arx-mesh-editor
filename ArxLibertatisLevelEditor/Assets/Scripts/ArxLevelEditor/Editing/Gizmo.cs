using Assets.Scripts;
using Assets.Scripts.ArxLevel.Mesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Scripting.Pipeline;

public class Gizmo : MonoBehaviour
{
    GameObject moveGameObject, rotateGameObject, scaleGameObject;
    Mesh move, rotate, scale;

    Material gizmoMaterial;

    void CreateMoveMesh()
    {
        move = new Mesh();

        List<Vector3> positions = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> indices = new List<int>();

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

        int vertsPerArrow = positions.Count;
        for (int i = 0; i < vertsPerArrow; i++)
        {
            colors.Add(Color.green);
        }

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

        for (int i = 0; i < vertsPerArrow; i++)
        {
            colors.Add(Color.red);
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

        moveGameObject = new GameObject();
        moveGameObject.transform.SetParent(transform);
        moveGameObject.transform.localPosition = Vector3.zero;
        var mf = moveGameObject.AddComponent<MeshFilter>();
        mf.sharedMesh = move;
        var mr = moveGameObject.AddComponent<MeshRenderer>();
        mr.sharedMaterial = gizmoMaterial;
    }

    void CreateMeshes()
    {
        gizmoMaterial = Instantiate(MaterialsDatabase.GizmoMaterial); //copy material so we can modify it

        CreateMoveMesh();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateMeshes();
    }

    // Update is called once per frame
    void Update()
    {
        //Do raycasting from mouse pointer
    }

    void HighlightX()
    {
        gizmoMaterial.color = new Color(1, 0.75f, 0.75f);
    }
    void HighlightY()
    {
        gizmoMaterial.color = new Color(0.75f, 1, 0.75f);
    }
    void HighlightZ()
    {
        gizmoMaterial.color = new Color(0.75f, 0.75f, 1);
    }
}
