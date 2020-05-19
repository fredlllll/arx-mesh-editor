using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DLF_PATHWAYS
    {
        public SavedVec3 rpos;
        public int flag;
        public uint time;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] fpadd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] lpadd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] cpadd;
    }
}
