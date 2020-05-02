using Assets.Scripts.Util;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts.DLF
{
    public class DLF
    {
        public DANAE_LS_HEADER dlh;
        public DANAE_LS_SCENE dls;
        public List<DANAE_LS_INTER> dlis = new List<DANAE_LS_INTER>();
        public List<DANAE_LS_FOG> dlfs = new List<DANAE_LS_FOG>();
        public List<DANAE_LS_PATH> dlps = new List<DANAE_LS_PATH>();

        public DANAE_LLF_HEADER llh;

        public void LoadFrom(Stream s)
        {
            StructReader reader = new StructReader(s);

            dlh = reader.ReadStruct<DANAE_LS_HEADER>();

            Debug.Log("Version " + dlh.version);

            if (dlh.version >= 1.44f)
            {
                byte[] restOfFile = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
                byte[] unpacked = ArxIO.Unpack(restOfFile);
                reader = new StructReader(new MemoryStream(unpacked));
            }

            Debug.Log("scenes " + dlh.nb_scn);

            if (dlh.nb_scn > 0)
            {
                dls = reader.ReadStruct<DANAE_LS_SCENE>();
            }

            Debug.Log("inters " + dlh.nb_inter);

            for (int i = 0; i < dlh.nb_inter; i++)
            {
                var dli = reader.ReadStruct<DANAE_LS_INTER>();
                dlis.Add(dli);
            }

            if (dlh.lighting != 0)
            {
                //load lighting
                //loadLighting(dat, pos, dlh.version > 1.001f, lightingFile != NULL);
            }

            int nb_lights = (dlh.version < 1.003f) ? 0 : dlh.nb_lights;

            Debug.Log("lights " + nb_lights);

            var lightingFileExists = false;

            if (!lightingFileExists)
            {
                //TODO: this
                //loadLights(dat, pos, nb_lights);
            }
            else
            {
                //skip
                reader.BaseStream.Position += nb_lights * Marshal.SizeOf(typeof(DANAE_LS_LIGHT));
            }

            Debug.Log("fogs " + dlh.nb_fogs);

            for (int i = 0; i < dlh.nb_fogs; i++)
            {
                var dlf = reader.ReadStruct<DANAE_LS_FOG>();
                dlfs.Add(dlf);
            }

            // Skip nodes
            reader.BaseStream.Position += (dlh.version < 1.001f) ? 0 : dlh.nb_nodes * (204 + dlh.nb_nodeslinks * 64);

            Debug.Log("paths " + dlh.nb_paths);

            for (int i = 0; i < dlh.nb_paths; i++)
            {
                var dlp = reader.ReadStruct<DANAE_LS_PATH>();
                dlps.Add(dlp);

                if (dlp.height != 0)
                {
                    //zone
                    for (int j = 0; j < dlp.nb_pathways; j++)
                    {
                        var dlpw = reader.ReadStruct<DANAE_LS_PATHWAYS>();
                        //TODO: keep track of these somewhere and associate with the path
                    }
                }
                else
                {
                    //path
                    for (int j = 0; j < dlp.nb_pathways; j++)
                    {
                        var dlpw = reader.ReadStruct<DANAE_LS_PATHWAYS>();
                        //TODO: same as above
                    }
                }
            }

            //now load llf
            if (lightingFileExists)
            {
                reader = null; //TODO: set reader to proper file

                // using compression
                if (dlh.version >= 1.44f)
                {
                    //decompress and reassign reader
                    byte[] restOfFile = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
                    byte[] unpacked = ArxIO.Unpack(restOfFile);
                    reader = new StructReader(new MemoryStream(unpacked));
                }

                llh = reader.ReadStruct<DANAE_LLF_HEADER>();

                //TODO: now load lighting from this instead from dlf
            }
        }

        public void WriteTo(Stream s)
        {
            StructWriter writer = new StructWriter(s);

            writer.WriteStruct(dlh);
        }
    }
}
