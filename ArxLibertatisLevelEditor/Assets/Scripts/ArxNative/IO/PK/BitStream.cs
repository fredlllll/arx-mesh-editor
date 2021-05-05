using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ArxNative.IO.PK
{
    public class BitStream
    {
        Queue<bool> bits = new Queue<bool>();
        public void WriteFixedLiteral(byte b)
        {
            bits.Enqueue(false);
            for (int i = 0; i < 8; i++)
            {
                if ((b & (1 << i)) != 0)
                {
                    bits.Enqueue(true);
                }
                else
                {
                    bits.Enqueue(false);
                }
            }
        }

        public void WriteEOS()
        {
            bits.Enqueue(true);
            for (int i = 0; i < 7; i++)
            {
                bits.Enqueue(false);
            }
            for (int i = 0; i < 8; i++)
            {
                bits.Enqueue(true);
            }
        }

        public bool ByteReady()
        {
            return bits.Count >= 8;
        }

        public byte GetByte()
        {
            if (bits.Count < 8)
            {
                throw new IndexOutOfRangeException("not enough bits to make a byte");
            }
            byte val = 0;
            for (int i = 0; i < 8; i++)
            {
                bool bit = bits.Dequeue();
                if (bit)
                {
                    val |= (byte)(1 << i);
                }
            }
            return val;
        }

        public byte GetBytePadded()
        {
            byte val = 0;
            for (int i = 0; i < 8 && bits.Count > 0; i++)
            {
                bool bit = bits.Dequeue();
                if (bit)
                {
                    val |= (byte)(1 << i);
                }
            }
            return val;
        }
    }
}
