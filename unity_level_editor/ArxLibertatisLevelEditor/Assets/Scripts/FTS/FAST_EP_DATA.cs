using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FAST_EP_DATA
    {
        public short px;
        public short py;
        public short idx;
        public short padd;
    }
}
