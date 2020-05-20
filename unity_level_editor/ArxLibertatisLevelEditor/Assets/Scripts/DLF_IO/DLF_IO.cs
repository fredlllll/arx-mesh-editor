using Assets.Scripts.Shared_IO;
using Assets.Scripts.Util;
using System.IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.DLF_IO
{
    public class DLF_IO
    {
        public DLF_IO_HEADER header;
        public DLF_IO_SCENE[] scenes;
        public DLF_IO_INTER[] inters;
        public DANAE_IO_LIGHTINGHEADER lightingHeader;
        public uint[] lightColors;
        public DANAE_IO_LIGHT[] lights;
        public DLF_IO_FOG[] fogs;
        public byte[] nodesData;
        public DLF_IO_PATH[] paths;

        public void LoadFrom(Stream unpackedStream)
        {
            using (StructReader reader = new StructReader(unpackedStream, System.Text.Encoding.ASCII, true))
            {

                header = reader.ReadStruct<DLF_IO_HEADER>();

                scenes = new DLF_IO_SCENE[header.numScenes];
                for (int i = 0; i < header.numScenes; i++)
                {
                    scenes[i] = reader.ReadStruct<DLF_IO_SCENE>();
                }

                inters = new DLF_IO_INTER[header.numInters];
                for (int i = 0; i < header.numInters; i++)
                {
                    inters[i] = reader.ReadStruct<DLF_IO_INTER>();
                }

                if (header.lighting != 0)
                {
                    lightingHeader = reader.ReadStruct<DANAE_IO_LIGHTINGHEADER>();

                    lightColors = new uint[lightingHeader.numLights];
                    for (int i = 0; i < lightingHeader.numLights; i++)
                    {
                        lightColors[i] = reader.ReadUInt32(); //TODO is apparently BGRA if its in compact mode.
                    }
                }

                lights = new DANAE_IO_LIGHT[header.numLights];
                for (int i = 0; i < header.numLights; i++)
                {
                    lights[i] = reader.ReadStruct<DANAE_IO_LIGHT>();
                }

                fogs = new DLF_IO_FOG[header.numFogs];
                for (int i = 0; i < header.numFogs; i++)
                {
                    fogs[i] = reader.ReadStruct<DLF_IO_FOG>();
                }

                // Skip nodes, dont know why
                //save in var so we can write it back later
                nodesData = reader.ReadBytes(header.numNodes * (204 + header.numNodelinks * 64));

                paths = new DLF_IO_PATH[header.numPaths];
                for (int i = 0; i < header.numPaths; i++)
                {
                    var path = new DLF_IO_PATH();
                    path.ReadFrom(reader);
                    paths[i] = path;
                }
            }
        }

        public void WriteTo(Stream s)
        {
            using (StructWriter writer = new StructWriter(s, System.Text.Encoding.ASCII, true))
            {
                writer.WriteStruct(header);

                for (int i = 0; i < scenes.Length; i++)
                {
                    writer.WriteStruct(scenes[i]);
                }

                for (int i = 0; i < inters.Length; i++)
                {
                    writer.WriteStruct(inters[i]);
                }

                if (header.lighting != 0)
                {
                    writer.WriteStruct(lightingHeader);

                    for (int i = 0; i < lightColors.Length; i++)
                    {
                        writer.Write(lightColors[i]);
                    }
                }

                for (int i = 0; i < lights.Length; i++)
                {
                    writer.WriteStruct(lights[i]);
                }

                for (int i = 0; i < fogs.Length; i++)
                {
                    writer.WriteStruct(fogs[i]);
                }

                //write back nodes data
                writer.Write(nodesData);

                for (int i = 0; i < paths.Length; i++)
                {
                    paths[i].WriteTo(writer);
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
                var header = reader.ReadStruct<DLF_IO_HEADER>(); //read full header

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
            byte[] header = reader.ReadBytes(Marshal.SizeOf(typeof(DLF_IO_HEADER)));
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
