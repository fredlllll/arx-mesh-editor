using Assets.Scripts.ArxLevelEditor.Editing;
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

        private void Start()
        {
            X.onEndEdit.AddListener(XEndEdit);
            Y.onEndEdit.AddListener(YEndEdit);
            Z.onEndEdit.AddListener(ZEndEdit);

            Gizmo.OnMove.AddListener(OnGizmoMove);
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
