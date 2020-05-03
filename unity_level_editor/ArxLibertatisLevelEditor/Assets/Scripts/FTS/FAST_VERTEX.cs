using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTS
{
	[StructLayout(LayoutKind.Sequential)]
	public struct FAST_VERTEX
	{
		public float posY;
		public float posX;
		public float posZ;
		public float texU;
		public float texV;
	}
}
