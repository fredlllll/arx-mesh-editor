using Assets.Scripts.ArxNative;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.ArxLevelEditor.Material
{
    public class EditorMaterialKey : EditorMaterialBase, IEquatable<EditorMaterialKey>
    {
        //only these types will be used to differentiate between materials. for example QUAD has nothing to do with the material
        const PolyType materialPolyTypes = PolyType.DOUBLESIDED | PolyType.FALL | PolyType.GLOW | PolyType.LAVA | PolyType.TRANS | PolyType.WATER;

        public EditorMaterialKey(string texArxPath, PolyType polyType, float transVal)
        {
            TexturePath = texArxPath;
            PolygonType = polyType & materialPolyTypes;
            TransVal = transVal;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EditorMaterialKey);
        }

        public bool Equals(EditorMaterialKey other)
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

        public EditorMaterial ToMaterial()
        {
            return new EditorMaterial(TexturePath, PolygonType, TransVal);
        }
    }
}
