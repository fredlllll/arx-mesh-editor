using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FAST_ANCHOR_DATA
    {
        public SavedVec3 pos;
        public float radius;
        public float height;
        public short nb_linked;
        public short flags;
    }
}
