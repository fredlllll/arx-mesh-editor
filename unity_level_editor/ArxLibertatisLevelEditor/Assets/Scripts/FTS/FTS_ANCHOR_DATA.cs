using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public class FTS_ANCHOR_DATA
    {
        public SavedVec3 pos;
        public float radius;
        public float height;
        public short nb_linked;
        public short flags;
    }
}
