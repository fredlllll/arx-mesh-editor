using Assets.Scripts.Util;
using System.Collections.ObjectModel;

namespace Assets.Scripts.FTS
{
    public class FTS_ROOM
    {
        public EERIE_SAVE_ROOM_DATA Data { get; private set; }
        public ObservableCollection<int> Portals { get; } = new ObservableCollection<int>();
        public ObservableCollection<FTS_EP_DATA> Polygons { get; } = new ObservableCollection<FTS_EP_DATA>(); //not sure if this are actually polygons or just the vertices

        public void ReadFrom(StructReader reader)
        {
            Data = reader.ReadStruct<EERIE_SAVE_ROOM_DATA>();

            for (int i = 0; i < Data.nb_portals; i++)
            {
                Portals.Add(reader.ReadInt32());
            }

            for (int i = 0; i < Data.nb_polys; i++)
            {
                Polygons.Add(reader.ReadStruct<FTS_EP_DATA>());
            }

            Portals.CollectionChanged += Portals_CollectionChanged;
            Polygons.CollectionChanged += Polygons_CollectionChanged;
        }

        private void Portals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Data.nb_portals = Portals.Count;
        }

        private void Polygons_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Data.nb_polys = Polygons.Count;
        }

        public void WriteTo(StructWriter writer)
        {
            writer.WriteStruct(Data);

            for (int i = 0; i < Portals.Count; i++)
            {
                writer.Write(Portals[i]);
            }

            for (int i = 0; i < Polygons.Count; i++)
            {
                writer.WriteStruct(Polygons[i]);
            }
        }
    }
}
