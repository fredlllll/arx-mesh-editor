using Assets.Scripts.Util;
using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    public struct FTS_IO_ANCHOR
    {
        public FTS_IO_ANCHOR_DATA data;
        public int[] linkedAnchors;

        public void ReadFrom(StructReader reader)
        {
            data = reader.ReadStruct<FTS_IO_ANCHOR_DATA>();

            linkedAnchors = new int[data.nb_linked];
            for (int i = 0; i < data.nb_linked; i++)
            {
                linkedAnchors[i] = reader.ReadInt32();
            }
        }

        public void WriteTo(StructWriter writer)
        {
            writer.WriteStruct(data);

            for (int i = 0; i < linkedAnchors.Length; i++)
            {
                writer.Write(linkedAnchors[i]);
            }
        }

        public int CalculateWrittenSize()
        {
            int size = 0;

            size += Marshal.SizeOf<FTS_IO_ANCHOR_DATA>();
            size += sizeof(int) * linkedAnchors.Length;

            return size;
        }
    }
}
