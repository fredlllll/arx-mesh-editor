using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class FTS_TEXTURE_CONTAINER
    {
        public int tc;
        public int temp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] fic;
    }
}
