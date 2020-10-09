using Assets.Scripts.ArxNative.IO.Shared_IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DLF_IO_FOG
    {
        public SavedVec3 pos;
        public SavedColor rgb;
        public float size;
        public int special;
        public float scale;
        public SavedVec3 move;
        public SavedAnglef angle;
        public float speed;
        public float rotatespeed;
        public int tolive;
        public int blend;
        public float frequency;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public float[] fpadd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public int[] lpadd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] cpadd;
    }
}
