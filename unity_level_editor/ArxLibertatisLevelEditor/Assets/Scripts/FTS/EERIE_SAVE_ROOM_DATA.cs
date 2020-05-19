using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class EERIE_SAVE_ROOM_DATA
    {
        public int nb_portals;
        public int nb_polys;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] padd;
    }
}
