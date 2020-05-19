using Assets.Scripts.Data;
using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential)]
    public class DANAE_LIGHT
    {
        public SavedVec3 pos;
        public SavedColor rgb;
        public float fallStart;
        public float fallEnd;
        public float intensity;
        public float i;
        public SavedColor ex_flicker;
        public float ex_radius;
        public float ex_frequency;
        public float ex_size;
        public float ex_speed;
        public float ex_flaresize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public float[] fpad;
        public int extras;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
        public int[] ipad;
    }
}
