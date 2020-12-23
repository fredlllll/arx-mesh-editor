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

        void Deselect()
        {
            Gizmo.Detach();
            Gizmo.Visible = false;

            if (currentlySelected != null)
            {
                //nothing to do to unselect for vertices
            }
        }

        private void Update()
        {
            if (EditWindow.MouseInEditWindow)
            {
                if (LevelEditor.EditState == EditState.Vertices && Input.GetMouseButtonUp(0) && !Gizmo.Instance.Dragging)
                {
                    //convert mouse position to viewport position
                    var ray = EditWindow.GetRayFromMousePosition();
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
                        }
                    }
                    else
                    {
                        Deselect();
                    }
                }
            }
        }
    }
}
