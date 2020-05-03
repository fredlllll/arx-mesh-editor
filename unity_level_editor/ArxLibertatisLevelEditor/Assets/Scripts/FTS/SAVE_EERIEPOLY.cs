using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SAVE_EERIEPOLY
    {
        public int type; // at least 16 bits
        public SavedVec3 min;
        public SavedVec3 max;
        public SavedVec3 norm;
        public SavedVec3 norm2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public SavedTextureVertex[] v;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 * 4)]
        public char[] unused;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public SavedVec3[] nrml;
        public int tex;
        public SavedVec3 center;
        public float transval;
        public float area;
        public short room;
        public short misc;
    }
}
