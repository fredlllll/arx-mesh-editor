using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class MaterialsDatabase
    {
        public static Material TEST { get; private set; }
        public static Material ArxMat { get; private set; }
        public static Material ArxMatDoubleSided { get; private set; }
        public static Material ArxMatTransparent { get; private set; }
        public static Material ArxMatDoubleSidedTransparent { get; private set; }
        public static Material NotFound { get; private set; }

        static MaterialsDatabase()
        {
            ArxMat = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/ArxMat"));
            ArxMatDoubleSided = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/ArxMatDoubleSided"));
            ArxMatTransparent = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/ArxMatTransparent"));
            ArxMatDoubleSidedTransparent = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/ArxMatDoubleSidedTransparent"));

            NotFound = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/NotFound"));
            TEST = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/TEST"));
        }
    }
}
