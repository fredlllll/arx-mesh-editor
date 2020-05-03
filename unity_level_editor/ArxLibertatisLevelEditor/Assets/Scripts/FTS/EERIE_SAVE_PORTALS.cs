using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
	[StructLayout(LayoutKind.Sequential)]
	public struct EERIE_SAVE_PORTALS
	{
		public SAVE_EERIEPOLY poly;
		public int room_1; // facing normal
		public int room_2;
		public short useportal;
		public short paddy;
	}
}
