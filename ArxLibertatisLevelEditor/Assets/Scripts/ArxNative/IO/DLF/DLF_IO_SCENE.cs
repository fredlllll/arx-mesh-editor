using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DLF_IO_SCENE
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] pad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public float[] fpad;
    }
}
