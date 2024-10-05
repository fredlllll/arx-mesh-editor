using ArxLibertatisEditorIO.Util;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor
{
    public class EditorMaterial : IEquatable<EditorMaterial>
    {
        UnityEngine.Material mat = null;
        public UnityEngine.Material Material
        {
            get
            {
                if(mat == null)
                {
                    mat = CreateMaterial(this);
                }
                return mat;
            }
        }

        public string TexturePath
        {
            get;
            protected set;
        }

        public PolyType PolygonType
        {
            get;
            protected set;
        }

        public float TransVal
        {
            get;
            protected set;
        }

        //only these types will be used to differentiate between materials. for example QUAD has nothing to do with the material
        public const PolyType materialPolyTypes = PolyType.DOUBLESIDED | PolyType.FALL | PolyType.GLOW | PolyType.LAVA | PolyType.TRANS | PolyType.WATER;

        public EditorMaterial(string texArxPath, PolyType polyType, float transVal)
        {
            TexturePath = texArxPath;
            PolygonType = polyType & materialPolyTypes;
            TransVal = transVal;
        }

        ~EditorMaterial()
        {
            if(mat != null)
            {
                //prevent memory leaks
                MainThreadDestroyer.AddForDestruction(mat);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EditorMaterial);
        }

        public bool Equals(EditorMaterial other)
        {
            return other != null &&
                   TexturePath == other.TexturePath &&
                   PolygonType == other.PolygonType &&
                   TransVal == other.TransVal;
        }

        public override int GetHashCode()
        {
            var hashCode = 1482638751;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TexturePath);
            hashCode = hashCode * -1521134295 + PolygonType.GetHashCode();
            hashCode = hashCode * -1521134295 + TransVal.GetHashCode();
            return hashCode;
        }

        private static UnityEngine.Material CreateMaterial(EditorMaterial editorMat)
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
