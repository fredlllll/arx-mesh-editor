using Assets.Scripts.ArxNative.IO.Shared_IO;
using Assets.Scripts.Util;
using System.Collections.ObjectModel;
using System.IO;

namespace Assets.Scripts.ArxNative.IO.LLF
{
    public class LLF_IO
    {
        public LLF_IO_HEADER header;
        public DANAE_IO_LIGHT[] lights;
        public DANAE_IO_LIGHTINGHEADER lightingHeader;
        public uint[] lightColors;

        public void LoadFrom(Stream s)
        {
            var reader = new StructReader(s);

            header = reader.ReadStruct<LLF_IO_HEADER>();

            lights = new DANAE_IO_LIGHT[header.numLights];
            for (int i = 0; i < header.numLights; i++)
            {
                lights[i] = reader.ReadStruct<DANAE_IO_LIGHT>();
            }

            lightingHeader = reader.ReadStruct<DANAE_IO_LIGHTINGHEADER>();

            lightColors = new uint[lightingHeader.numLights];
            for (int i = 0; i < lightingHeader.numLights; i++)
            {
                lightColors[i] = reader.ReadUInt32(); //TODO is apparently BGRA if its in compact mode.
            }
        }

        public void WriteTo(Stream s)
        {
            var writer = new StructWriter(s);

            writer.WriteStruct(header);

            for (int i = 0; i < lights.Length; i++)
            {
                writer.WriteStruct(lights[i]);
            }

            writer.WriteStruct(lightingHeader);

            for (int i = 0; i < lightColors.Length; i++)
            {
                writer.Write(lightColors[i]);
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
