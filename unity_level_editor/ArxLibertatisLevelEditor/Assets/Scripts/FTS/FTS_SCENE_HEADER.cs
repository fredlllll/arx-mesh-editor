using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public class FTS_SCENE_HEADER
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
