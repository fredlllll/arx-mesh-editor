using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class ArxIOHelper
    {
        public static string GetString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(ArrayHelper.TrimEnd<byte>(bytes, 0));
        }

        public static byte[] GetBytes(string str, int bytesLength)
        {
            return ArrayHelper.FixArrayLength<byte>(Encoding.ASCII.GetBytes(str), bytesLength, 0);
        }

        public static Color FromBGRA(uint bgra)
        {
            //return Color.red;

            byte[] bytes = BitConverter.GetBytes(bgra);
            return new Color(bytes[2] / 255f, bytes[1] / 255f, bytes[0] / 255f);
        }
    }
}
