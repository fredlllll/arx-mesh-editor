using Assets.Scripts.Util;
using System.Collections.ObjectModel;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    public struct FTS_IO_ROOM
    {
        public EERIE_IO_ROOM_DATA data;
        public int[] portals;
        public FTS_IO_EP_DATA[] polygons; //not sure if this are actually polygons or just the vertices

        public void ReadFrom(StructReader reader)
        {
            data = reader.ReadStruct<EERIE_IO_ROOM_DATA>();

            portals = new int[data.nb_portals];
            for (int i = 0; i < data.nb_portals; i++)
            {
                portals[i] = reader.ReadInt32();
            }

            polygons = new FTS_IO_EP_DATA[data.nb_polys];
            for (int i = 0; i < data.nb_polys; i++)
            {
                polygons[i] = reader.ReadStruct<FTS_IO_EP_DATA>();
            }
        }

        public void WriteTo(StructWriter writer)
        {
            writer.WriteStruct(data);

            for (int i = 0; i < portals.Length; i++)
            {
                writer.Write(portals[i]);
            }

            for (int i = 0; i < polygons.Length; i++)
            {
                writer.WriteStruct(polygons[i]);
            }
        }
    }
}
