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
        public class ArxMaterialKey : IEquatable<ArxMaterialKey>
        {
            public string textureArxPath;
            public PolyType polyType;

            public ArxMaterialKey(string texArxPath, PolyType polyType)
            {
                textureArxPath = texArxPath;
                this.polyType = polyType;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ArxMaterialKey);
            }

            public bool Equals(ArxMaterialKey other)
            {
                return other != null &&
                       textureArxPath == other.textureArxPath &&
                       polyType == other.polyType;
            }

            public override int GetHashCode()
            {
                var hashCode = 676519645;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(textureArxPath);
                hashCode = hashCode * -1521134295 + polyType.GetHashCode();
                return hashCode;
            }
        }

        static Dictionary<ArxMaterialKey, ArxMaterial> arxLevelMaterials = new Dictionary<ArxMaterialKey, ArxMaterial>();

        public static Material GetArxLevelMaterial(string texArxPath, PolyType polyType)
        {
            var key = new ArxMaterialKey(texArxPath, polyType);
            if (!arxLevelMaterials.TryGetValue(key, out ArxMaterial retval))
            {
                retval = new ArxMaterial(texArxPath, polyType);
                arxLevelMaterials[key] = retval;
            }
            return retval.Material;
        }
    }
}
