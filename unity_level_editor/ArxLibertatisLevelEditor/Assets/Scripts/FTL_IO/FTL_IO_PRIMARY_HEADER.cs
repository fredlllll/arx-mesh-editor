using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTL_IO
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FTL_IO_PRIMARY_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] ident; // FTL\0
        public float version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] checksum; //not read by game, so we luckily dont have to provide it
    }
}
