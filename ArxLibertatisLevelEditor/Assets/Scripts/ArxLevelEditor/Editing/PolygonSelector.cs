using Assets.Scripts.ArxLevelEditor.Mesh;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{


    public class PolygonSelector : MonoBehaviour
    {
        public static PolygonSelector Instance { get; private set; }
        public static EditablePrimitiveEvent OnDeselected { get; } = new EditablePrimitiveEvent();
        public static EditablePrimitiveEvent OnSelected { get; } = new EditablePrimitiveEvent();

        GameObject currentlySelected = null;

        public static EditablePrimitive CurrentlySelected
        {
            get
            {
                if (Instance.currentlySelected != null)
                {
                    return Instance.currentlySelected.GetComponent<EditablePrimitive>();
                }
                return null;
            }
        }

        static int polygonsLayerMask;
        public static int PolygonsLayerMask
        {
            get { return polygonsLayerMask; }
        }

        private void Awake()
        {
            Instance = this;
            polygonsLayerMask = 1 << LayerMask.NameToLayer("EditableLevelMesh");
        }

        private void Start()
        {
            EditWindowClickDetection.clickHandlers.Add(HandleClick, 0);
        }

        public void Deselect()
        {
            Gizmo_OLD.Detach();
            Gizmo_OLD.Visible = false;

            //place currently selected back into mesh
            if (currentlySelected != null)
            {
                var selectedPrimitive = currentlySelected.GetComponent<EditablePrimitive>();
                var editableMesh = LevelEditor.CurrentLevel.EditableLevelMesh.GetMaterialMesh(selectedPrimitive.Material);
                editableMesh.AddPrimitive(selectedPrimitive.info);
                editableMesh.UpdateMesh();
                OnDeselected.Invoke(selectedPrimitive);
                Destroy(currentlySelected);
                currentlySelected = null;
            }
        }

        public void Duplicate()
        {
            if (currentlySelected != null)
            {
                //adds the currently selected back to the mesh, but doesnt destroy the gameobject so its like a dupe
                var selectedPrimitive = currentlySelected.GetComponent<EditablePrimitive>();
                var editableMesh = LevelEditor.CurrentLevel.EditableLevelMesh.GetMaterialMesh(selectedPrimitive.Material);
                editableMesh.AddPrimitive(selectedPrimitive.info.Copy()); //add copy as adding the same twice could lead to problems
                editableMesh.UpdateMesh();
            }
        }

        public void DeleteSelected()
        {
            if(currentlySelected != null)
            {
                var selectedPrimitive = currentlySelected.GetComponent<EditablePrimitive>();
                OnDeselected.Invoke(selectedPrimitive);
                Destroy(currentlySelected);
                currentlySelected = null;
            }
        }

        private bool HandleClick(Vector3 localPos, int btn)
        {
            if (LevelEditor.EditState == EditState.Polygons && btn == EditWindowClickDetection.BTN_PRIMARY)
            {
                var ray = EditWindow.GetRayFromMousePosition(localPos);
                //raycast with levelmeshes to see what polygon was clicked
                if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, polygonsLayerMask))
                {
                    var mesh = hitInfo.transform.gameObject.GetComponent<MaterialMesh>();
                    if (mesh != null)
                    {
                        var primitive = mesh.GetByTriangleIndex(hitInfo.triangleIndex);
                        if (primitive != null)
                        {
                            Deselect();

                            //remove newly selected from mesh
                            mesh.RemovePrimitive(primitive);
                            mesh.UpdateMesh();

                            var go = new GameObject();
                            currentlySelected = go;
                            go.transform.SetParent(mesh.gameObject.transform);
                            go.transform.localPosition = Vector3.zero;
                            go.transform.localScale = Vector3.one;
                            var editablePrimitive = go.AddComponent<EditablePrimitive>();
                            editablePrimitive.UpdatePrimitive(primitive, mesh.EditorMaterial);

                            OnSelected.Invoke(editablePrimitive);
                        }
                        else
                        {
                            Debug.LogWarning("primitive is null for " + hitInfo.triangleIndex);
                        }
                    }
                    return true;
                }
                else
                {
                    Deselect();
                }
            }
            return false;
        }
    }
}
