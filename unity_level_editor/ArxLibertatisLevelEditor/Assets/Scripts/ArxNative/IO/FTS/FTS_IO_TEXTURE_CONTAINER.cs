using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FTS_IO_TEXTURE_CONTAINER
    {
        public int tc;
        public int temp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] fic;
    }
}
