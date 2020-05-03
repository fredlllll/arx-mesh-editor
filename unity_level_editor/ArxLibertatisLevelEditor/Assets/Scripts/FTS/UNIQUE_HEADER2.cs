using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct UNIQUE_HEADER2
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] path;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public char[] check;
    }
}
