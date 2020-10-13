using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EERIE_IO_ROOM_DATA
    {
        public int nb_portals;
        public int nb_polys;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] padd;
    }
}
