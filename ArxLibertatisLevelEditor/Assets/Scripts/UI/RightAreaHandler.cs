using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Editing;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.ArxNative;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class RightAreaHandler : MonoBehaviour
    {
        public GameObject properties;
        public InputField X, Y, Z, U, V, NX, NY, NZ;

        public Button colorPickerButton;
        public ColorPicker.ColorPicker colorPicker;
        public Button closeColorPickerButton;

        public Button pickTextureButton;
        public RawImage polyTextureImage;

        public Toggle noShadow, doubleSided, trans, water, glow, ignore, quad, tiled, metal, hide, stone, wood, gravel, earth, nocol, lava, climb, fall, noPath, noDraw, precisePath, noClimb, angular, angularIDX0, angularIDX1, angularIDX2, angularIDX3, lateMip;
        private readonly List<Tuple<PolyType, Toggle>> toggleAssoc = new List<Tuple<PolyType, Toggle>>();

        private void Start()
        {
            X.onEndEdit.AddListener(XEndEdit);
            Y.onEndEdit.AddListener(YEndEdit);
            Z.onEndEdit.AddListener(ZEndEdit);

            U.onEndEdit.AddListener(UEndEdit);
            V.onEndEdit.AddListener(VEndEdit);

            NX.onEndEdit.AddListener(NXEndEdit);
            NY.onEndEdit.AddListener(NYEndEdit);
            NZ.onEndEdit.AddListener(NZEndEdit);

            colorPickerButton.onClick.AddListener(OnColorPickerClicked);
            closeColorPickerButton.onClick.AddListener(OnCloseColorPickerClicked);
            colorPicker.ColorChanged.AddListener(OnPickerColorChanged);

            Gizmo_OLD.OnMove.AddListener(OnGizmoMove);

            PolygonSelector.OnSelected.AddListener(OnPolySelected);
            PolygonSelector.OnDeselected.AddListener(OnPolyDeselected);

            VertexSelector.OnSelected.AddListener(OnVertSelected);
            VertexSelector.OnDeselected.AddListener(OnVertDeselected);

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

            pickTextureButton.onClick.AddListener(OnPickTextureClick);
        }

        private void SyncPolyTypeToggles()
        {
            var polyType = PolygonSelector.CurrentlySelected.info.polyType;
            foreach (var ass in toggleAssoc)
            {
                ass.Item2.isOn = polyType.HasFlag(ass.Item1);
            }
        }

        private void OnVertSelected(EditableVertex vert)
        {
            U.interactable = true;
            V.interactable = true;
            NX.interactable = true;
            NY.interactable = true;
            NZ.interactable = true;
            colorPickerButton.interactable = true;
            var v = vert.primitive.info.vertices[vert.vertIndex];

            U.text = v.uv.x.ToString(System.Globalization.CultureInfo.InvariantCulture);
            V.text = v.uv.y.ToString(System.Globalization.CultureInfo.InvariantCulture);
            NX.text = v.normal.x.ToString(System.Globalization.CultureInfo.InvariantCulture);
            NY.text = v.normal.y.ToString(System.Globalization.CultureInfo.InvariantCulture);
            NZ.text = v.normal.z.ToString(System.Globalization.CultureInfo.InvariantCulture);
            colorPicker.PickerColor = v.color;
        }

        private void OnVertDeselected(EditableVertex vert)
        {
            U.interactable = false;
            V.interactable = false;
            NX.interactable = false;
            NY.interactable = false;
            NZ.interactable = false;
            colorPickerButton.interactable = false;
            if (colorPicker.gameObject.activeSelf)
            {
                OnCloseColorPickerClicked();
            }
            UpdateUV();
            UpdateNormal();
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
            var tex = LevelEditor.TextureDatabase[prim.Material.TexturePath];
            polyTextureImage.texture = tex;
            pickTextureButton.interactable = true;
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
            pickTextureButton.interactable = false;
            polyTextureImage.texture = null;
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

        private void UEndEdit(string value)
        {
            UpdateUV();
        }

        private void VEndEdit(string value)
        {
            UpdateUV();
        }

        private void NXEndEdit(string value)
        {
            UpdateNormal();
        }

        private void NYEndEdit(string value)
        {
            UpdateNormal();
        }

        private void NZEndEdit(string value)
        {
            UpdateNormal();
        }

        private void OnColorPickerClicked()
        {
            properties.SetActive(false);
            colorPicker.gameObject.SetActive(true);
        }

        private void OnCloseColorPickerClicked()
        {
            properties.SetActive(true);
            colorPicker.gameObject.SetActive(false);
        }

        private void OnPickerColorChanged(Color col)
        {
            var vert = VertexSelector.CurrentlySelected;
            if (vert != null)
            {
                vert.primitive.info.vertices[vert.vertIndex].color = col;
                vert.primitive.UpdateMesh();
            }
        }

        private void UpdateUV()
        {
            var vert = VertexSelector.CurrentlySelected;
            if (vert != null)
            {
                if (float.TryParse(U.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float u))
                {
                    vert.primitive.info.vertices[vert.vertIndex].uv.x = u;
                }
                if (float.TryParse(V.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float v))
                {
                    vert.primitive.info.vertices[vert.vertIndex].uv.y = v;
                }
                vert.primitive.UpdateMesh();
            }
        }

        private void UpdateNormal()
        {
            var vert = VertexSelector.CurrentlySelected;
            if (vert != null)
            {
                var v = vert.primitive.info.vertices[vert.vertIndex];

                if (float.TryParse(NX.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float nx))
                {
                    v.normal.x = nx;
                }
                if (float.TryParse(NY.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float ny))
                {
                    v.normal.y = ny;
                }
                if (float.TryParse(NZ.text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float nz))
                {
                    v.normal.z = nz;
                }

                v.normal.Normalize();
                NX.text = v.normal.x.ToString(System.Globalization.CultureInfo.InvariantCulture);
                NY.text = v.normal.y.ToString(System.Globalization.CultureInfo.InvariantCulture);
                NZ.text = v.normal.z.ToString(System.Globalization.CultureInfo.InvariantCulture);

                vert.primitive.UpdateMesh();
            }
        }

        private void UpdateGizmo()
        {
            if (Gizmo_OLD.Instance.Target != null)
            {
                Vector3 gizmoPos = Gizmo_OLD.Instance.Target.position;

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
                Gizmo_OLD.Instance.Target.position = gizmoPos;
            }
        }

        private void OnGizmoMove()
        {
            Vector3 gizmoPos = Gizmo_OLD.Instance.Target.position;

            X.text = gizmoPos.x.ToString(System.Globalization.CultureInfo.InvariantCulture);
            Y.text = gizmoPos.y.ToString(System.Globalization.CultureInfo.InvariantCulture);
            Z.text = gizmoPos.z.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        private IEnumerator PickTextureCoroutine()
        {
            OpenFileDialog.Filter = "Arx Texture Files (*.jpg|*.bmp)";
            OpenFileDialog.Title = "Open Texture";
            OpenFileDialog.FileName = Path.Combine(EditorSettings.DataDir, PolygonSelector.CurrentlySelected.Material.TexturePath);

            var t = OpenFileDialog.OpenDialog();
            t.Start();
            while (!t.IsCompleted)
            {
                yield return null;
            }
            if (t.Result == OpenFileDialog.DialogResult.OK)
            {
                var path = OpenFileDialog.FileName;
                if (!path.StartsWith(EditorSettings.DataDir))
                {
                    Debug.LogWarning("file is not in datadir, cant use it as a texture");
                }
                else
                {
                    var relPath = path.Replace(EditorSettings.DataDir, "");
                    var em = new EditorMaterial(relPath, PolygonSelector.CurrentlySelected.info.polyType, PolygonSelector.CurrentlySelected.Material.TransVal);
                    PolygonSelector.CurrentlySelected.Material = em;
                    polyTextureImage.texture = em.Material.mainTexture;
                }
            }
        }

        private void OnPickTextureClick()
        {
            StartCoroutine(PickTextureCoroutine());
        }
    }
}
