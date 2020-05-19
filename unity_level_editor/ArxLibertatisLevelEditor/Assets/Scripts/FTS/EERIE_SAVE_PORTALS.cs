using System.Runtime.InteropServices;

namespace Assets.Scripts.FTS
{
    [StructLayout(LayoutKind.Sequential)]
	public class EERIE_SAVE_PORTALS
	{
		public SAVED_EERIEPOLY poly;
		public int room_1; // facing normal
		public int room_2;
		public short useportal;
		public short paddy;
	}
}
