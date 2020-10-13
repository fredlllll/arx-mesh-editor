using Assets.Scripts.Util;

namespace Assets.Scripts.ArxNative.IO.FTL
{
    public struct FTL_IO_3D_DATA_SECTION
    {
        public FTL_IO_3D_DATA_HEADER header;

        public EERIE_OLD_VERTEX[] vertexList;
        public EERIE_FACE_FTL[] faceList;
        public FTL_IO_TEXTURE_CONTAINER[] textureContainers;
        public FTL_IO_3D_DATA_GROUP[] groups;
        public EERIE_ACTIONLIST_FTL[] actionList;
        public FTL_IO_3D_DATA_SELECTION[] selections;

        public void ReadFrom(StructReader reader)
        {
            header = reader.ReadStruct<FTL_IO_3D_DATA_HEADER>();
            vertexList = new EERIE_OLD_VERTEX[header.nb_vertex];
            faceList = new EERIE_FACE_FTL[header.nb_faces];
            textureContainers = new FTL_IO_TEXTURE_CONTAINER[header.nb_maps];
            groups = new FTL_IO_3D_DATA_GROUP[header.nb_groups];
            actionList = new EERIE_ACTIONLIST_FTL[header.nb_action];
            selections = new FTL_IO_3D_DATA_SELECTION[header.nb_selections];

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

            //groups have to read in two steps
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i].group = reader.ReadStruct<EERIE_GROUP_FTL>();
            }

            for (int i = 0; i < groups.Length; i++)
            {
                var indices = groups[i].indices = new int[groups[i].group.nb_index];
                for (int j = 0; j < indices.Length; j++)
                {
                    indices[j] = reader.ReadInt32();
                }
            }

            for (int i = 0; i < actionList.Length; i++)
            {
                actionList[i] = reader.ReadStruct<EERIE_ACTIONLIST_FTL>();
            }

            //same as with groups
            for (int i = 0; i < selections.Length; i++)
            {
                selections[i].selection = reader.ReadStruct<EERIE_SELECTIONS_FTL>();
            }

            for (int i = 0; i < selections.Length; i++)
            {
                var selected = selections[i].selected = new int[selections[i].selection.nb_selected];
                for (int j = 0; j < selected.Length; j++)
                {
                    selected[j] = reader.ReadInt32();
                }
            }


        }

        public void WriteTo(StructWriter writer)
        {
            writer.WriteStruct(header);

            for (int i = 0; i < vertexList.Length; i++)
            {
                writer.WriteStruct(vertexList[i]);
            }

            for (int i = 0; i < faceList.Length; i++)
            {
                writer.WriteStruct(faceList[i]);
            }

            for (int i = 0; i < textureContainers.Length; i++)
            {
                writer.WriteStruct(textureContainers[i]);
            }
        }
    }
}
