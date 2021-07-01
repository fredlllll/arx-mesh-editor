using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FTS_IO_EP_DATA
    {
        public short cell_x; //x
        public short cell_z; //y
        public short idx; //poly index in cell
        public short padd;
    }
}
