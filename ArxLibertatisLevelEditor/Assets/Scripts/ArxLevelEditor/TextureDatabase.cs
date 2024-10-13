using External;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor
{
    public class TextureDatabase
    {
        readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public IReadOnlyDictionary<string, Texture2D> Textures
        {
            get { return textures; }
        }

        public Texture2D this[string arxPath]
        {
            get
            {
                return GetTexture(arxPath);
            }
        }

        public Texture2D GetTexture(string arxPath)
        {
            if (arxPath == null || arxPath.Length == 0)
            {
                return null;
            }
            var path = Path.Combine(ArxLibertatisEditorIO.ArxPaths.DataDir, arxPath);
            path = Path.GetFullPath(path); //in case there are relative path things in there like .. or .
            path = GetRealTexturePath(path); //in case the extension isnt the right one
            if (!textures.TryGetValue(path, out Texture2D retval))
            {
                //Load texture if it doesnt exist
                retval = LoadTexture(path);
                textures[path] = retval;
            }
            return retval;
        }

        /// <summary>
        /// returns the real path to a texture if it exists, otherwise null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetRealTexturePath(string path)
        {
            int lastDot = path.LastIndexOf('.');
            if (lastDot >= 0)
            {
                path = path.Substring(0, lastDot); //strip extension
            }

            //because textures come as either jpg or bmp, extensions sometimes dont match up with the level files
            if (File.Exists(path + ".jpg"))
            {
                path += ".jpg";
            }
            else if (File.Exists(path + ".bmp"))
            {
                path += ".bmp";
            }
            return path;
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
