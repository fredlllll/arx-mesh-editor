using Assets.Scripts.ArxLevelEditor.Preloading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.ArxLevelEditor
{
    /*public class Preloader : MonoBehaviour
    {
        public Text statusText;

        private IEnumerator Start()
        {
            //preload textures
            statusText.text = "Preloading Textures (Calculating)";
            Texture.allowThreadedTextureCreation = true;

            var co = StartCoroutine(TexturesPreloader.PreloadTextures());

            while(TexturesPreloader.LoaderStatus != TexturesPreloader.Status.Done)
            {
                if (TexturesPreloader.LoaderStatus == TexturesPreloader.Status.Loading)
                {
                    statusText.text = "Preloading Textures (" + TexturesPreloader.Loaded + "/" + TexturesPreloader.Total + ")";
                }
                yield return new WaitForEndOfFrame();
            }

            Texture.allowThreadedTextureCreation = false;


            //load editor
            SceneManager.LoadScene("EditorMain", LoadSceneMode.Single);
        }
    }*/
}
