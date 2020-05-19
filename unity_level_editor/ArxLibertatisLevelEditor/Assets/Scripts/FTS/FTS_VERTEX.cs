using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
	public class FTS_VERTEX
	{
		public float posY;
		public float posX;
		public float posZ;
		public float texU;
		public float texV;
	}
}
