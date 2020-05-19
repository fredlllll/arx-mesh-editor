using Assets.Scripts.Data;
using Assets.Scripts.Util;
using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DLF_PATH_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] name;
        public short idx;
        public short flags;
        public SavedVec3 initPos;
        public SavedVec3 pos;
        [SetElsewhere("DLF_PATH.Paths.Count")]
        public int numPathways;
        public SavedColor rgb;
        public float farClip;
        public float reverb;
        public float ambientMaxVolume;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
        public float[] fpad;
        public int height;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
        public int[] ipad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public char[] ambiance;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public char[] cpad;
    }
}
