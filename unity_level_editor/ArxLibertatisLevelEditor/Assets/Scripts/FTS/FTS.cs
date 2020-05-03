using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FTS
{
    public class FTS
    {
        public UNIQUE_HEADER uh;
        public List<UNIQUE_HEADER2> uh2s = new List<UNIQUE_HEADER2>();

        public FAST_SCENE_HEADER fsh;

        public List<FAST_TEXTURE_CONTAINER> ftcs = new List<FAST_TEXTURE_CONTAINER>();

        public List<FAST_SCENE_INFO> fsis = new List<FAST_SCENE_INFO>();
        public List<List<FAST_EERIEPOLY>> fepss = new List<List<FAST_EERIEPOLY>>();

        public List<FAST_ANCHOR_DATA> fads = new List<FAST_ANCHOR_DATA>();

        public List<EERIE_SAVE_PORTALS> esps = new List<EERIE_SAVE_PORTALS>();
        public List<EERIE_SAVE_ROOM_DATA> esrs = new List<EERIE_SAVE_ROOM_DATA>();

        public List<ROOM_DIST_DATA_SAVE> rdds = new List<ROOM_DIST_DATA_SAVE>();
        public void LoadFrom(Stream s)
        {
            StructReader reader = new StructReader(s);

            uh = reader.ReadStruct<UNIQUE_HEADER>();

            for (int i = 0; i < uh.count; i++)
            {
                var uh2 = reader.ReadStruct<UNIQUE_HEADER2>();
                uh2s.Add(uh2);
            }

            //unpack stuff
            reader = new StructReader(new MemoryStream(ArxIO.Unpack(reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position)))));

            fsh = reader.ReadStruct<FAST_SCENE_HEADER>();

            for (int i = 0; i < fsh.nb_textures; i++)
            {
                var ftc = reader.ReadStruct<FAST_TEXTURE_CONTAINER>();
                ftcs.Add(ftc);
            }

            for (int z = 0; z < fsh.sizez; z++)
            {
                for (int x = 0; x < fsh.sizex; x++)
                {
                    var fsi = reader.ReadStruct<FAST_SCENE_INFO>(); //TODO: make this a complex object to hold variable lists like polys and anchors
                    fsis.Add(fsi);

                    var feps = new List<FAST_EERIEPOLY>();
                    for (int i = 0; i < fsi.nbpoly; i++)
                    {
                        var fep = reader.ReadStruct<FAST_EERIEPOLY>();
                        feps.Add(fep);
                    }
                    fepss.Add(feps);


                    if (fsi.nbianchors > 0)
                    {
                        //its skipped in the code... these are probably the associated anchors indices. anchors are read after this, so i guess thats why its skipped here now
                        //Debug.Log("skipping " + fsi.nbianchors);
                        reader.BaseStream.Position += sizeof(int) * fsi.nbianchors;
                    }
                }
            }

            for (int i = 0; i < fsh.nb_anchors; i++)
            {
                var fad = reader.ReadStruct<FAST_ANCHOR_DATA>();
                fads.Add(fad);

                //skipping linked anchors atm, gotta put that into a data structure eventually
                reader.BaseStream.Position += sizeof(int) * fad.nb_linked;
            }

            for (int i = 0; i < fsh.nb_portals; i++)
            {
                var esp = reader.ReadStruct<EERIE_SAVE_PORTALS>();
                esps.Add(esp);
            }

            for (int i = 0; i < fsh.nb_rooms + 1; i++) //no idea why +1, but its in the code
            {
                var esr = reader.ReadStruct<EERIE_SAVE_ROOM_DATA>();
                esrs.Add(esr);

                //skipping room associated portals for now
                reader.BaseStream.Position += sizeof(int) * esr.nb_portals;

                //skipping polys too
                reader.BaseStream.Position += Marshal.SizeOf(typeof(FAST_EP_DATA)) * esr.nb_polys;
            }

            for (int i = 0; i < fsh.nb_rooms + 1; i++) //no idea why +1, but its in the code
            {
                for (int j = 0; j < fsh.nb_rooms + 1; j++)
                {
                    var rdd = reader.ReadStruct<ROOM_DIST_DATA_SAVE>();
                    rdds.Add(rdd);
                }
            }

            long remaining = reader.BaseStream.Length - reader.BaseStream.Position;
            if (remaining > 0)
            {
                Debug.Log("ignoring " + remaining + " bytes in fts");
            }
        }
        public void WriteTo(Stream s)
        {
            StructWriter writer = new StructWriter(s);

            //TODO
        }
    }
}
