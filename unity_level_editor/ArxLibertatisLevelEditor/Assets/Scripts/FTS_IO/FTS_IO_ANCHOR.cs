using Assets.Scripts.Util;
using System.Collections.ObjectModel;

namespace Assets.Scripts.FTS_IO
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
    }
}
