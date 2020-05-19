using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public class FTS_EP_DATA
    {
        public short px;
        public short py;
        public short idx;
        public short padd;
    }
}
