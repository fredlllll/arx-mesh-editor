using Assets.Scripts.ArxNative;
using Assets.Scripts.ArxNative.IO;
using Assets.Scripts.Util;
using External;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class TexturesCache
    {
        private static readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static readonly BMPLoader bmpLoader = new BMPLoader();

        public static Texture2D GetTexture(string arxPath)
        {
            string absPath = Path.Combine(ArxDirs.DataDir, ArxIOHelper.ArxPathToPlatformPath(arxPath));
            int lastDot = absPath.LastIndexOf('.');
            string absPathNoExt = absPath.Substring(0, lastDot);

            if (textures.TryGetValue(absPathNoExt, out Texture2D retval))
            {
                return retval;
            }

            //because textures come as either jpg or bmp, extensions sometimes dont match up with the level files
            if (File.Exists(absPathNoExt + ".jpg"))
            {
                absPath = absPathNoExt + ".jpg";

                retval = new Texture2D(1, 1);
                using (var fs = new FileStream(absPath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    ImageConversion.LoadImage(retval, bytes);
                }
            }
            else if (File.Exists(absPathNoExt + ".bmp"))
            {
                absPath = absPathNoExt + ".bmp";
                var bmp = bmpLoader.LoadBMP(absPath);
                retval = bmp.ToTexture2D();
            }

            textures[absPathNoExt] = retval;

            return retval;
        }
    }
}
