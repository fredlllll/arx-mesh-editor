using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS_IO
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FTS_IO_UNIQUE_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] path;
        public int count;
        public float version;
        public int uncompressedsize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] pad;
    }
}
