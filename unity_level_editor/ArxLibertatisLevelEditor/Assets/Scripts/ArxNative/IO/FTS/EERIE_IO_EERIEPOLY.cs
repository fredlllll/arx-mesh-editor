using Assets.Scripts.ArxNative.IO.Shared_IO;
using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EERIE_IO_EERIEPOLY
    {
        public int type;
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
