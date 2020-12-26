using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.ArxLevelEditor.Editing
{
    public class EditorCameraViewport : MonoBehaviour
    {
        public int widthOffset = 0;
        public int heightOffset = 0;
        private RenderTexture viewport;
        public RawImage targetImage;

        private void RecreateViewport()
        {
            if (viewport != null)
            {
                Destroy(viewport);
            }

            var desc = new RenderTextureDescriptor
            {
                autoGenerateMips = false,
                bindMS = false,
                colorFormat = RenderTextureFormat.RGB111110Float,
                depthBufferBits = 24,
                dimension = UnityEngine.Rendering.TextureDimension.Tex2D,
                enableRandomWrite = false,
                height = Screen.height + heightOffset,
                memoryless = RenderTextureMemoryless.None,
                mipCount = 0,
                msaaSamples = 1,
                shadowSamplingMode = UnityEngine.Rendering.ShadowSamplingMode.CompareDepths,
                sRGB = false,
                stencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.None,
                useDynamicScale = false,
                useMipMap = false,
                volumeDepth = 1,
                width = Screen.width + widthOffset
            };
            viewport = new RenderTexture(desc);

            targetImage.texture = viewport;
            gameObject.GetComponent<Camera>().targetTexture = viewport;
        }

        private void Start()
        {
            RecreateViewport();
        }

        private void Update()
        {
            if (viewport != null)
            {
                if (viewport.width != Screen.width || viewport.height != Screen.height)
                {
                    RecreateViewport();
                }
            }
        }
    }
}
