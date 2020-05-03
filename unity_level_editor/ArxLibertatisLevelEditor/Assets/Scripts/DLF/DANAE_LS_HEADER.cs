using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DANAE_LS_HEADER
    {
        public float version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] ident;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] lastuser;
        public int time;
        public SavedVec3 pos_edit;
        public SavedAnglef angle_edit;
        public int nb_scn;
        public int nb_inter;
        public int nb_nodes;
        public int nb_nodeslinks;
        public int nb_zones;
        public int lighting;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public int[] Bpad;
        public int nb_lights;
        public int nb_fogs;

        public int nb_bkgpolys;
        public int nb_ignoredpolys;
        public int nb_childpolys;
        public int nb_paths;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 250)]
        public int[] pad;
        public SavedVec3 offset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 253)]
        public float[] fpad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public char[] cpad;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public int[] bpad;
    }
}
