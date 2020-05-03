using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct UNIQUE_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] path;
        public int count;
        public float version;
        public int uncompressedsize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] pad;
    }
}
