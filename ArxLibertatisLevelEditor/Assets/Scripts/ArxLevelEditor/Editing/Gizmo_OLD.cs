﻿using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.ArxLevelEditor.Editing
{

    public class Gizmo_OLD : MonoBehaviour
    {
        static int gizmoLayer = 0;
        static int gizmoLayerMask = 0;

        public static Gizmo_OLD Instance
        {
            get;
            private set;
        }

        GameObject moveGameObject, rotateGameObject, scaleGameObject;
        GameObject moveX, moveY, moveZ;
        Transform target = null;
        public Transform Target { get { return target; } }

        Material gizmoMaterial;

        GizmoMode mode;
        public GizmoMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                switch (mode)
                {
                    case GizmoMode.Move:
                        moveGameObject.SetActive(true);
                        rotateGameObject.SetActive(false);
                        scaleGameObject.SetActive(false);
                        break;
                    case GizmoMode.Rotate:
                        moveGameObject.SetActive(false);
                        rotateGameObject.SetActive(true);
                        scaleGameObject.SetActive(false);
                        break;
                    case GizmoMode.Scale:
                        moveGameObject.SetActive(false);
                        rotateGameObject.SetActive(false);
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

        bool HandleDrag(Vector3 from, Vector3 to, Vector3 mouseOffset, int btn)
        {
            if (dragging && btn == EditWindowClickDetection.BTN_PRIMARY)
            {
                //TODO: project the dragged object arrow to viewport and then figure out movement from offset
                mouseOffset /= 100;
                Vector3 worldOffset = new Vector3();

                SnapAxis snapAxis = SnapAxis.None;

                if (dragObject == moveX)
                {
                    worldOffset.x += mouseOffset.x;
                    snapAxis = SnapAxis.X;
                }
                else if (dragObject == moveY)
                {
                    worldOffset.y += mouseOffset.y;
                    snapAxis = SnapAxis.Y;
                }
                else if (dragObject == moveZ)
                {
                    worldOffset.z += mouseOffset.x;
                    snapAxis = SnapAxis.Z;
                }

                virtualPosition += worldOffset;
                Vector3 pos = LevelEditor.SnapManager.Snap(virtualPosition, snapAxis);

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

        private void Update()
        {
            if (target != null && target.hasChanged)
            {
                OnMove.Invoke();
                target.hasChanged = false;
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

        public static void HighlightNone()
        {
            Instance.gizmoMaterial.color = new Color(0.75f, 0.75f, 0.75f);
        }

        public static void Attach(Transform target, Vector3 localPosition)
        {
            Instance.target = target;
            Instance.transform.SetParent(target);
            Instance.transform.localPosition = localPosition;
            OnMove.Invoke();
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

        public static UnityEvent OnMove
        {
            get;
        } = new UnityEvent();
    }
}