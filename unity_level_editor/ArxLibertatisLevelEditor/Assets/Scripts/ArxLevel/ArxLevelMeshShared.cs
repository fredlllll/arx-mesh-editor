using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.ArxLevel
{
    public static class ArxLevelMeshShared
    {

        static Dictionary<string, Material> materialsCache = new Dictionary<string, Material>();

        public static Material GetMaterial(string arxPath)
        {
            if(materialsCache.TryGetValue(arxPath,out Material retval))
            {
                return retval;
            }

            retval = Object.Instantiate(MaterialsDatabase.ArxLevelBackground);
            var tex = TexturesCache.GetTexture(arxPath);
            retval.mainTexture = tex;

            materialsCache[arxPath] = retval;

            return retval;
        }
    }
}