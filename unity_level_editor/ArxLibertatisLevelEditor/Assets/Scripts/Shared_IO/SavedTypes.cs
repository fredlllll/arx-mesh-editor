using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts.Shared_IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SavedColor
    {
        public float r;
        public float g;
        public float b;

        public SavedColor(Color col)
        {
            r = col.r;
            g = col.g;
            b = col.b;
        }

        public Color ToColor()
        {
            return new Color(r, g, b);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SavedVec3
    {
        public float x;
        public float y;
        public float z;

        public SavedVec3(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SavedAnglef
    {
        public float a;
        public float b;
        public float g;

        public SavedAnglef(Vector3 euler)
        {
            a = euler.x; //TODO: check if this is rads or degrees and if its flipped or anything
            b = euler.y+90;
            g = euler.z;
        }

        public Vector3 ToEuler()
        {
            return new Vector3(a, b-90, g); //TODO: seems that rotation is handled differently depending on what type of object its used on...
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SavedTextureVertex
    {
        public SavedVec3 pos;
        public float rhw;
        public uint color;
        public uint specular;
        public float tu;
        public float tv;
    }
}
