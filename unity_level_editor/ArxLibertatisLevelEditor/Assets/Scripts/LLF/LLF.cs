using Assets.Scripts.DLF;
using Assets.Scripts.Util;
using System.Collections.ObjectModel;
using System.IO;

namespace Assets.Scripts.LLF
{
    public class LLF
    {
        public LLF_HEADER Header { get; private set; }
        public ObservableCollection<DANAE_LIGHT> Lights { get; } = new ObservableCollection<DANAE_LIGHT>();
        public DANAE_LIGHTINGHEADER LightingHeader { get; private set; }
        public ObservableCollection<uint> LightColors { get; } = new ObservableCollection<uint>();

        public void LoadFrom(Stream s)
        {
            var reader = new StructReader(s);

            Header = reader.ReadStruct<LLF_HEADER>();

            for (int i = 0; i < Header.numLights; i++)
            {
                var light = reader.ReadStruct<DANAE_LIGHT>();
                Lights.Add(light);
            }

            LightingHeader = reader.ReadStruct<DANAE_LIGHTINGHEADER>();

            for (int i = 0; i < LightingHeader.numLights; i++)
            {
                LightColors.Add(reader.ReadUInt32()); //TODO is apparently BGRA if its in compact mode.
            }

            Lights.CollectionChanged += Lights_CollectionChanged;
            LightColors.CollectionChanged += LightColors_CollectionChanged;
        }

        private void Lights_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Header.numLights = Lights.Count;
        }

        private void LightColors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LightingHeader.numLights = LightColors.Count;
        }


        public void WriteTo(Stream s)
        {
            var writer = new StructWriter(s);

            writer.WriteStruct(Header);

            for (int i = 0; i < Header.numLights; i++)
            {
                writer.WriteStruct(Lights[i]);
            }

            writer.WriteStruct(LightingHeader);

            for (int i = 0; i < LightingHeader.numLights; i++)
            {
                writer.Write(LightColors[i]);
            }
        }

        public static Stream EnsureUnpacked(Stream s)
        {
            byte[] packed = new byte[s.Length];
            s.Read(packed, 0, packed.Length);
            byte[] unpacked = ArxIO.Unpack(packed);

            MemoryStream ms = new MemoryStream();
            ms.Write(unpacked, 0, unpacked.Length);
            ms.Position = 0;
            s.Dispose(); //close old stream
            return ms;
        }

        public static Stream EnsurePacked(Stream s)
        {
            byte[] unpacked = new byte[s.Length];
            s.Read(unpacked, 0, unpacked.Length);
            byte[] packed = ArxIO.Pack(unpacked);

            MemoryStream ms = new MemoryStream();
            ms.Write(packed, 0, packed.Length);
            ms.Position = 0;
            s.Dispose(); //close old stream
            return ms;
        }
    }
}
