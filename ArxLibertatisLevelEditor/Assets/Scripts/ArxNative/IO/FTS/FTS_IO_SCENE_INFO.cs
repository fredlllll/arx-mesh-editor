using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FTS_IO_SCENE_INFO
    {
        public int nbpoly;
        public int nbianchors;
    }
}
