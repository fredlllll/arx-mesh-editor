using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ArxNative.IO.PK
{
    public enum ImplodeLiteralSize : byte
    {
        Fixed = 0,
        Variable = 1,
    }

    public enum ImplodeDictSize : byte
    {
        Size1024 = 4,
        Size2048 = 5,
        Size4096 = 6,
    }

    public struct ImplodeHeader
    {
        public ImplodeLiteralSize literalSize;
        public ImplodeDictSize dictSize;
    }
    public static class Implode
    {
        public static byte[] DoImplode(byte[] bytes)
        {
            MemoryStream output = new MemoryStream();

            var header = new ImplodeHeader();

            header.literalSize = ImplodeLiteralSize.Fixed;
            header.dictSize = ImplodeDictSize.Size1024;

            using(var sw = new StructWriter(output, Encoding.UTF8, true))
            {
                sw.WriteStruct(header);
            }

            BitStream bits = new BitStream();

            for(int i =0; i< bytes.Length; i++)
            {
                bits.WriteFixedLiteral(bytes[i]);
            }
            bits.WriteEOS();
            while (bits.ByteReady())
            {
                output.WriteByte(bits.GetByte());
            }
            output.WriteByte(bits.GetBytePadded());

            return output.ToArray();
        }
    }
}
