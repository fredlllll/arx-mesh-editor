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
        }

        bool dragging = false;
        GameObject dragObject;

        bool HandleBeginDrag(Vector3 localPos, int btn)
        {
            if (LevelEditor.EditState == EditState.Vertices && btn == EditWindowClickDetection.BTN_PRIMARY)
            {
                var ray = EditWindow.GetRayFromMousePosition(localPos);
                if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, gizmoLayerMask))
                {
                    dragObject = hit.transform.gameObject;
                    if (dragObject == moveX || dragObject == moveY || dragObject == moveZ)
                    {
                        dragging = true;
                        return true;
                    }
                }
            }
            return false;
        }

        bool HandleDrag(Vector3 from, Vector3 to, Vector3 offset, int btn)
        {
            if (dragging && btn == EditWindowClickDetection.BTN_PRIMARY)
            {
                //TODO: project the dragged object arrow to viewport and then figure out movement from offset
                offset /= 100;
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
                return true;
            }
            return false;
        }

        bool HandleEndDrag(Vector3 localPos, int btn)
        {
            if (dragging)
            {
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