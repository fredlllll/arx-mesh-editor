using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.ArxLevel
{
    public static class ArxLevelMeshMaterials
    {

        static Dictionary<string, Material> materials = new Dictionary<string, Material>();

        public static Material GetMaterial(string path)
        {
            if(materials.TryGetValue(path,out Material retval))
            {
                return retval;
            }

            retval = Object.Instantiate(MaterialsDatabase.ArxLevelBackground);

            var tex = new Texture2D(1, 1);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                ImageConversion.LoadImage(tex, bytes);
            }
            retval.mainTexture = tex;

            materials[path] = retval;

            return retval;
        }
    }
}