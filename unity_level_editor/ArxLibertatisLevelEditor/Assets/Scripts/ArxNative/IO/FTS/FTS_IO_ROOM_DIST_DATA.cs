using Assets.Scripts.ArxNative.IO.Shared_IO;
using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FTS_IO_ROOM_DIST_DATA
    {
        public float distance; // -1 means use truedist
        public SavedVec3 startpos;
        public SavedVec3 endpos;
    }
}
