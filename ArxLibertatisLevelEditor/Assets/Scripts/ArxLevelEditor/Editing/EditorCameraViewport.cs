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

            var desc = new RenderTextureDescriptor();
            desc.autoGenerateMips = false;
            desc.bindMS = false;
            desc.colorFormat = RenderTextureFormat.ARGB32;
            desc.depthBufferBits = 24;
            desc.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            desc.enableRandomWrite = false;
            desc.height = Screen.height + heightOffset;
            desc.msaaSamples = 1;
            desc.volumeDepth = 1;
            desc.width = Screen.width + widthOffset;
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
