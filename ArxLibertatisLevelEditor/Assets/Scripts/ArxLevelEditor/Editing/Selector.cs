using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class Selector : MonoBehaviour
    {
        public static Selector Instance { get; private set; }

        public Selection Selection { get; } = new Selection();

        int selectablesLayerMask;

        private void Awake()
        {
            Instance = this;
            selectablesLayerMask = 1 << LayerMask.NameToLayer("Vertices");
        }

        private void Start()
        {
            EditWindowClickDetection.clickHandlers.Add(HandleClick, 0);

            Gizmo.Instance.PositionChanged.AddListener(OnGizmoPositionChanged);
        }

        public void Deselect()
        {
            Gizmo.Instance.Visible = false;
            Selection.Clear();
        }

        private bool HandleClick(Vector3 localPos, int btn)
        {
            if (LevelEditor.EditState == EditState.Vertices && btn == EditWindowClickDetection.BTN_PRIMARY)
            {
                var ray = EditWindow.GetRayFromMousePosition(localPos);
                //raycast with selectables
                if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, selectablesLayerMask))
                {
                    var selectable = hitInfo.transform.gameObject.GetComponent<Selectable>();
                    if (selectable != null)
                    {
                        if (Selection.Count > 0)
                        {
                            if (Selection.SelectedObjects.First() != selectable)
                            {
                                Selection.Clear();
                                Selection.Add(selectable);
                            }
                        }
                        Gizmo.Instance.Visible = true;
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

        private void Update()
        {
            var gtrans = Gizmo.Instance.transform;
            if (!gtrans.hasChanged)
            {
                gtrans.position = Selection.Center;
            }
            else
            {
                Selection.SetCenter(gtrans.position);
            }
            gtrans.hasChanged = false;
        }

        private void OnGizmoPositionChanged(Gizmo g, Vector3 oldPos, Vector3 newPos)
        {
            var translation = newPos - oldPos;
            Selection.Translate(translation);
            //Gizmo.Instance.transform.position = Selection.Center;
        }
    }
}
