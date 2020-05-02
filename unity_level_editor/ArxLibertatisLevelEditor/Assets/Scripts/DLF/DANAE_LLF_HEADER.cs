using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DANAE_LLF_HEADER
    {
        public float version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] ident;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] lastuser;
        public int time;
        public int nb_lights;
        public int nb_Shadow_Polys;
        public int nb_IGNORED_Polys;
        public int nb_bkgpolys;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public int[] pad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public float[] fpad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public char[] cpad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public int[] bpad;
    }
}
