using Assets.Scripts.Util;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF
{
    public class DLF
    {
        public DLF_HEADER Header { get; private set; }
        public ObservableCollection<DLF_SCENE> Scenes { get; } = new ObservableCollection<DLF_SCENE>();
        public ObservableCollection<DLF_INTER> Inters { get; } = new ObservableCollection<DLF_INTER>();
        public DANAE_LIGHTINGHEADER LightingHeader { get; private set; } //TODO make a way to switch between embedded lighting and external?
        public ObservableCollection<uint> LightColors { get; } = new ObservableCollection<uint>();
        public ObservableCollection<DANAE_LIGHT> Lights { get; } = new ObservableCollection<DANAE_LIGHT>();
        public ObservableCollection<DLF_FOG> Fogs { get; } = new ObservableCollection<DLF_FOG>();
        private byte[] nodesData;
        public ObservableCollection<DLF_PATH> Paths { get; } = new ObservableCollection<DLF_PATH>();

        public void LoadFrom(Stream unpackedStream)
        {
            using (StructReader reader = new StructReader(unpackedStream, System.Text.Encoding.ASCII, true))
            {

                Header = reader.ReadStruct<DLF_HEADER>();

                for (int i = 0; i < Header.numScenes; i++)
                {
                    var scene = reader.ReadStruct<DLF_SCENE>();
                    Scenes.Add(scene);
                }

                for (int i = 0; i < Header.numInters; i++)
                {
                    var inter = reader.ReadStruct<DLF_INTER>();
                    Inters.Add(inter);
                }

                if (Header.lighting != 0)
                {
                    LightingHeader = reader.ReadStruct<DANAE_LIGHTINGHEADER>();

                    for (int i = 0; i < LightingHeader.numLights; i++)
                    {
                        LightColors.Add(reader.ReadUInt32()); //TODO is apparently BGRA if its in compact mode.
                    }
                }

                for (int i = 0; i < Header.numLights; i++)
                {
                    var light = reader.ReadStruct<DANAE_LIGHT>();
                    Lights.Add(light);
                }

                for (int i = 0; i < Header.numFogs; i++)
                {
                    var dlf = reader.ReadStruct<DLF_FOG>();
                    Fogs.Add(dlf);
                }

                // Skip nodes, dont know why
                //save in var so we can write it back later
                nodesData = reader.ReadBytes(Header.numNodes * (204 + Header.numNodelinks * 64));

                for (int i = 0; i < Header.numPaths; i++)
                {
                    var path = new DLF_PATH();
                    path.ReadFrom(reader);
                    Paths.Add(path);
                }
            }

            //set event listeners
            Scenes.CollectionChanged += Scenes_CollectionChanged;
            Inters.CollectionChanged += Inters_CollectionChanged;
            LightColors.CollectionChanged += LightColors_CollectionChanged;
            Lights.CollectionChanged += Lights_CollectionChanged;
            Fogs.CollectionChanged += Fogs_CollectionChanged;
            Paths.CollectionChanged += Paths_CollectionChanged;
        }

        private void Scenes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Header.numScenes = Scenes.Count;
        }

        private void Inters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Header.numInters = Inters.Count;
        }

        private void LightColors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LightingHeader.numLights = LightColors.Count;
        }

        private void Lights_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Header.numLights = Lights.Count;
        }

        private void Fogs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Header.numFogs = Fogs.Count;
        }

        private void Paths_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Header.numPaths = Paths.Count;
        }

        public void WriteTo(Stream s)
        {
            using (StructWriter writer = new StructWriter(s, System.Text.Encoding.ASCII, true))
            {

                writer.WriteStruct(Header);

                for (int i = 0; i < Scenes.Count; i++)
                {
                    writer.WriteStruct(Scenes[i]);
                }

                for (int i = 0; i < Inters.Count; i++)
                {
                    writer.WriteStruct(Inters[i]);
                }

                if (Header.lighting != 0)
                {
                    writer.WriteStruct(LightingHeader);

                    for (int i = 0; i < LightingHeader.numLights; i++)
                    {
                        writer.Write(LightColors[i]);
                    }
                }

                for (int i = 0; i < Header.numLights; i++)
                {
                    writer.WriteStruct(Lights[i]);
                }

                for (int i = 0; i < Header.numFogs; i++)
                {
                    writer.WriteStruct(Fogs[i]);
                }

                //write back nodes data
                writer.Write(nodesData);

                for (int i = 0; i < Header.numPaths; i++)
                {
                    Paths[i].WriteTo(writer);
                }
            }
        }

        public static Stream EnsureUnpacked(Stream s)
        {
            var reader = new StructReader(s, System.Text.Encoding.ASCII, true);
            var streamStart = s.Position;

            var version = reader.ReadSingle(); //read just version
            s.Position = streamStart; //back to start for further processing
            if (version >= 1.44f)
            {
                var header = reader.ReadStruct<DLF_HEADER>(); //read full header

                MemoryStream ms = new MemoryStream();

                StructWriter writer = new StructWriter(ms, System.Text.Encoding.ASCII, true);
                writer.WriteStruct(header); //write header

                byte[] restOfFile = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
                byte[] unpacked = ArxIO.Unpack(restOfFile);

                writer.Write(unpacked); //write unpacked rest
                s.Dispose(); //close old stream
                ms.Position = 0;
                return ms;
            }
            return s; //no need to unpack, return input stream
        }

        public static Stream EnsurePacked(Stream s)
        {
            //TODO: i should pack stuff depending on version, but for now ill just assume version 1.44 by default

            MemoryStream ms = new MemoryStream();

            BinaryReader reader = new BinaryReader(s);
            byte[] header = reader.ReadBytes(Marshal.SizeOf(typeof(DLF_HEADER)));
            byte[] restOfFile = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));

            byte[] packed = ArxIO.Pack(restOfFile);

            ms.Write(header, 0, header.Length);
            ms.Write(packed, 0, packed.Length);
            ms.Position = 0;

            s.Dispose();
            return ms;
        }
    }
}
