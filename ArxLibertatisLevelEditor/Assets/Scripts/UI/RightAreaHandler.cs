using Assets.Scripts.ArxLevelEditor.Editing;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.ArxNative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class RightAreaHandler : MonoBehaviour
    {
        public InputField X, Y, Z;

        public Toggle noShadow, doubleSided, trans, water, glow, ignore, quad, tiled, metal, hide, stone, wood, gravel, earth, nocol, lava, climb, fall, noPath, noDraw, precisePath, noClimb, angular, angularIDX0, angularIDX1, angularIDX2, angularIDX3, lateMip;
        private readonly List<Tuple<PolyType, Toggle>> toggleAssoc = new List<Tuple<PolyType, Toggle>>();

        private void Start()
        {
            X.onEndEdit.AddListener(XEndEdit);
            Y.onEndEdit.AddListener(YEndEdit);
            Z.onEndEdit.AddListener(ZEndEdit);

            Gizmo.OnMove.AddListener(OnGizmoMove);

            PolygonSelector.OnSelected.AddListener(OnPolySelected);
            PolygonSelector.OnDeselected.AddListener(OnPolyDeselected);

            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.NO_SHADOW, noShadow));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.DOUBLESIDED, doubleSided));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.TRANS, trans));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.WATER, water));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.GLOW, glow));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.IGNORE, ignore));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.QUAD, quad));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.TILED, tiled));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.METAL, metal));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.HIDE, hide));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.STONE, stone));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.WOOD, wood));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.GRAVEL, gravel));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.EARTH, earth));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.NOCOL, nocol));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.LAVA, lava));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.CLIMB, climb));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.FALL, fall));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.NOPATH, noPath));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.NODRAW, noDraw));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.PRECISE_PATH, precisePath));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.NO_CLIMB, noClimb));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.ANGULAR, angular));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.ANGULAR_IDX0, angularIDX0));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.ANGULAR_IDX1, angularIDX1));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.ANGULAR_IDX2, angularIDX2));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.ANGULAR_IDX3, angularIDX3));
            toggleAssoc.Add(new Tuple<PolyType, Toggle>(PolyType.LATE_MIP, lateMip));

            foreach (var ass in toggleAssoc)
            {
                var type = ass.Item1;
                ass.Item2.onValueChanged.AddListener((newVal) =>
                {
                    if (PolygonSelector.CurrentlySelected != null)
                    {
                        if (newVal)
                        {
                            PolygonSelector.CurrentlySelected.SetPolyTypeFlag(type);
                        }
                        else
                        {
                            PolygonSelector.CurrentlySelected.UnsetPolyTypeFlag(type);
                        }
                    }
                });
            }
        }

        private void SyncPolyTypeToggles()
        {
            var polyType = PolygonSelector.CurrentlySelected.info.polyType;
            foreach (var ass in toggleAssoc)
            {
                ass.Item2.isOn = polyType.HasFlag(ass.Item1);
            }
        }

        private void OnPolySelected(EditablePrimitive prim)
        {
            X.interactable = true;
            Y.interactable = true;
            Z.interactable = true;
            foreach (var ass in toggleAssoc)
            {
                ass.Item2.interactable = true;
            }
            SyncPolyTypeToggles();
        }

        private void OnPolyDeselected(EditablePrimitive prim)
        {
            X.interactable = false;
            Y.interactable = false;
            Z.interactable = false;
            foreach (var ass in toggleAssoc)
            {
                ass.Item2.interactable = false;
            }
        }

        private void XEndEdit(string value)
        {
            UpdateGizmo();
        }

        private void YEndEdit(string value)
        {
            UpdateGizmo();
        }

        private void ZEndEdit(string value)
        {
            UpdateGizmo();
        }

        private void UpdateGizmo()
        {
            if (Gizmo.Instance.Target != null)
            {
                Vector3 gizmoPos = Gizmo.Instance.Target.position;

                if (float.TryParse(X.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float x))
                {
                    gizmoPos.x = x;
                }
                if (float.TryParse(Y.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float y))
                {
                    gizmoPos.y = y;
                }
                if (float.TryParse(Z.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float z))
                {
                    gizmoPos.z = z;
                }
                Gizmo.Instance.Target.position = gizmoPos;
            }
        }

        private void OnGizmoMove()
        {
            Vector3 gizmoPos = Gizmo.Instance.Target.position;

            X.text = gizmoPos.x.ToString(System.Globalization.CultureInfo.InvariantCulture);
            Y.text = gizmoPos.y.ToString(System.Globalization.CultureInfo.InvariantCulture);
            Z.text = gizmoPos.z.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
