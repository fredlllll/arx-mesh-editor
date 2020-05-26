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

        public static Material ArxLevelBackground { get; private set; }

        static MaterialsDatabase()
        {
            ArxLevelBackground = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/ArxLevelBackground"));
            TEST = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/TEST"));
        }
    }
}
