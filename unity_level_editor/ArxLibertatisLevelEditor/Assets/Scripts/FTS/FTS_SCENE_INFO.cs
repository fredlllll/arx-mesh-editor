using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public class FTS_SCENE_INFO
    {
        public int nbpoly;
        public int nbianchors;
    }
}
