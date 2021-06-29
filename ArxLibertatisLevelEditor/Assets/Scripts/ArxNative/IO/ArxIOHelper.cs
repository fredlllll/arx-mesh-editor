using Assets.Scripts.Util;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ArxNative.IO
{
    public static class ArxIOHelper
    {
        /// <summary>
        /// get a string from bytes
        /// be aware this searches for first non 0 char from the end, so you could end up with 0 chars in the string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetString(byte[] bytes)
        {
            int strlen = 0;
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                if (bytes[i] != 0)
                {
                    strlen = i + 1;
                    break;
                }
            }
            string retval = Encoding.ASCII.GetString(bytes, 0, strlen);
            return retval;
        }

        public static byte[] GetBytes(string str, int bytesLength)
        {
            byte[] retval = new byte[bytesLength];
            try
            {
                Encoding.ASCII.GetBytes(str, 0, str.Length, retval, 0);
            }
            catch (ArgumentException) //not enough space in retval for the string
            {
                byte[] bytes = Encoding.ASCII.GetBytes(str);
                Array.Copy(bytes, retval, bytesLength);
            }
            return retval;
        }

        public static Color FromBGRA(uint bgra)
        {
            //return Color.red;

            byte[] bytes = BitConverter.GetBytes(bgra);
            return new Color(bytes[2] / 255f, bytes[1] / 255f, bytes[0] / 255f);
        }

        public static Color FromRGB(uint rgba)
        {
            byte[] bytes = BitConverter.GetBytes(rgba);
            return new Color(bytes[0] / 255f, bytes[1] / 255f, bytes[2] / 255f);
        }

        public static string ArxPathToPlatformPath(string arxPath)
        {
            string[] parts = arxPath.Split('\\');
            return Path.Combine(parts);
        }

        public static string PlatformPathToArxPath(string platformPath)
        {
            string[] parts = platformPath.Split(Path.DirectorySeparatorChar);
            return string.Join("\\", parts);
        }

        public static int XZToCellIndex(int x, int z, int sizex, int sizez)
        {
            return z * sizex + x;
        }

        public static uint ToBGRA(Color color)
        {
            byte[] bytes = new byte[]
            {
                (byte)(color.b*255),
                (byte)(color.g*255),
                (byte)(color.r*255),
                (byte)(color.a*255),
            };

            return BitConverter.ToUInt32(bytes, 0);
        }
    }
}
