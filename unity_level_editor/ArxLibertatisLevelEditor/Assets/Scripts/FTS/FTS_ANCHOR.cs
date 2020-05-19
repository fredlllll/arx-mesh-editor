using Assets.Scripts.Util;
using System.Collections.ObjectModel;

namespace Assets.Scripts.FTS
{
    public class FTS_ANCHOR
    {
        public FTS_ANCHOR_DATA Data { get; private set; }
        public ObservableCollection<int> LinkedAnchors { get; } = new ObservableCollection<int>();

        public void ReadFrom(StructReader reader)
        {
            Data = reader.ReadStruct<FTS_ANCHOR_DATA>();

            for (int i = 0; i < Data.nb_linked; i++)
            {
                LinkedAnchors.Add(reader.ReadInt32());
            }

            LinkedAnchors.CollectionChanged += LinkedAnchors_CollectionChanged;
        }

        private void LinkedAnchors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Data.nb_linked = (short)LinkedAnchors.Count;
        }

        public void WriteTo(StructWriter writer)
        {
            writer.WriteStruct(Data);

            for (int i = 0; i < LinkedAnchors.Count; i++)
            {
                writer.Write(LinkedAnchors[i]);
            }
        }
    }
}
