using System;
using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.Shared_IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DANAE_IO_LIGHT
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
        public ExtrasType extras;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
        public int[] ipad;
    }

    [Flags]
    public enum ExtrasType : int
    {
        None = 0,
        EXTRAS_SEMIDYNAMIC = 0x00000001,
        EXTRAS_EXTINGUISHABLE = 0x00000002, //can be ignited or doused by spells etc
        EXTRAS_STARTEXTINGUISHED = 0x00000004, //starts extinguished
        EXTRAS_SPAWNFIRE = 0x00000008, //spawns fire particle effect
        EXTRAS_SPAWNSMOKE = 0x00000010, //spawns smoke particle effect
        EXTRAS_OFF = 0x00000020,
        EXTRAS_COLORLEGACY = 0x00000040,
        EXTRAS_NOCASTED = 0x00000080, //unused, if enabled, the light should not cast shadows
        EXTRAS_FIXFLARESIZE = 0x00000100,
        EXTRAS_FIREPLACE = 0x00000200, //can prepare food
        EXTRAS_NO_IGNIT = 0x00000400,
        EXTRAS_FLARE = 0x00000800
    }
}
