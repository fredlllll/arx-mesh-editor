using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DANAE_LS_INTER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public char[] name;
        public SavedVec3 pos;
        public SavedAnglef angle;
        public int ident;
        public int flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        public int[] pad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public float[] fpad;
    }
}
