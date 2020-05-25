using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FTL_IO
{
    public class FTL_IO
    {
        public FTL_IO_PRIMARY_HEADER header;
        public FTL_IO_SECONDARY_HEADER secondaryHeader;

        public FTL_IO_3D_DATA_HEADER _3DDataHeader;
        public EERIE_OLD_VERTEX[] vertexList;
        public EERIE_FACE_FTL[] faceList;
        public FTL_IO_TEXTURE_CONTAINER[] textureContainers;

        public void ReadFrom(Stream s)
        {
            StructReader reader = new StructReader(s, Encoding.ASCII, true);

            header = reader.ReadStruct<FTL_IO_PRIMARY_HEADER>();

            secondaryHeader = reader.ReadStruct<FTL_IO_SECONDARY_HEADER>();

            if (secondaryHeader.offset_3Ddata != -1)
            {
                s.Position = secondaryHeader.offset_3Ddata;

                _3DDataHeader = reader.ReadStruct<FTL_IO_3D_DATA_HEADER>();
                vertexList = new EERIE_OLD_VERTEX[_3DDataHeader.nb_vertex];
                faceList = new EERIE_FACE_FTL[_3DDataHeader.nb_faces];
                textureContainers = new FTL_IO_TEXTURE_CONTAINER[_3DDataHeader.nb_maps];

                for (int i = 0; i < vertexList.Length; i++)
                {
                    vertexList[i] = reader.ReadStruct<EERIE_OLD_VERTEX>();
                }

                for (int i = 0; i < faceList.Length; i++)
                {
                    faceList[i] = reader.ReadStruct<EERIE_FACE_FTL>();
                }

                for (int i = 0; i < textureContainers.Length; i++)
                {
                    textureContainers[i] = reader.ReadStruct<FTL_IO_TEXTURE_CONTAINER>();
                }
            }
        }

        public void WriteTo(Stream s)
        {

        }

        public static Stream EnsureUnpacked(Stream s)
        {
            var start = s.Position;

            byte[] first3 = new byte[3];
            s.Read(first3, 0, first3.Length);
            s.Position = start;
            if (first3[0] == 'F' && first3[1] == 'T' && first3[2] == 'L')
            {
                //uncomressed
                return s;
            }

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
