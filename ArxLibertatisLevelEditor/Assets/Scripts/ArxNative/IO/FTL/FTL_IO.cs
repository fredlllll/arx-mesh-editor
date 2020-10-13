using Assets.Scripts.Util;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ArxNative.IO.FTL
{
    public class FTL_IO
    {
        public FTL_IO_PRIMARY_HEADER header;
        public FTL_IO_SECONDARY_HEADER secondaryHeader;

        public bool has3DDataSection = false;
        public FTL_IO_3D_DATA_SECTION _3DDataSection;
        //apparently all the other sections are unused and undocumented, saving other contents of file into arrays, this will break values in secondary header if modified (for now)
        byte[] dataTill3Ddata, dataTillFileEnd;

        public void ReadFrom(Stream s)
        {
            StructReader reader = new StructReader(s, Encoding.ASCII, true);

            header = reader.ReadStruct<FTL_IO_PRIMARY_HEADER>();

            secondaryHeader = reader.ReadStruct<FTL_IO_SECONDARY_HEADER>();

            long till3Ddata = secondaryHeader.offset_3Ddata - s.Position;
            if (till3Ddata > 0)
            {
                dataTill3Ddata = reader.ReadBytes((int)till3Ddata);
            }
            else
            {
                dataTill3Ddata = null;
            }


            if (secondaryHeader.offset_3Ddata >= 0)
            {
                s.Position = secondaryHeader.offset_3Ddata;

                _3DDataSection = new FTL_IO_3D_DATA_SECTION();
                _3DDataSection.ReadFrom(reader);
                has3DDataSection = true;
            }
            else
            {
                Debug.LogWarning("invalid 3d offset: " + secondaryHeader.offset_3Ddata);
            }

            long tillFileEnd = s.Length - s.Position;
            if (tillFileEnd > 0)
            {
                dataTillFileEnd = reader.ReadBytes((int)tillFileEnd);
            }
            else
            {
                dataTillFileEnd = null;
            }
        }

        public void WriteTo(Stream s)
        {
            StructWriter writer = new StructWriter(s, Encoding.ASCII, true);

            writer.WriteStruct(header);

            var secondaryHeaderPosition = s.Position;
            writer.WriteStruct(secondaryHeader); //write header with old values for now

            if (dataTill3Ddata != null)
            {
                writer.Write(dataTill3Ddata);
            }

            if (has3DDataSection)
            {
                secondaryHeader.offset_3Ddata = (int)s.Position;
                writer.WriteStruct(_3DDataSection);
            }
            else
            {
                secondaryHeader.offset_3Ddata = -1;
            }

            if (dataTillFileEnd != null)
            {
                writer.Write(dataTillFileEnd);
            }

            var end = s.Position;
            s.Position = secondaryHeaderPosition;
            writer.WriteStruct(secondaryHeader); //update offsets
            s.Position = end; //go back to end, in case user wants to do more with the stream
        }

        public Mesh CreateMesh()
        {
            Mesh m = new Mesh();

            if (has3DDataSection)
            {
                Vector3[] verts = new Vector3[_3DDataSection.vertexList.Length];
                Vector3[] norms = new Vector3[verts.Length];
                Vector2[] uvs = new Vector2[verts.Length];
                Color[] colors = new Color[verts.Length]; //blender plugin says always 0, skip for now cause i dunno if its argb, or rgba

                //TODO: basically this is using the faces list to create seperate faces. i have to copy the vertex for the faces because the normals & uv are redefined in every face

                List<int> indices = new List<int>();

                for (int i = 0; i < verts.Length; i++)
                {
                    var vert = _3DDataSection.vertexList[i];
                    verts[i] = vert.vert.ToVector3();
                    norms[i] = vert.norm.ToVector3();
                }

                foreach (var face in _3DDataSection.faceList)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        ushort vertIndex = face.vid[i];

                        uvs[vertIndex] = new Vector2(face.u[i], face.v[i]);
                        indices.Add(vertIndex);
                    }
                }

                m.vertices = verts;
                m.triangles = indices.ToArray();
                m.normals = norms;

                m.RecalculateBounds();
                m.RecalculateTangents();
                m.Optimize();
            }

            return m;
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
