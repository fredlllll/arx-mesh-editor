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
    public struct FAST_SCENE_HEADER
    {
        public float version;
        public int sizex;
        public int sizez;
        public int nb_textures;
        public int nb_polys;
        public int nb_anchors;
        public SavedVec3 playerpos;
        public SavedVec3 Mscenepos;
        public int nb_portals;
        public int nb_rooms;
    }
}
