using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class ArxMaterialBase
    {
        public string TextureArxPath
        {
            get;
            protected set;
        }
        public PolyType PolyType
        {
            get;
            protected set;
        }

        public float TransVal
        {
            get;
            protected set;
        }
    }

    public class ArxMaterial :ArxMaterialBase
    {
        private Material mat;
        public Material Material
        {
            get
            {
                if (mat == null)
                {
                    mat = GetMaterial();
                }
                return mat;
            }
        }

        public ArxMaterial(string texArxPath, PolyType polyType, float transVal = 0)
        {
            TextureArxPath = texArxPath;
            PolyType = polyType;
            TransVal = transVal;
        }

        public Material GetMaterial()
        {
            bool doubleSided = PolyType.HasFlag(PolyType.DOUBLESIDED);
            bool transparent = PolyType.HasFlag(PolyType.TRANS);
            bool water = PolyType.HasFlag(PolyType.WATER);
            bool glow = PolyType.HasFlag(PolyType.GLOW);
            bool lava = PolyType.HasFlag(PolyType.LAVA);
            bool fall = PolyType.HasFlag(PolyType.FALL);

            Material mat;
            if (doubleSided && transparent)
            {
                mat = MaterialsDatabase.ArxMatDoubleSidedTransparent;
            }
            else if (doubleSided)
            {
                mat = MaterialsDatabase.ArxMatDoubleSided;
            }
            else if (transparent)
            {
                mat = MaterialsDatabase.ArxMatTransparent;
            }
            else
            {
                mat = MaterialsDatabase.ArxMat;
            }
            mat = UnityEngine.Object.Instantiate(mat);
            mat.name = System.IO.Path.GetFileNameWithoutExtension(TextureArxPath); //TODO: probably breaks on linux cause of dir seperator
            mat.mainTexture = TexturesCache.GetTexture(TextureArxPath);
            if (transparent)
            {//transparents look better with point filtering
                //TODO: check transval
                mat.mainTexture.filterMode = FilterMode.Point;//should do this on an instance of the tex, but that would break the caching.. ughhhh TODO!!!
            }
            if (water)
            {
                mat.EnableKeyword("WATER");
            }
            if (lava)
            {
                mat.EnableKeyword("LAVA");
            }
            if (glow)
            {
                mat.EnableKeyword("GLOW");
            }
            if (fall)
            {
                mat.EnableKeyword("FALL");
            }
            return mat;
        }
    }

    public class ArxMaterialKey : ArxMaterialBase, IEquatable<ArxMaterialKey>
    {
        public ArxMaterialKey(string texArxPath, PolyType polyType, float transVal)
        {
            TextureArxPath = texArxPath;
            PolyType = polyType;
            TransVal = transVal;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ArxMaterialKey);
        }

        public bool Equals(ArxMaterialKey other)
        {
            return other != null &&
                   TextureArxPath == other.TextureArxPath &&
                   PolyType == other.PolyType &&
                   TransVal == other.TransVal;
        }

        public override int GetHashCode()
        {
            var hashCode = 1482638751;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TextureArxPath);
            hashCode = hashCode * -1521134295 + PolyType.GetHashCode();
            hashCode = hashCode * -1521134295 + TransVal.GetHashCode();
            return hashCode;
        }
    }
}
