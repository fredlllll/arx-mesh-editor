using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.DLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DLF_IO_VLIGHTING
    {
        public int r;
        public int g;
        public int b;
    }
}
