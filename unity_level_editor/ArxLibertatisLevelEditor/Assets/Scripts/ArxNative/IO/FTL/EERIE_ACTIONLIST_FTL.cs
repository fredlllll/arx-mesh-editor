using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTL
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EERIE_ACTIONLIST_FTL
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] name;
        public int idx; // index vertex;
        public int action;
        public int sfx;
    }
}
