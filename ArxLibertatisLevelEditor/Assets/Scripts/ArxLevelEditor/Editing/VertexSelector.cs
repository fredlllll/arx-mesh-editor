using Assets.Scripts.ArxLevelEditor.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class VertexSelector : MonoBehaviour
    {
        public static VertexSelector Instance { get; private set; }

        GameObject currentlySelected = null;
        public static EditableVertex CurrentlySelected
        {
            get
            {
                if (Instance.currentlySelected != null)
                {
                    return Instance.currentlySelected.GetComponent<EditableVertex>();
                }
                return null;
            }
        }

        public static EditableVertexEvent OnDeselected { get; } = new EditableVertexEvent();
        public static EditableVertexEvent OnSelected { get; } = new EditableVertexEvent();

        int vertexLayerMask;

        private void Awake()
        {
            Instance = this;
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
                OnDeselected.Invoke(currentlySelected.GetComponent<EditableVertex>());
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
                        OnSelected.Invoke(vertex);
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
