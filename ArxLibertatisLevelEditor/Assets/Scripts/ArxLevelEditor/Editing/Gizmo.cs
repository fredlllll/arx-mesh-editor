using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public enum GizmoMode
    {
        Move,
        Rotate,
        Scale,
    }

    public class PositionChangedEvent : UnityEvent<Gizmo, Vector3, Vector3> { }
    //TODO: need events for rotation and scale too in the future?

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

        public bool Visible
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }

        public PositionChangedEvent PositionChanged { get; } = new PositionChangedEvent();

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



        Vector3 virtualPosition;
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
                        virtualPosition = transform.position;
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

                var oldPos = transform.position;
                var newPos = LevelEditor.SnapManager.Snap(virtualPosition, snapAxis);

                transform.position = newPos;
                PositionChanged.Invoke(this, oldPos, newPos);
                return true;
            }
            return false;
        }

        bool HandleEndDrag(Vector3 localPos, int btn)
        {
            if (dragging)
            {
                transform.hasChanged = false;
                dragging = false;
                return true;
            }
            return false;
        }

        public void HighlightX()
        {
            gizmoMaterial.color = new Color(1, 0.75f, 0.75f);
        }
        public void HighlightY()
        {
            gizmoMaterial.color = new Color(0.75f, 1, 0.75f);
        }
        public void HighlightZ()
        {
            gizmoMaterial.color = new Color(0.75f, 0.75f, 1);
        }

        public void HighlightNone()
        {
            gizmoMaterial.color = new Color(0.75f, 0.75f, 0.75f);
        }

    }
}