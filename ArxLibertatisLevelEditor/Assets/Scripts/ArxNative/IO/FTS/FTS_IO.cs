using Assets.Scripts.Util;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    public class FTS_IO
    {
        public FTS_IO_UNIQUE_HEADER header;
        public FTS_IO_UNIQUE_HEADER2[] uniqueHeaders;

        public FTS_IO_SCENE_HEADER sceneHeader;

        public FTS_IO_TEXTURE_CONTAINER[] textureContainers;
        public FTS_IO_CELL[] cells;
        public FTS_IO_ANCHOR[] anchors;
        public EERIE_IO_PORTALS[] portals;
        public FTS_IO_ROOM[] rooms;
        public FTS_IO_ROOM_DIST_DATA[] roomDistances;

        public void LoadFrom(Stream s)
        {
            StructReader reader = new StructReader(s);

            header = reader.ReadStruct<FTS_IO_UNIQUE_HEADER>();

            uniqueHeaders = new FTS_IO_UNIQUE_HEADER2[header.count];
            for (int i = 0; i < header.count; i++)
            {
                uniqueHeaders[i] = reader.ReadStruct<FTS_IO_UNIQUE_HEADER2>();
            }

            sceneHeader = reader.ReadStruct<FTS_IO_SCENE_HEADER>();
            textureContainers = new FTS_IO_TEXTURE_CONTAINER[sceneHeader.nb_textures];
            for (int i = 0; i < sceneHeader.nb_textures; i++)
            {
                textureContainers[i] = reader.ReadStruct<FTS_IO_TEXTURE_CONTAINER>();
            }

            int cellCount = sceneHeader.sizez * sceneHeader.sizex;
            cells = new FTS_IO_CELL[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                var cell = new FTS_IO_CELL();
                cell.ReadFrom(reader);
                cells[i] = cell;
            }
            /*for (int z = 0, index = 0; z < sceneHeader.sizez; z++)
            {
                for (int x = 0; x < sceneHeader.sizex; x++, index++)
                {
                    var cell = new FTS_IO_CELL();
                    cell.ReadFrom(reader);
                    cells[index] = cell;
                }
            }*/

            anchors = new FTS_IO_ANCHOR[sceneHeader.nb_anchors];
            for (int i = 0; i < sceneHeader.nb_anchors; i++)
            {
                var anchor = new FTS_IO_ANCHOR();
                anchor.ReadFrom(reader);
                anchors[i] = anchor;
            }

            portals = new EERIE_IO_PORTALS[sceneHeader.nb_portals];
            for (int i = 0; i < sceneHeader.nb_portals; i++)
            {
                portals[i] = reader.ReadStruct<EERIE_IO_PORTALS>();
            }

            rooms = new FTS_IO_ROOM[sceneHeader.nb_rooms + 1];
            for (int i = 0; i < sceneHeader.nb_rooms + 1; i++) //no idea why +1, but its in the code
            {
                var room = new FTS_IO_ROOM();
                room.ReadFrom(reader);
                rooms[i] = room;
            }

            roomDistances = new FTS_IO_ROOM_DIST_DATA[rooms.Length * rooms.Length];
            for (int i = 0, index = 0; i < rooms.Length; i++)
            {
                for (int j = 0; j < rooms.Length; j++, index++)
                {
                    roomDistances[index] = reader.ReadStruct<FTS_IO_ROOM_DIST_DATA>();
                }
            }

            long remaining = reader.BaseStream.Length - reader.BaseStream.Position;
            if (remaining > 0)
            {
                Debug.Log("ignoring " + remaining + " bytes in fts");
            }
        }

        public int CalculatePolyCount()
        {
            int count = 0;

            for (int i = 0; i < cells.Length; i++)
            {
                count += cells[i].polygons.Length;
            }

            return count;
        }

        public void WriteTo(Stream s)
        {
            StructWriter writer = new StructWriter(s);

            writer.WriteStruct(header);

            for (int i = 0; i < uniqueHeaders.Length; i++)
            {
                writer.WriteStruct(uniqueHeaders[i]);
            }

            writer.WriteStruct(sceneHeader);

            for (int i = 0; i < textureContainers.Length; i++)
            {
                writer.WriteStruct(textureContainers[i]);
            }

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].WriteTo(writer);
            }

            for (int i = 0; i < anchors.Length; i++)
            {
                anchors[i].WriteTo(writer);
            }

            for (int i = 0; i < portals.Length; i++)
            {
                writer.WriteStruct(portals[i]);
            }

            for (int i = 0; i < rooms.Length; i++)
            {
                rooms[i].WriteTo(writer);
            }

            for (int i = 0; i < roomDistances.Length; i++)
            {
                writer.WriteStruct(roomDistances[i]);
            }
        }

        public int CalculateWrittenSize(bool onlyCompressedStuff)
        {
            int size = 0;

            if (!onlyCompressedStuff)
            {
                size += Marshal.SizeOf<FTS_IO_UNIQUE_HEADER>();

                size += Marshal.SizeOf<FTS_IO_UNIQUE_HEADER2>() * uniqueHeaders.Length;
            }

            size += Marshal.SizeOf<FTS_IO_SCENE_HEADER>();

            size += Marshal.SizeOf<FTS_IO_TEXTURE_CONTAINER>() * textureContainers.Length;

            for (int i = 0; i < cells.Length; i++)
            {
                var c = cells[i];
                size += c.CalculateWrittenSize();
            }

            for (int i = 0; i < anchors.Length; i++)
            {
                var a = anchors[i];
                size += a.CalculateWrittenSize();
            }

            size += Marshal.SizeOf<EERIE_IO_PORTALS>() * portals.Length;

            for (int i = 0; i < rooms.Length; i++)
            {
                var r = rooms[i];
                size += r.CalculateWrittenSize();
            }

            size += Marshal.SizeOf<FTS_IO_ROOM_DIST_DATA>() * roomDistances.Length;

            return size;
        }

        public static Stream EnsureUnpacked(Stream s)
        {
            s.Position = 0;
            var reader = new StructReader(s, System.Text.Encoding.ASCII, true);

            MemoryStream ms = new MemoryStream();
            StructWriter writer = new StructWriter(ms, System.Text.Encoding.ASCII, true);

            var header = reader.ReadStruct<FTS_IO_UNIQUE_HEADER>();
            writer.WriteStruct(header);

            for (int i = 0; i < header.count; i++)
            {
                writer.WriteStruct(reader.ReadStruct<FTS_IO_UNIQUE_HEADER2>());
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
            s.Position = 0;
            StructReader reader = new StructReader(s);
            MemoryStream ms = new MemoryStream();
            StructWriter writer = new StructWriter(ms, System.Text.Encoding.ASCII, true);

            var header = reader.ReadStruct<FTS_IO_UNIQUE_HEADER>();
            writer.WriteStruct(header);

            for (int i = 0; i < header.count; i++)
            {
                writer.WriteStruct(reader.ReadStruct<FTS_IO_UNIQUE_HEADER2>());
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
