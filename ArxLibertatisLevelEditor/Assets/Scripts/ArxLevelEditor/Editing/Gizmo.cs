using Assets.Scripts;
using System;
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
        static int gizmoLayerMask = 0;

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

        SnapGrid snapGrid = new SnapGrid();

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
            gizmoLayerMask = 1 << gizmoLayer;
        }

        void Start()
        {
            gizmoMaterial = Instantiate(MaterialsDatabase.GizmoMaterial); //copy material so we can modify it

            CreateMove();
            CreateRotate();
            CreateScale();

            Mode = GizmoMode.Move;

            EditWindowClickDetection.beginDragHandlers.Add(HandleBeginDrag, 0);
            EditWindowClickDetection.dragHandlers.Add(HandleDrag, 0);
            EditWindowClickDetection.endDragHandlers.Add(HandleEndDrag, 0);

            Visible = false;
        }

        Vector3 virtualPosition;
        bool dragging = false;
        GameObject dragObject;
        bool HandleBeginDrag(Vector3 localPos, int btn)
        {
            if (LevelEditor.EditState == EditState.Vertices && btn == EditWindowClickDetection.BTN_PRIMARY)
            {
                Debug.Log("begin drag");
                var ray = EditWindow.GetRayFromMousePosition(localPos);
                if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, gizmoLayerMask))
                {
                    dragObject = hit.transform.gameObject;
                    if (dragObject == moveX || dragObject == moveY || dragObject == moveZ)
                    {
                        virtualPosition = target.position;
                        dragging = true;
                        return true;
                    }
                }
            }
            return false;
        }

        Vector3 SnapVertex(Vector3 pos)
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

        bool HandleDrag(Vector3 from, Vector3 to, Vector3 mouseOffset, int btn)
        {
            if (dragging && btn == EditWindowClickDetection.BTN_PRIMARY)
            {
                //TODO: project the dragged object arrow to viewport and then figure out movement from offset
                mouseOffset /= 100;
                Vector3 worldOffset = new Vector3();

                if (dragObject == moveX)
                {
                    worldOffset.x += mouseOffset.x;
                }
                else if (dragObject == moveY)
                {
                    worldOffset.y += mouseOffset.y;
                }
                else if (dragObject == moveZ)
                {
                    worldOffset.z += mouseOffset.x;
                }

                virtualPosition += worldOffset;
                Vector3 pos = new Vector3();
                switch (LevelEditor.SnapMode)
                {
                    case SnapMode.None:
                        pos = virtualPosition;
                        break;
                    case SnapMode.Grid:
                        pos = snapGrid.Snap(virtualPosition);
                        break;
                    case SnapMode.Vertex:
                        pos = SnapVertex(virtualPosition);
                        break;
                }

                target.position = pos;
                return true;
            }
            return false;
        }

        bool HandleEndDrag(Vector3 localPos, int btn)
        {
            if (dragging)
            {
                Debug.Log("end drag");
                dragging = false;
                return true;
            }
            return false;
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

        public static void HighlightNone()
        {
            Instance.gizmoMaterial.color = new Color(0.75f, 0.75f, 0.75f);
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