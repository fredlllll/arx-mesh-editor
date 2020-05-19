using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public class FTS_EERIEPOLY
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public FTS_VERTEX[] vertices;
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
