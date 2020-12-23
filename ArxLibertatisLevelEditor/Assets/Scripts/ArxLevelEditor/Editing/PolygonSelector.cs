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
        int selectLayer;

        private void Awake()
        {
            selectLayer = 1 << LayerMask.NameToLayer("EditableLevelMesh");
        }

        private void Update()
        {
            if (EditWindowState.MouseInEditWindow)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //convert mouse position to viewport position
                    var ray = EditWindowState.GetRayFromMousePosition();
                    //TODO: raycast with levelmeshes to see what polygon was clicked
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, selectLayer))
                    {
                        var mesh = hitInfo.transform.gameObject.GetComponent<MaterialMesh>();
                        if (mesh != null)
                        {
                            var primitive = mesh.GetByTriangleIndex(hitInfo.triangleIndex);
                            mesh.RemovePrimitive(primitive);
                            mesh.UpdateMesh();

                            var go = new GameObject();
                            go.transform.SetParent(mesh.gameObject.transform);
                            go.transform.localPosition = Vector3.zero;
                            go.transform.localScale = Vector3.one;
                            var editablePrimitive = go.AddComponent<EditablePrimitive>();
                            editablePrimitive.UpdatePrimitive(primitive, mesh.Material);

                            Vector3 localPos = hitInfo.point * 100;
                            localPos.y *= -1;
                            Gizmo.Attach(editablePrimitive.transform, localPos);
                        }
                        //Gizmo.Instance.transform.position = hitInfo.point;
                    }
                }
            }
        }
    }
}
