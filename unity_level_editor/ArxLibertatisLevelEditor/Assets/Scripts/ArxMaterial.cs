using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ArxMaterial
    {
        public Material Material { get; private set; }
        public string textureArxPath;
        public PolyType polyType;

        public ArxMaterial(string texArxPath, PolyType polyType)
        {
            textureArxPath = texArxPath;
            this.polyType = polyType;

            bool doubleSided = polyType.HasFlag(PolyType.DOUBLESIDED);
            bool transparent = polyType.HasFlag(PolyType.TRANS);
            bool water = polyType.HasFlag(PolyType.WATER);
            bool glow = polyType.HasFlag(PolyType.GLOW);
            bool lava = polyType.HasFlag(PolyType.LAVA);
            bool fall = polyType.HasFlag(PolyType.FALL);

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
            mat.name = System.IO.Path.GetFileNameWithoutExtension(texArxPath); //TODO: probably breaks on linux cause of dir seperator
            mat.mainTexture = TexturesCache.GetTexture(texArxPath);
            if (transparent)
            {//transparents look better with point filtering
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
            Material = mat;
        }
    }
}
