using Assets.Scripts.Data;
using Assets.Scripts.Shared_IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS_IO
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EERIE_IO_EERIEPOLY
    {
        public int type; // at least 16 bits
        public SavedVec3 min;
        public SavedVec3 max;
        public SavedVec3 norm;
        public SavedVec3 norm2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public SavedTextureVertex[] v;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 * 4)]
        public byte[] unused; //TODO: apparently this does hold data, question is what kind of data...
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
