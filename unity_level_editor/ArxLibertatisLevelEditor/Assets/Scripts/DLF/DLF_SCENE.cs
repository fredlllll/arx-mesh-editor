using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class DLF_SCENE
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public char[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] pad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public float[] fpad;
    }
}
