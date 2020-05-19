using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    [StructLayout(LayoutKind.Sequential)]
    public class DANAE_LIGHTINGHEADER
    {
        public int numLights;
        public int viewMode; // unused
        public int modeLight; // unused
        public int ipad;
    }
}
