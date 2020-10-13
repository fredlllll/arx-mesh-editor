using Assets.Scripts.ArxNative.IO.Shared_IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FTS_IO_EERIEPOLY
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public FTS_IO_VERTEX[] vertices;
        public int tex;
        public SavedVec3 norm;
        public SavedVec3 norm2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public SavedVec3[] normals;
        public float transval;
        public float area;
        public PolyType type;
        public short room;
        public short paddy;
    }
}
