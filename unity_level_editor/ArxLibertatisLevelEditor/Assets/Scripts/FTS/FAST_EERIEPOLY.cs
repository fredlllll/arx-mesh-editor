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
    public struct FAST_EERIEPOLY
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public FAST_VERTEX[] vertices;
        public int tex;
        public SavedVec3 norm;
        public SavedVec3 norm2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public SavedVec3[] normals;
        public float transval;
        public float area;
        public int type;
        public short room;
        public short paddy;
    }
}
