using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ArxMaterialManager
    {
        static Dictionary<ArxMaterialKey, ArxMaterial> arxLevelMaterials = new Dictionary<ArxMaterialKey, ArxMaterial>();

        public static Material GetMaterial(ArxMaterialKey key)
        {
            if (!arxLevelMaterials.TryGetValue(key, out ArxMaterial retval))
            {
                retval = new ArxMaterial(key.TextureArxPath, key.PolyType, key.TransVal);
                arxLevelMaterials[key] = retval;
            }
            return retval.Material;
        }

        public static Material GetMaterial(string texArxPath, PolyType polyType, float transVal)
        {
            var key = new ArxMaterialKey(texArxPath, polyType, transVal);
            return GetMaterial(key);
        }
    }
}
