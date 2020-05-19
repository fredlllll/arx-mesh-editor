using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class FTS_UNIQUE_HEADER2
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] path;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public char[] check;
    }
}
