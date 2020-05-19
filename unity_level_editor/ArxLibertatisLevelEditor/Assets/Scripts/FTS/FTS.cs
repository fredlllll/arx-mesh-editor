using Assets.Scripts.Util;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.FTS
{
    public class FTS
    {
        public FTS_UNIQUE_HEADER Header { get; private set; }
        public ObservableCollection<FTS_UNIQUE_HEADER2> UniqueHeaders { get; } = new ObservableCollection<FTS_UNIQUE_HEADER2>();

        public FTS_SCENE_HEADER SceneHeader { get; private set; }

        public ObservableCollection<FTS_TEXTURE_CONTAINER> TextureContainers { get; } = new ObservableCollection<FTS_TEXTURE_CONTAINER>();
        public ObservableCollection<FTS_CELL> Cells { get; } = new ObservableCollection<FTS_CELL>();
        public ObservableCollection<FTS_ANCHOR> Anchors { get; } = new ObservableCollection<FTS_ANCHOR>();
        public ObservableCollection<EERIE_SAVE_PORTALS> Portals { get; } = new ObservableCollection<EERIE_SAVE_PORTALS>();
        public ObservableCollection<FTS_ROOM> Rooms { get; } = new ObservableCollection<FTS_ROOM>();
        public ObservableCollection<ROOM_DIST_DATA_SAVE> RoomDistances { get; } = new ObservableCollection<ROOM_DIST_DATA_SAVE>();

        public void LoadFrom(Stream s)
        {
            StructReader reader = new StructReader(s);

            Header = reader.ReadStruct<FTS_UNIQUE_HEADER>();

            for (int i = 0; i < Header.count; i++)
            {
                var uh2 = reader.ReadStruct<FTS_UNIQUE_HEADER2>();
                UniqueHeaders.Add(uh2);
            }

            SceneHeader = reader.ReadStruct<FTS_SCENE_HEADER>();

            for (int i = 0; i < SceneHeader.nb_textures; i++)
            {
                var ftc = reader.ReadStruct<FTS_TEXTURE_CONTAINER>();
                TextureContainers.Add(ftc);
            }

            for (int z = 0; z < SceneHeader.sizez; z++)
            {
                for (int x = 0; x < SceneHeader.sizex; x++)
                {
                    var cell = new FTS_CELL(x, z);
                    cell.ReadFrom(reader);
                    Cells.Add(cell);
                }
            }

            for (int i = 0; i < SceneHeader.nb_anchors; i++)
            {
                var anchor = new FTS_ANCHOR();
                anchor.ReadFrom(reader);
                Anchors.Add(anchor);
            }

            for (int i = 0; i < SceneHeader.nb_portals; i++)
            {
                var esp = reader.ReadStruct<EERIE_SAVE_PORTALS>();
                Portals.Add(esp);
            }

            for (int i = 0; i < SceneHeader.nb_rooms + 1; i++) //no idea why +1, but its in the code
            {
                var room = new FTS_ROOM();
                room.ReadFrom(reader);
                Rooms.Add(room);
            }

            for (int i = 0; i < Rooms.Count; i++)
            {
                for (int j = 0; j < Rooms.Count; j++)
                {
                    var rdd = reader.ReadStruct<ROOM_DIST_DATA_SAVE>();
                    RoomDistances.Add(rdd);
                }
            }

            long remaining = reader.BaseStream.Length - reader.BaseStream.Position;
            if (remaining > 0)
            {
                Debug.Log("ignoring " + remaining + " bytes in fts");
            }

            UniqueHeaders.CollectionChanged += UniqueHeaders_CollectionChanged;
            TextureContainers.CollectionChanged += TextureContainers_CollectionChanged;
            //cant do cells cause its based on x and y, unless i use a dict instead
            Anchors.CollectionChanged += Anchors_CollectionChanged;
            Portals.CollectionChanged += Portals_CollectionChanged;
            Rooms.CollectionChanged += Rooms_CollectionChanged;
            //cant do room distances cause its based on room count square
        }

        private void UniqueHeaders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Header.count = UniqueHeaders.Count;
        }

        private void TextureContainers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SceneHeader.nb_textures = TextureContainers.Count;
        }

        private void Anchors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SceneHeader.nb_anchors = Anchors.Count;
        }

        private void Portals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SceneHeader.nb_portals = Portals.Count;
        }

        private void Rooms_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SceneHeader.nb_rooms = Rooms.Count - 1;// cause 1 is added everywhere else
        }

        public void WriteTo(Stream s)
        {
            StructWriter writer = new StructWriter(s);

            writer.WriteStruct(Header);

            for (int i = 0; i < UniqueHeaders.Count; i++)
            {
                writer.WriteStruct(UniqueHeaders[i]);
            }

            writer.WriteStruct(SceneHeader);

            for (int i = 0; i < TextureContainers.Count; i++)
            {
                writer.WriteStruct(TextureContainers[i]);
            }

            for (int z = 0; z < SceneHeader.sizez; z++)
            {
                for (int x = 0; x < SceneHeader.sizex; x++)
                {
                    int index = z * SceneHeader.sizex + x;
                    Cells[index].WriteTo(writer);
                }
            }

            for (int i = 0; i < Anchors.Count; i++)
            {
                Anchors[i].WriteTo(writer);
            }

            for (int i = 0; i < Portals.Count; i++)
            {
                writer.WriteStruct(Portals[i]);
            }

            for (int i = 0; i < Rooms.Count; i++)
            {
                Rooms[i].WriteTo(writer);
            }

            for (int i = 0; i < Rooms.Count; i++)
            {
                for (int j = 0; j < Rooms.Count; j++)
                {
                    int index = i * Rooms.Count + j;
                    writer.WriteStruct(RoomDistances[index]);
                }
            }
        }

        public static Stream EnsureUnpacked(Stream s)
        {
            var reader = new StructReader(s, System.Text.Encoding.ASCII, true);

            MemoryStream ms = new MemoryStream();
            StructWriter writer = new StructWriter(ms, System.Text.Encoding.ASCII, true);

            var header = reader.ReadStruct<FTS_UNIQUE_HEADER>();
            writer.WriteStruct(header);

            for (int i = 0; i < header.count; i++)
            {
                writer.WriteStruct(reader.ReadStruct<FTS_UNIQUE_HEADER2>());
            }

            byte[] restOfFile = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
            byte[] unpacked = ArxIO.Unpack(restOfFile);

            writer.Write(unpacked); //write unpacked rest
            s.Dispose(); //close old stream
            ms.Position = 0;
            return ms;
        }

        public static Stream EnsurePacked(Stream s)
        {
            StructReader reader = new StructReader(s);
            MemoryStream ms = new MemoryStream();
            StructWriter writer = new StructWriter(ms, System.Text.Encoding.ASCII, true);

            var header = reader.ReadStruct<FTS_UNIQUE_HEADER>();
            writer.WriteStruct(header);

            for (int i = 0; i < header.count; i++)
            {
                writer.WriteStruct(reader.ReadStruct<FTS_UNIQUE_HEADER2>());
            }

            byte[] restOfFile = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
            byte[] packed = ArxIO.Pack(restOfFile);

            ms.Write(packed, 0, packed.Length);
            ms.Position = 0;

            s.Dispose();
            return ms;
        }
    }
}
