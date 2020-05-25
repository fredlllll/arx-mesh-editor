using Assets.Scripts.Shared_IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTL_IO
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EERIE_OLD_VERTEX
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] unused;
        public SavedVec3 vert;
        public SavedVec3 norm;
    }
}
