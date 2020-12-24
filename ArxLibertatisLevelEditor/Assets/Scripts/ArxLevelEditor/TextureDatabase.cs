using External;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor
{
    public class TextureDatabase
    {
        readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public Texture2D this[string path]
        {
            get
            {
                if (path == null || path.Length == 0)
                {
                    return null;
                }
                path = Path.GetFullPath(path);
                if (!textures.TryGetValue(path, out Texture2D retval))
                {
                    //Load texture if it doesnt exist
                    retval = LoadTexture(path);
                    textures[path] = retval;
                }
                return retval;
            }
        }

        public void Clear()
        {
            foreach (var kv in textures)
            {
                //free old textures
                UnityEngine.Object.Destroy(kv.Value);
            }

            textures.Clear();
        }

        /// <summary>
        /// returns the real path to a texture if it exists, otherwise null
        /// </summary>
        /// <param name="arxPath"></param>
        /// <returns></returns>
        public static string GetRealTexturePath(string arxPath)
        {
            int lastDot = arxPath.LastIndexOf('.');
            string absPath = arxPath.Substring(0, lastDot); //strip extension

            //because textures come as either jpg or bmp, extensions sometimes dont match up with the level files
            if (File.Exists(absPath + ".jpg"))
            {
                absPath += ".jpg";
            }
            else if (File.Exists(absPath + ".bmp"))
            {
                absPath += ".bmp";
            }
            return absPath;
        }


        static readonly BMPLoader bmpLoader = new BMPLoader();
        /// <summary>
        /// loads jpg and bmp images into a texture and returns it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string path)
        {
            int lastDot = path.LastIndexOf('.');
            string ext = path.Substring(lastDot + 1);

            if (ext == "jpg")
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    var tex = new Texture2D(1, 1);
                    tex.LoadImage(bytes); //this can only do jpg, png, exr and tga (last two unsure)
                    return tex;
                }
            }
            else if (ext == "bmp")
            {
                var bmp = bmpLoader.LoadBMP(path);
                return bmp.ToTexture2D();
            }

            return null;
        }
    }
}
