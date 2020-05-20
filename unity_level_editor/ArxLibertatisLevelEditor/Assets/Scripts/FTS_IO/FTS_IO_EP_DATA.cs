using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS_IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FTS_IO_EP_DATA
    {
        public short px;
        public short py;
        public short idx;
        public short padd;
    }
}
