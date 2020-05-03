using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EERIE_SAVE_ROOM_DATA
    {
        public int nb_portals;
        public int nb_polys;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] padd;
    }
}
