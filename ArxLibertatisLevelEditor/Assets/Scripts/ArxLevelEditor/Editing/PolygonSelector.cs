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
                        Gizmo.Instance.transform.position = hitInfo.point;
                    }
                }
            }
        }
    }
}
