using Assets.Scripts.Util;
using System.Collections.ObjectModel;

namespace Assets.Scripts.FTS
{
    public class FTS_CELL
    {
        int x, z;
        public FTS_CELL(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public FTS_SCENE_INFO SceneInfo { get; private set; }
        public ObservableCollection<FTS_EERIEPOLY> Polygons { get; } = new ObservableCollection<FTS_EERIEPOLY>();
        public ObservableCollection<int> Anchors { get; } = new ObservableCollection<int>();

        public void ReadFrom(StructReader reader)
        {
            SceneInfo = reader.ReadStruct<FTS_SCENE_INFO>();

            for (int i = 0; i < SceneInfo.nbpoly; i++)
            {
                var poly = reader.ReadStruct<FTS_EERIEPOLY>();
                Polygons.Add(poly);
            }

            for (int i = 0; i < SceneInfo.nbianchors; i++)
            {
                Anchors.Add(reader.ReadInt32());
            }

            Polygons.CollectionChanged += Polygons_CollectionChanged;
            Anchors.CollectionChanged += Anchors_CollectionChanged;
        }

        private void Polygons_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SceneInfo.nbpoly = Polygons.Count;
        }

        private void Anchors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SceneInfo.nbianchors = Anchors.Count;
        }

        public void WriteTo(StructWriter writer)
        {
            writer.WriteStruct(SceneInfo);

            for (int i = 0; i < Polygons.Count; i++)
            {
                writer.WriteStruct(Polygons[i]);
            }

            for (int i = 0; i < Anchors.Count; i++)
            {
                writer.Write(Anchors[i]);
            }
        }
    }
}
