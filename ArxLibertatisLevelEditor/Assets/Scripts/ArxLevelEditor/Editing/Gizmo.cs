using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public enum GizmoMode
    {
        Move,
        Rotate,
        Scale,
    }

    public class Gizmo : MonoBehaviour
    {
        public static Gizmo Instance
        {
            get;
            private set;
        }

        GameObject moveGameObject, rotateGameObject, scaleGameObject;
        Transform target = null;

        UnityEngine.Material gizmoMaterial;

        GizmoMode mode;
        public GizmoMode Mode
        {
            get { return mode; }
            set
            {
                moveGameObject.SetActive(false);
                rotateGameObject.SetActive(false);
                scaleGameObject.SetActive(false);
                mode = value;
                switch (mode)
                {
                    case GizmoMode.Move:
                        moveGameObject.SetActive(true);
                        break;
                    case GizmoMode.Rotate:
                        rotateGameObject.SetActive(true);
                        break;
                    case GizmoMode.Scale:
                        scaleGameObject.SetActive(true);
                        break;
                }
            }
        }

        void CreateMove()
        {
            moveGameObject = new GameObject();
            moveGameObject.transform.SetParent(transform);
            moveGameObject.transform.localPosition = Vector3.zero;

            GizmoCreator.CreateMove(moveGameObject, gizmoMaterial);
        }

        void CreateRotate()
        {
            rotateGameObject = new GameObject();
            rotateGameObject.transform.SetParent(transform);
            rotateGameObject.transform.localPosition = Vector3.zero;

            GizmoCreator.CreateRotate(rotateGameObject, gizmoMaterial);
        }

        void CreateScale()
        {
            scaleGameObject = new GameObject();
            scaleGameObject.transform.SetParent(transform);
            scaleGameObject.transform.localPosition = Vector3.zero;

            GizmoCreator.CreateScale(scaleGameObject, gizmoMaterial);
        }

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            gizmoMaterial = Instantiate(MaterialsDatabase.GizmoMaterial); //copy material so we can modify it

            CreateMove();
            CreateRotate();
            CreateScale();

            Mode = GizmoMode.Move;
        }

        // Update is called once per frame
        void Update()
        {
            //Do raycasting from mouse pointer and monitor dragging to do stuff
        }

        public static void HighlightX()
        {
            Instance.gizmoMaterial.color = new Color(1, 0.75f, 0.75f);
        }
        public static void HighlightY()
        {
            Instance.gizmoMaterial.color = new Color(0.75f, 1, 0.75f);
        }
        public static void HighlightZ()
        {
            Instance.gizmoMaterial.color = new Color(0.75f, 0.75f, 1);
        }

        public static void Attach(Transform target, Vector3 localPosition)
        {
            Instance.target = target;
            Instance.transform.SetParent(target);
            Instance.transform.localPosition = localPosition;
        }
    }
}