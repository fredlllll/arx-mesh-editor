using Assets.Scripts.Util;
using System.Collections.ObjectModel;

namespace Assets.Scripts.DLF
{
    public class DLF_PATH
    {
        public DLF_PATH_HEADER Header { get; private set; }
        public ObservableCollection<DLF_PATHWAYS> Paths { get; } = new ObservableCollection<DLF_PATHWAYS>();

        public void ReadFrom(StructReader reader)
        {
            Header = reader.ReadStruct<DLF_PATH_HEADER>();
            for (int i = 0; i < Header.numPathways; i++)
            {
                var path = reader.ReadStruct<DLF_PATHWAYS>();
                Paths.Add(path);
            }

            Paths.CollectionChanged += Paths_CollectionChanged;
        }

        private void Paths_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Header.numPathways = Paths.Count; //sync count
        }

        public void WriteTo(StructWriter writer)
        {
            writer.WriteStruct(Header);
            for (int i = 0; i < Header.numPathways; i++)
            {
                writer.WriteStruct(Paths[i]);
            }
        }
    }
}
