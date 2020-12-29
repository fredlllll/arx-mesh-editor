using Assets.Scripts.ArxLevel;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.ArxNative;
using Assets.Scripts.ArxNative.IO;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;

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
            int loadedPolys = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int lightIndex = 0;
            var fts = lvl.ArxLevelNative.FTS;
            Dictionary<int, int> tcToIndex = new Dictionary<int, int>();
            //texture indices
            for (int i = 0; i < fts.textureContainers.Length; i++)
            {
                tcToIndex[fts.textureContainers[i].tc] = i;
            }

            //TODO: use external placeholder texture so it can be set to 0 on export
            var notFoundMaterialKey = new EditorMaterial(EditorSettings.DataDir + "graph\\interface\\misc\\default[icon].bmp", PolyType.GLOW, 0);

            for (int c = 0; c < fts.cells.Length; c++)
            {
                var cell = fts.cells[c];
                loadedPolys += cell.polygons.Length;
                for (int p = 0; p < cell.polygons.Length; p++)
                {
                    var poly = cell.polygons[p];

                    var matKey = notFoundMaterialKey;
                    if (tcToIndex.TryGetValue(poly.tex, out int textureIndex))
                    {
                        string texArxPath = ArxIOHelper.GetString(fts.textureContainers[textureIndex].fic);
                        string texPath = TextureDatabase.GetRealTexturePath(EditorSettings.DataDir + texArxPath);
                        matKey = new EditorMaterial(texPath, poly.type, poly.transval); //TODO: speed up by using a pool of some sort?
                    }

                    MaterialMesh mm = lvl.EditableLevelMesh.GetMaterialMesh(matKey);

                    EditablePrimitiveInfo prim = new EditablePrimitiveInfo
                    {
                        polyType = poly.type,
                        norm = poly.norm.ToVector3(),
                        norm2 = poly.norm2.ToVector3(),
                        area = poly.area,
                        room = poly.room,
                        paddy = poly.paddy
                    };
                    int vertCount = prim.VertexCount;
                    for (int i = 0; i < vertCount; i++)
                    {
                        var vert = poly.vertices[i];
                        prim.vertices[i] = new EditableVertexInfo(new Vector3(vert.posX, vert.posY, vert.posZ),
                            new Vector2(vert.texU, 1 - vert.texV),
                            poly.normals[i].ToVector3(),
                            ArxIOHelper.FromBGRA(lvl.ArxLevelNative.LLF.lightColors[lightIndex++]));
                    }

                    if (prim.IsTriangle)
                    {
                        //load 4th vertex manually as it has no lighting value and would break lighting otherwise
                        var lastVert = poly.vertices[3];
                        prim.vertices[3] = new EditableVertexInfo(new Vector3(lastVert.posX, lastVert.posY, lastVert.posZ),
                            new Vector2(lastVert.texU, 1 - lastVert.texV),
                            poly.normals[3].ToVector3(),
                            Color.white);
                    }

                    mm.AddPrimitive(prim);
                }
            }

            foreach (var kv in lvl.EditableLevelMesh.MaterialMeshes)
            {
                kv.Value.UpdateMesh();
            }
            UnityEngine.Debug.Log("total load time: " + sw.Elapsed);
            UnityEngine.Debug.Log("Loaded polys: " + loadedPolys);
        }
    }
}
