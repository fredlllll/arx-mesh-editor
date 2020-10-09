using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTL
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FTL_IO_TEXTURE_CONTAINER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] name;
    }
}
