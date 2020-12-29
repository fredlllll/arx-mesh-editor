using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class PolygonSelector : MonoBehaviour
    {
        public static PolygonSelector Instance { get; private set; }

        GameObject currentlySelected = null;

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

        void Deselect()
        {
            Gizmo.Detach();
            Gizmo.Visible = false;

            //place currently selected back into mesh
            if (currentlySelected != null)
            {
                var selectedPrimitive = currentlySelected.GetComponent<EditablePrimitive>();
                var editableMesh = LevelEditor.CurrentLevel.EditableLevelMesh.GetMaterialMesh(selectedPrimitive.material);
                editableMesh.AddPrimitive(selectedPrimitive.info);
                editableMesh.UpdateMesh();
                Destroy(currentlySelected);
                currentlySelected = null;
            }
        }

        public static void Duplicate()
        {
            if (Instance.currentlySelected != null)
            {
                //adds the currently selected back to the mesh, but doesnt destroy the gameobject so its like a dupe
                var selectedPrimitive = Instance.currentlySelected.GetComponent<EditablePrimitive>();
                var editableMesh = LevelEditor.CurrentLevel.EditableLevelMesh.GetMaterialMesh(selectedPrimitive.material);
                editableMesh.AddPrimitive(selectedPrimitive.info.Copy()); //add copy as adding the same twice could lead to problems
                editableMesh.UpdateMesh();
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
