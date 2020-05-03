using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ROOM_DIST_DATA_SAVE
    {
        public float distance; // -1 means use truedist
        public SavedVec3 startpos;
        public SavedVec3 endpos;
    }
}
