using Assets.Scripts.ArxLevelEditor.Editing;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class EditorViewport : MonoBehaviour
    {
        private const float textureScale = 2f;

        private UIDocument document;
        private VisualElement viewport;
        private Camera editorCamera;
        private RenderTexture viewportTexture;
        private int textureWidth;
        private int textureHeight;

        public static void Setup(UIDocument document)
        {
            if (EditWindow.ViewportElement != null)
            {
                return;
            }

            var viewportComp = document.GetComponent<EditorViewport>();
            if (viewportComp == null)
            {
                viewportComp = document.gameObject.AddComponent<EditorViewport>();
            }
            viewportComp.Initialize(document);
        }

        private void Initialize(UIDocument doc)
        {
            document = doc;
            viewport = document.rootVisualElement.Q<VisualElement>("Viewport");
            if (viewport == null)
            {
                Debug.LogError("EditorViewport: Could not find 'Viewport' element in the UI document.");
                return;
            }

            EditWindow.ViewportElement = viewport;

            editorCamera = Camera.main;

            viewport.RegisterCallback<PointerDownEvent>(OnPointerDown);
            viewport.RegisterCallback<PointerUpEvent>(OnPointerUp);
            viewport.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            viewport.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            viewport.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        private void OnDisable()
        {
            if (EditWindow.ViewportElement == viewport)
            {
                EditWindow.ViewportElement = null;
            }
        }

        private void OnDestroy()
        {
            if (editorCamera != null)
            {
                editorCamera.targetTexture = null;
            }
            if (viewportTexture != null)
            {
                Destroy(viewportTexture);
            }
            EditWindow.ViewportElement = null;
        }

        private void Update()
        {
            if (viewport == null || editorCamera == null || document == null)
            {
                return;
            }

            float w = viewport.resolvedStyle.width;
            float h = viewport.resolvedStyle.height;
            if (float.IsNaN(w) || float.IsNaN(h) || w <= 0 || h <= 0)
            {
                return;
            }

            int textureW = Mathf.Max(16, Mathf.RoundToInt(w * textureScale));
            int textureH = Mathf.Max(16, Mathf.RoundToInt(h * textureScale));
            if (textureW == textureWidth && textureH == textureHeight)
            {
                return;
            }

            RecreateViewport(textureW, textureH);
        }

        private void RecreateViewport(int width, int height)
        {
            if (viewportTexture != null)
            {
                Destroy(viewportTexture);
            }

            var desc = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 24)
            {
                autoGenerateMips = false,
                dimension = UnityEngine.Rendering.TextureDimension.Tex2D,
                msaaSamples = 1,
                sRGB = false,
                useMipMap = false,
            };
            viewportTexture = new RenderTexture(desc) { name = "EditorViewportRT" };
            textureWidth = width;
            textureHeight = height;

            editorCamera.targetTexture = viewportTexture;
            editorCamera.aspect = (float)width / height;
            viewport.style.backgroundImage = Background.FromRenderTexture(viewportTexture);
            viewport.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Cover);
        }

        private Vector3 UIToLocalEditor(Vector2 localPosition)
        {
            float height = viewport.resolvedStyle.height;
            return new Vector3(localPosition.x, height - localPosition.y, 0);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button < 0)
            {
                return;
            }
            EditWindowClickDetection.HandlePointerDown(UIToLocalEditor(evt.localPosition), evt.button);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (evt.button < 0)
            {
                return;
            }
            EditWindowClickDetection.HandlePointerUp(UIToLocalEditor(evt.localPosition), evt.button);
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            var localPos = UIToLocalEditor(evt.localPosition);
            int pressed = evt.pressedButtons;
            for (int button = 0; button < 3; button++)
            {
                if ((pressed & (1 << button)) != 0)
                {
                    EditWindowClickDetection.HandlePointerMove(localPos, button);
                }
            }
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            EditWindow.MouseInEditWindow = true;
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            EditWindow.MouseInEditWindow = false;
        }
    }
}