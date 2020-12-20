using Assets.Scripts.ArxLevel;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Material;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.ArxNative;
using Assets.Scripts.ArxNative.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ArxLevelLoading
{
    public static class LevelLoader
    {
        public static Level LoadLevel(string name)
        {
            var lvln = new ArxLevelNative();
            lvln.LoadLevel(name);

            Level lvl = new Level(name, lvln);

            Vector3 camPos = lvln.DLF.header.positionEdit.ToVector3() / 100;
            camPos.y *= -1;
            LevelEditor.EditorCamera.transform.position = camPos;
            LevelEditor.EditorCamera.transform.eulerAngles = lvln.DLF.header.angleEdit.ToEuler();
            lvl.LevelOffset = lvln.DLF.header.offset.ToVector3();

            LoadMesh(lvl);

            return lvl;
        }

        static void LoadMesh(Level lvl)
        {
            int lightIndex = 0;
            var fts = lvl.ArxLevelNative.FTS;
            Dictionary<int, int> tcToIndex = new Dictionary<int, int>();
            //texture indices
            for (int i = 0; i < fts.textureContainers.Length; i++)
            {
                tcToIndex[fts.textureContainers[i].tc] = i;
            }

            //TODO: use external placeholder texture so it can be set to 0 on export
            var notFoundMaterialKey = new EditorMaterialKey(EditorSettings.DataDir + "graph\\interface\\misc\\default[icon].bmp", PolyType.GLOW, 0);

            for (int c = 0; c < fts.cells.Length; c++)
            {
                var cell = fts.cells[c];
                for (int p = 0; p < cell.polygons.Length; p++)
                {
                    var poly = cell.polygons[p];

                    var matKey = notFoundMaterialKey;
                    if (tcToIndex.TryGetValue(poly.tex, out int textureIndex))
                    {
                        string texArxPath = ArxIOHelper.GetString(fts.textureContainers[textureIndex].fic);
                        string texPath = TextureDatabase.GetRealTexturePath(EditorSettings.DataDir + texArxPath);
                        matKey = new EditorMaterialKey(texPath, poly.type, poly.transval);
                    }

                    MaterialMesh mm = lvl.EditableLevelMesh.GetMaterialMesh(matKey);

                    EditablePrimitive prim;
                    if (poly.type.HasFlag(PolyType.QUAD))
                    { //QUAD
                        prim = new EditableQuad();

                        for (int i = 0; i < 4; i++)
                        {
                            var vert = poly.vertices[i];
                            var evert = new EditableVertex(new Vector3(vert.posX, vert.posY, vert.posZ),
                                new Vector2(vert.texU, 1 - vert.texV),
                                poly.normals[i].ToVector3(),
                                ArxIOHelper.FromBGRA(lvl.ArxLevelNative.LLF.lightColors[lightIndex++]));
                            prim.vertices[i] = evert;
                        }
                    }
                    else
                    { //TRIANGLE
                        prim = new EditableTriangle();

                        for (int i = 0; i < 3; i++)
                        {
                            var vert = poly.vertices[i];
                            var evert = new EditableVertex(new Vector3(vert.posX, vert.posY, vert.posZ),
                                new Vector2(vert.texU, 1 - vert.texV),
                                poly.normals[i].ToVector3(),
                                ArxIOHelper.FromBGRA(lvl.ArxLevelNative.LLF.lightColors[lightIndex++]));
                            prim.vertices[i] = evert;
                        }
                    }

                    prim.norm = poly.norm.ToVector3();
                    prim.norm2 = poly.norm2.ToVector3();
                    prim.area = poly.area;
                    prim.room = poly.room;
                    prim.paddy = poly.paddy;

                    mm.AddPrimitive(prim);
                }
            }

            foreach (var kv in lvl.EditableLevelMesh.MaterialMeshes)
            {
                kv.Value.UpdateMesh();
            }
        }
    }
}
