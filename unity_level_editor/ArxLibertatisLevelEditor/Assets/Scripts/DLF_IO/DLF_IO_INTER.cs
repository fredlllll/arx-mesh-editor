using Assets.Scripts.Data;
using Assets.Scripts.Shared_IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF_IO
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DLF_IO_INTER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] name;
        public SavedVec3 pos;
        public SavedAnglef angle;
        public int ident;
        public int flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        public int[] ipad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public float[] fpad;
    }
}
