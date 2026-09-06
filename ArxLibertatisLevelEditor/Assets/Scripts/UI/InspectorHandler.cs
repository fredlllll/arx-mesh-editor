using ArxLibertatisEditorIO.Util;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Editing;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.Util;
using SFB;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

namespace Assets.Scripts.UI
{
    public class InspectorHandler : MonoBehaviour
    {
        private static readonly PolyType[] PolyTypeFlags =
        {
            PolyType.NO_SHADOW, PolyType.DOUBLESIDED, PolyType.TRANS, PolyType.WATER,
            PolyType.GLOW, PolyType.IGNORE, PolyType.QUAD, PolyType.TILED,
            PolyType.METAL, PolyType.HIDE, PolyType.STONE, PolyType.WOOD,
            PolyType.GRAVEL, PolyType.EARTH, PolyType.NOCOL, PolyType.LAVA,
            PolyType.CLIMB, PolyType.FALL, PolyType.NOPATH, PolyType.NODRAW,
            PolyType.PRECISE_PATH, PolyType.NO_CLIMB, PolyType.ANGULAR,
            PolyType.ANGULAR_IDX0, PolyType.ANGULAR_IDX1, PolyType.ANGULAR_IDX2,
            PolyType.ANGULAR_IDX3, PolyType.LATE_MIP,
        };

        public static void Setup(UIDocument document)
        {
            var inspectorComp = document.GetComponent<InspectorHandler>();
            if (inspectorComp == null)
            {
                inspectorComp = document.gameObject.AddComponent<InspectorHandler>();
            }
            inspectorComp.Initialize(document);
        }

        //polygon fields
        private FloatField xField, yField, zField;
        private IntegerField roomField;
        private Image texturePreview;
        private Button pickTextureButton;
        private readonly Dictionary<PolyType, Toggle> polyTypeToggles = new Dictionary<PolyType, Toggle>();

        //vertex fields
        private FloatField uField, vField, nxField, nyField, nzField;
        private FloatField colorRField, colorGField, colorBField;
        private VisualElement colorPreview;

        private bool syncing = false;

        private void Initialize(UIDocument doc)
        {
            var inspector = doc.rootVisualElement.Q<VisualElement>("Inspector");
            if (inspector == null)
            {
                Debug.LogError("InspectorHandler: Could not find 'Inspector' element in the UI document.");
                return;
            }

            BuildInspector(inspector);

            PolygonSelector.OnSelected.AddListener(OnPolySelected);
            PolygonSelector.OnDeselected.AddListener(OnPolyDeselected);
            VertexSelector.OnSelected.AddListener(OnVertSelected);
            VertexSelector.OnDeselected.AddListener(OnVertDeselected);
            Gizmo_OLD.OnMove.AddListener(OnGizmoMove);
        }

        private void OnDestroy()
        {
            PolygonSelector.OnSelected.RemoveListener(OnPolySelected);
            PolygonSelector.OnDeselected.RemoveListener(OnPolyDeselected);
            VertexSelector.OnSelected.RemoveListener(OnVertSelected);
            VertexSelector.OnDeselected.RemoveListener(OnVertDeselected);
            Gizmo_OLD.OnMove.RemoveListener(OnGizmoMove);
        }

        private void BuildInspector(VisualElement inspector)
        {
            var polygonHeader = new Label("Polygon");
            polygonHeader.AddToClassList("InspectorHeader");
            inspector.Add(polygonHeader);

            var posField = new VisualElement();
            posField.AddToClassList("InspectorFieldRow");
            posField.Add(CreateFieldLabel("Position"));
            xField = CreateFloatField("X", OnXChanged);
            yField = CreateFloatField("Y", OnYChanged);
            zField = CreateFloatField("Z", OnZChanged);
            posField.Add(xField);
            posField.Add(yField);
            posField.Add(zField);
            inspector.Add(posField);

            var roomFieldRow = new VisualElement();
            roomFieldRow.AddToClassList("InspectorFieldRow");
            roomFieldRow.Add(CreateFieldLabel("Room"));
            roomField = new IntegerField();
            roomField.AddToClassList("InspectorField");
            roomField.RegisterValueChangedCallback(OnRoomChanged);
            roomFieldRow.Add(roomField);
            inspector.Add(roomFieldRow);

            var textureRow = new VisualElement();
            textureRow.AddToClassList("InspectorFieldRow");
            textureRow.Add(CreateFieldLabel("Texture"));
            texturePreview = new Image();
            texturePreview.AddToClassList("InspectorTexturePreview");
            textureRow.Add(texturePreview);
            pickTextureButton = new Button(PickTextureClicked) { text = "Pick" };
            pickTextureButton.AddToClassList("InspectorButton");
            textureRow.Add(pickTextureButton);
            inspector.Add(textureRow);

            var polyTypeHeader = new Label("Poly Type");
            polyTypeHeader.AddToClassList("InspectorHeader");
            inspector.Add(polyTypeHeader);

            var flagGrid = new VisualElement();
            flagGrid.AddToClassList("InspectorToggleGrid");
            foreach (var flag in PolyTypeFlags)
            {
                var toggle = new Toggle(flag.ToString());
                toggle.AddToClassList("InspectorToggle");
                toggle.SetEnabled(false);
                var captured = flag;
                toggle.RegisterValueChangedCallback(evt =>
                {
                    if (syncing)
                    {
                        return;
                    }
                    var poly = PolygonSelector.CurrentlySelected;
                    if (poly == null)
                    {
                        return;
                    }
                    if (evt.newValue)
                    {
                        poly.SetPolyTypeFlag(captured);
                    }
                    else
                    {
                        poly.UnsetPolyTypeFlag(captured);
                    }
                });
                polyTypeToggles[captured] = toggle;
                flagGrid.Add(toggle);
            }
            inspector.Add(flagGrid);

            var vertexHeader = new Label("Vertex");
            vertexHeader.AddToClassList("InspectorHeader");
            inspector.Add(vertexHeader);

            var uvRow = new VisualElement();
            uvRow.AddToClassList("InspectorFieldRow");
            uvRow.Add(CreateFieldLabel("UV"));
            uField = CreateFloatField("U", OnUChanged);
            vField = CreateFloatField("V", OnVChanged);
            uvRow.Add(uField);
            uvRow.Add(vField);
            inspector.Add(uvRow);

            var normalRow = new VisualElement();
            normalRow.AddToClassList("InspectorFieldRow");
            normalRow.Add(CreateFieldLabel("Normal"));
            nxField = CreateFloatField("X", OnNormalChanged);
            nyField = CreateFloatField("Y", OnNormalChanged);
            nzField = CreateFloatField("Z", OnNormalChanged);
            normalRow.Add(nxField);
            normalRow.Add(nyField);
            normalRow.Add(nzField);
            inspector.Add(normalRow);

            var colorHeader = new Label("Vertex Color");
            colorHeader.AddToClassList("InspectorHeader");
            inspector.Add(colorHeader);

            colorPreview = new VisualElement();
            colorPreview.AddToClassList("InspectorColorPreview");
            inspector.Add(colorPreview);

            var colorRow = new VisualElement();
            colorRow.AddToClassList("InspectorFieldRow");
            colorRField = CreateFloatField("R", OnColorChanged);
            colorGField = CreateFloatField("G", OnColorChanged);
            colorBField = CreateFloatField("B", OnColorChanged);
            colorRow.Add(colorRField);
            colorRow.Add(colorGField);
            colorRow.Add(colorBField);
            inspector.Add(colorRow);
        }

        private static Label CreateFieldLabel(string text)
        {
            var label = new Label(text);
            label.AddToClassList("InspectorFieldLabel");
            return label;
        }

        private static FloatField CreateFloatField(string label, EventCallback<ChangeEvent<float>> onChanged)
        {
            var field = new FloatField(label);
            field.AddToClassList("InspectorField");
            field.SetEnabled(false);
            field.RegisterValueChangedCallback(onChanged);
            return field;
        }

        private void SetFloatFieldValue(FloatField field, float value)
        {
            syncing = true;
            field.SetValueWithoutNotify(value);
            syncing = false;
        }

        private void OnPolySelected(EditablePrimitive prim)
        {
            xField.SetEnabled(true);
            yField.SetEnabled(true);
            zField.SetEnabled(true);
            roomField.SetEnabled(true);
            pickTextureButton.SetEnabled(true);
            SetObjectEnabled(polyTypeToggles.Values, true);

            SyncPolyToggles(prim);
            SyncPolyTransform(prim);

            var tex = EditorContext.TextureDatabase[prim.Material.TexturePath];
            if (tex == null)
            {
                tex = EditorContext.TextureDatabase.NoTextureFoundPlaceholder;
            }
            texturePreview.image = tex;
        }

        private void OnPolyDeselected(EditablePrimitive prim)
        {
            xField.SetEnabled(false);
            yField.SetEnabled(false);
            zField.SetEnabled(false);
            roomField.SetEnabled(false);
            pickTextureButton.SetEnabled(false);
            SetObjectEnabled(polyTypeToggles.Values, false);
            texturePreview.image = null;
        }

        private void OnVertSelected(EditableVertex vert)
        {
            uField.SetEnabled(true);
            vField.SetEnabled(true);
            nxField.SetEnabled(true);
            nyField.SetEnabled(true);
            nzField.SetEnabled(true);
            colorRField.SetEnabled(true);
            colorGField.SetEnabled(true);
            colorBField.SetEnabled(true);

            var v = vert.primitive.info.vertices[vert.vertIndex];
            SetFloatFieldValue(uField, v.uv.x);
            SetFloatFieldValue(vField, v.uv.y);
            SetFloatFieldValue(nxField, v.normal.x);
            SetFloatFieldValue(nyField, v.normal.y);
            SetFloatFieldValue(nzField, v.normal.z);
            SetColorFields(v.color);
        }

        private void OnVertDeselected(EditableVertex vert)
        {
            uField.SetEnabled(false);
            vField.SetEnabled(false);
            nxField.SetEnabled(false);
            nyField.SetEnabled(false);
            nzField.SetEnabled(false);
            colorRField.SetEnabled(false);
            colorGField.SetEnabled(false);
            colorBField.SetEnabled(false);

            UpdateUV();
            UpdateNormal();
        }

        private void SyncPolyToggles(EditablePrimitive prim)
        {
            syncing = true;
            foreach (var kv in polyTypeToggles)
            {
                kv.Value.value = prim.info.polyType.HasFlag(kv.Key);
            }
            syncing = false;
        }

        private void SyncPolyTransform(EditablePrimitive prim)
        {
            //transform is driven by the gizmo target, not the polygon itself
            if (Gizmo_OLD.Instance.Target != null)
            {
                OnGizmoMove();
            }
            roomField.SetValueWithoutNotify(prim.info.room);
        }

        private void OnXChanged(ChangeEvent<float> evt) => UpdateGizmo();
        private void OnYChanged(ChangeEvent<float> evt) => UpdateGizmo();
        private void OnZChanged(ChangeEvent<float> evt) => UpdateGizmo();

        private void UpdateGizmo()
        {
            var instance = Gizmo_OLD.Instance;
            if (instance == null || instance.Target == null)
            {
                return;
            }
            var pos = instance.Target.position;
            pos.x = xField.value;
            pos.y = yField.value;
            pos.z = zField.value;
            instance.Target.position = pos;
        }

        private void OnGizmoMove()
        {
            var instance = Gizmo_OLD.Instance;
            if (instance == null || instance.Target == null)
            {
                return;
            }
            var pos = instance.Target.position;
            SetFloatFieldValue(xField, pos.x);
            SetFloatFieldValue(yField, pos.y);
            SetFloatFieldValue(zField, pos.z);
        }

        private void OnRoomChanged(ChangeEvent<int> evt)
        {
            var poly = PolygonSelector.CurrentlySelected;
            if (poly != null)
            {
                poly.info.room = (short)evt.newValue;
            }
        }

        private void OnUChanged(ChangeEvent<float> evt) => UpdateUV();
        private void OnVChanged(ChangeEvent<float> evt) => UpdateUV();

        private void UpdateUV()
        {
            var vert = VertexSelector.CurrentlySelected;
            if (vert == null)
            {
                return;
            }
            vert.primitive.info.vertices[vert.vertIndex].uv = new Vector2(uField.value, vField.value);
            vert.primitive.UpdateMesh();
        }

        private void OnNormalChanged(ChangeEvent<float> evt) => UpdateNormal();

        private void UpdateNormal()
        {
            var vert = VertexSelector.CurrentlySelected;
            if (vert == null)
            {
                return;
            }
            var normal = new Vector3(nxField.value, nyField.value, nzField.value);
            if (normal.sqrMagnitude > 0.0001f)
            {
                normal.Normalize();
            }
            vert.primitive.info.vertices[vert.vertIndex].normal = normal;
            SetFloatFieldValue(nxField, normal.x);
            SetFloatFieldValue(nyField, normal.y);
            SetFloatFieldValue(nzField, normal.z);
            vert.primitive.UpdateMesh();
        }

        private void OnColorChanged(ChangeEvent<float> evt)
        {
            var vert = VertexSelector.CurrentlySelected;
            if (vert == null)
            {
                return;
            }
            var color = new Color(colorRField.value, colorGField.value, colorBField.value, 1);
            vert.primitive.info.vertices[vert.vertIndex].color = color;
            UpdateColorPreview(color);
            vert.primitive.UpdateMesh();
        }

        private void SetColorFields(Color color)
        {
            SetFloatFieldValue(colorRField, color.r);
            SetFloatFieldValue(colorGField, color.g);
            SetFloatFieldValue(colorBField, color.b);
            UpdateColorPreview(color);
        }

        private void UpdateColorPreview(Color color)
        {
            colorPreview.style.backgroundColor = color;
        }

        private void PickTextureClicked()
        {
            var poly = PolygonSelector.CurrentlySelected;
            if (poly == null)
            {
                return;
            }
            var directory = Path.GetDirectoryName(Path.Combine(ArxLibertatisEditorIO.ArxPaths.DataDir, poly.Material.TexturePath));
            StandaloneFileBrowser.OpenFilePanelAsync("Open Texture", directory,
                new[]
                {
                    new ExtensionFilter("Arx Texture Files", "jpg", "bmp"),
                    new ExtensionFilter("All Files", "*"),
                },
                false,
                paths =>
                {
                    if (paths.Length == 0)
                    {
                        return;
                    }
                    var path = paths[0];
                    if (!path.StartsWith(ArxLibertatisEditorIO.ArxPaths.DataDir))
                    {
                        Debug.LogWarning("file is not in datadir, cant use it as a texture");
                        return;
                    }
                    var relPath = PathUtil.GetRelativePath(ArxLibertatisEditorIO.ArxPaths.DataDir, path);
                    var em = new EditorMaterial(relPath, poly.info.polyType, poly.Material.TransVal);
                    poly.Material = em;
                    texturePreview.image = em.Material.mainTexture;
                });
        }

        private static void SetObjectEnabled(IEnumerable<Toggle> toggles, bool enabled)
        {
            foreach (var toggle in toggles)
            {
                toggle.SetEnabled(enabled);
            }
        }
    }
}