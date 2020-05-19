using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public class ROOM_DIST_DATA_SAVE
    {
        public float distance; // -1 means use truedist
        public SavedVec3 startpos;
        public SavedVec3 endpos;
    }
}
