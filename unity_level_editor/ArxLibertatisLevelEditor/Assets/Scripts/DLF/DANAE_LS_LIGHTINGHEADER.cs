using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DANAE_LS_LIGHTINGHEADER
    {
        public int nb_values;
        public int ViewMode; // unused
        public int ModeLight; // unused
        public int pad;
    }
}
