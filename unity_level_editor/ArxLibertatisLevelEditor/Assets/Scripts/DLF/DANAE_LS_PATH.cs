using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DANAE_LS_PATH
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] name;
        public short idx;
        public short flags;
        public SavedVec3 initpos;
        public SavedVec3 pos;
        public int nb_pathways;
        public SavedColor rgb;
        public float farclip;
        public float reverb;
        public float amb_max_vol;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
        public float[] fpadd;
        public int height;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
        public int[] lpadd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public char[] ambiance;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public char[] cpadd;
    }
}
