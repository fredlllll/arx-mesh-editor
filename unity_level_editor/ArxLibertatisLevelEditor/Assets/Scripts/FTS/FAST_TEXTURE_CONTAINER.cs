using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FAST_TEXTURE_CONTAINER
    {
        public int tc;
        public int temp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] fic;
    }
}
