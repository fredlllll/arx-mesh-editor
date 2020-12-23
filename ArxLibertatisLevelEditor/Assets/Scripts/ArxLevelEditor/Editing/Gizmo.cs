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
        static int gizmoLayer = 0;

        public static Gizmo Instance
        {
            get;
            private set;
        }

        GameObject moveGameObject, rotateGameObject, scaleGameObject;
        GameObject moveX, moveY, moveZ;
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

            moveX = new GameObject();
            moveX.layer = gizmoLayer;
            moveX.transform.SetParent(moveGameObject.transform);
            moveX.transform.localPosition = Vector3.right;
            var col = moveX.AddComponent<SphereCollider>();
            col.radius = 0.2f;

            moveY = new GameObject();
            moveY.layer = gizmoLayer;
            moveY.transform.SetParent(moveGameObject.transform);
            moveY.transform.localPosition = Vector3.up;
            col = moveY.AddComponent<SphereCollider>();
            col.radius = 0.2f;

            moveZ = new GameObject();
            moveZ.layer = gizmoLayer;
            moveZ.transform.SetParent(moveGameObject.transform);
            moveZ.transform.localPosition = Vector3.forward;
            col = moveZ.AddComponent<SphereCollider>();
            col.radius = 0.2f;
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
            gizmoLayer = LayerMask.NameToLayer("Gizmo");
        }

        void Start()
        {
            gizmoMaterial = Instantiate(MaterialsDatabase.GizmoMaterial); //copy material so we can modify it

            CreateMove();
            CreateRotate();
            CreateScale();

            Mode = GizmoMode.Move;
        }

        bool dragging = false;
        public bool Dragging
        {
            get { return dragging; }
        }
        Vector3 dragLast;
        GameObject dragObject;

        // Update is called once per frame
        void Update()
        {
            //Do raycasting from mouse pointer and monitor dragging to do stuff
            if (LevelEditor.EditState == EditState.Vertices)
            {
                if (!dragging && Input.GetMouseButtonDown(0))
                {
                    var ray = EditWindow.GetRayFromMousePosition();
                    if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, 1 << gizmoLayer))
                    {
                        dragObject = hit.transform.gameObject;
                        if (dragObject == moveX || dragObject == moveY || dragObject == moveZ)
                        {
                            dragLast = Input.mousePosition;
                            dragging = true;
                        }
                    }
                }
                else if (dragging && Input.GetMouseButtonUp(0))
                {
                    dragging = false;
                }
                if (dragging)
                {
                    Vector3 offset = Input.mousePosition - dragLast;
                    offset /= 100;
                    dragLast = Input.mousePosition;
                    var pos = target.position;
                    if (dragObject == moveX)
                    {
                        pos.x += offset.x;
                    }
                    else if (dragObject == moveY)
                    {
                        pos.y += offset.y;
                    }
                    else if (dragObject == moveZ)
                    {
                        pos.z -= offset.x;
                    }
                    target.position = pos;
                }
            }
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

        public static void Detach()
        {
            Instance.target = null;
            Instance.transform.SetParent(null);
        }

        public static bool Visible
        {
            get { return Instance.gameObject.activeSelf; }
            set { Instance.gameObject.SetActive(value); }
        }
    }
}