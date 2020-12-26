using Assets.Scripts.ArxLevelEditor.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class VertexSelector : MonoBehaviour
    {
        GameObject currentlySelected = null;

        int vertexLayerMask;

        private void Awake()
        {
            vertexLayerMask = 1 << LayerMask.NameToLayer("Vertices");
        }

        private void Start()
        {
            EditWindowClickDetection.clickHandlers.Add(HandleClick, 0);
        }

        void Deselect()
        {
            Gizmo.Detach();
            Gizmo.Visible = false;

            if (currentlySelected != null)
            {
                //nothing to do to unselect for vertices
            }
        }

        private bool HandleClick(Vector3 localPos, int btn)
        {
            if (LevelEditor.EditState == EditState.Vertices && btn == EditWindowClickDetection.BTN_PRIMARY)
            {
                var ray = EditWindow.GetRayFromMousePosition(localPos);
                //raycast with vertices
                if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, vertexLayerMask))
                {
                    var vertex = hitInfo.transform.gameObject.GetComponent<EditableVertex>();
                    if (vertex != null)
                    {
                        Deselect();
                        currentlySelected = hitInfo.transform.gameObject;

                        Gizmo.Attach(currentlySelected.transform, Vector3.zero);
                        Gizmo.Visible = true;
                        return true;
                    }
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
