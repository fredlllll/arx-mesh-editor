using Assets.Scripts.ArxNative;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Material
{
    public class EditorMaterial : EditorMaterialBase
    {
        private UnityEngine.Material mat;
        public UnityEngine.Material Material
        {
            get
            {
                if (mat == null)
                {
                    mat = CreateMaterial(this);
                }
                return mat;
            }
        }

        public EditorMaterial(string texPath, PolyType polyType, float transVal)
        {
            TexturePath = texPath;
            PolygonType = polyType;
            TransVal = transVal;
        }

        public static UnityEngine.Material CreateMaterial(EditorMaterialBase editorMat)
        {
            bool doubleSided = editorMat.PolygonType.HasFlag(PolyType.DOUBLESIDED);
            bool transparent = editorMat.PolygonType.HasFlag(PolyType.TRANS);
            bool water = editorMat.PolygonType.HasFlag(PolyType.WATER);
            bool glow = editorMat.PolygonType.HasFlag(PolyType.GLOW);
            bool lava = editorMat.PolygonType.HasFlag(PolyType.LAVA);
            bool fall = editorMat.PolygonType.HasFlag(PolyType.FALL);

            UnityEngine.Material mat;
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
            mat.name = editorMat.TexturePath;
            mat.mainTexture = LevelEditor.TextureDatabase[editorMat.TexturePath];
            if (transparent)
            {//transparents look better with point filtering
                //TODO: check transval
                if (mat.mainTexture != null)
                {
                    mat.mainTexture.filterMode = FilterMode.Point;//should do this on an instance of the tex, but that would break the caching.. ughhhh TODO!!!
                }
            }
            //The following keywords cause scrolling of the texture etc, so its done using shader variants
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
}
