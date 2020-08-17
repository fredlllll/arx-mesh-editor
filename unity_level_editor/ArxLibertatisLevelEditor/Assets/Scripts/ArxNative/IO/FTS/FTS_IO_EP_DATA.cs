using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
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
