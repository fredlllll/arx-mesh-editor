using ArxLibertatisEditorIO.MediumIO;
using ArxLibertatisEditorIO.RawIO;
using ArxLibertatisEditorIO.Util;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.Util;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.ArxLevelLoading
{
    public static class LevelLoader
    {
        public static Level LoadLevel(string name)
        {
            var ral = new RawArxLevel();
            var mal = new MediumArxLevel();
            mal.LoadFrom(ral.LoadLevel(name));

            Level lvl = new Level(name, mal);

            Vector3 camPos = mal.DLF.header.positionEdit.ToUnity() / 100;
            camPos.y *= -1;
            LevelEditor.EditorCamera.transform.position = camPos;
            LevelEditor.EditorCamera.transform.eulerAngles = mal.DLF.header.eulersEdit.ToUnity();
            lvl.LevelOffset = mal.DLF.header.offset.ToUnity();

            LoadMesh(lvl);
            //light debug:
            GameObject lights = new GameObject();
            foreach (var l in mal.LLF.lights)
            {
                GameObject go = new GameObject();
                go.name = "light " + l.extras;
                go.transform.position = l.pos.ToUnity();
                var light = go.AddComponent<Light>();
                light.color = l.color.ToUnity();
                light.intensity = l.intensity;
                light.range = l.fallEnd;
                go.transform.SetParent(lights.transform);
            }
            lights.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f);
            var pos = mal.FTS.sceneHeader.Mscenepos.ToUnity() / 100;
            pos.y = -pos.y;
            UnityEngine.Debug.Log(pos);
            lights.transform.position = pos;
            //inter debug:
            /*foreach (var inter in lvl.ArxLevelNative.DLF.inters)
            {
                var interName = ArxIOHelper.GetString(inter.name);
                interName = interName.Replace("C:\\ARX\\", "game\\").Replace(".teo", ".ftl");
                var filePath = EditorSettings.DataDir + interName;
                UnityEngine.Debug.Log(filePath);

                FTL_IO ftl = new FTL_IO();
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var s = FTL_IO.EnsureUnpacked(fs);
                    ftl.ReadFrom(s);
                }

                var m = ftl.CreateMesh();
                GameObject go = new GameObject();
                var mf = go.AddComponent<MeshFilter>();
                mf.sharedMesh = m;
                var mr = go.AddComponent<MeshRenderer>();
                go.transform.localScale = new Vector3(0.1f, -0.1f, 0.1f);
            }*/

            return lvl;
        }

        static void LoadMesh(Level lvl)
        {
            int loadedPolys = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int lightIndex = 0;
            var fts = lvl.MediumArxLevel.FTS;
            Dictionary<int, int> tcToIndex = new Dictionary<int, int>();
            //texture indices
            for (int i = 0; i < fts.textureContainers.Count; i++)
            {
                tcToIndex[fts.textureContainers[i].containerId] = i;
            }

            UnityEngine.Debug.Log("Texture containers loaded: " + fts.textureContainers.Count);

            //TODO: use external placeholder texture so it can be set to 0 on export
            var notFoundMaterialKey = new EditorMaterial(ArxLibertatisEditorIO.ArxPaths.DataDir + "graph\\interface\\misc\\default[icon].bmp", PolyType.GLOW, 0);

            for (int c = 0; c < fts.cells.Count; c++)
            {
                var cell = fts.cells[c];
                loadedPolys += cell.polygons.Count;
                for (int p = 0; p < cell.polygons.Count; p++)
                {
                    var poly = cell.polygons[p];

                    var matKey = notFoundMaterialKey;
                    if (tcToIndex.TryGetValue(poly.textureContainerId, out int textureIndex))
                    {
                        string texArxPath = fts.textureContainers[textureIndex].texturePath;
                        matKey = new EditorMaterial(texArxPath, poly.polyType, poly.transVal); //TODO: speed up by using a pool of some sort?
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Couldnt find texture container " + poly.textureContainerId);
                    }

                    MaterialMesh mm = lvl.EditableLevelMesh.GetMaterialMesh(matKey);

                    EditablePrimitiveInfo prim = new EditablePrimitiveInfo
                    {
                        polyType = poly.polyType,
                        norm = poly.norm.ToUnity(),
                        norm2 = poly.norm2.ToUnity(),
                        area = poly.area,
                        room = poly.room,
                    };

                    int vertCount = prim.VertexCount;
                    for (int i = 0; i < vertCount; i++)
                    {
                        var vert = poly.vertices[i];
                        //might want to add some code that detects if lightcolors isnt the right size, but only thing i can think of is try catch around this, which might be slow?
                        var uv = vert.uv.ToUnity();
                        uv.y = 1 - uv.y;
                        var lightCol = lvl.MediumArxLevel.LLF.lightColors[lightIndex++].ToUnity();
                        prim.vertices[i] = new EditableVertexInfo(vert.position.ToUnity(),
                            uv,
                            vert.normal.ToUnity(),
                            lightCol);
                    }

                    if (prim.IsTriangle)
                    {
                        //load 4th vertex manually as it has no lighting value and would break lighting otherwise
                        var lastVert = poly.vertices[3];
                        var uv  = lastVert.uv.ToUnity();
                        uv.y = 1 - uv.y;
                        prim.vertices[3] = new EditableVertexInfo(lastVert.position.ToUnity(),
                            uv,
                            lastVert.normal.ToUnity(),
                            UnityEngine.Color.white);
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
