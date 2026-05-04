using ArxLibertatisEditorIO.MediumIO;
using ArxLibertatisEditorIO.RawIO;
using ArxLibertatisEditorIO.RawIO.FTL;
using ArxLibertatisEditorIO.Util;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            LoadLights(lvl);
            LoadInters(lvl);
            LoadPortals(lvl);
            LoadNavGrid(lvl);

            return lvl;
        }

        private static void LoadLights(Level lvl)
        {
            GameObject lights = lvl.LevelLightsObject;

            var mal = lvl.MediumArxLevel;
            foreach (var l in mal.LLF.lights)
            {
                GameObject go = new GameObject();
                go.name = "light " + l.extras;
                go.transform.position = l.pos.ToUnity();
                var light = go.AddComponent<Light>();
                light.color = l.color.ToUnity();
                light.intensity = l.intensity;
                light.range = l.fallEnd / 100;
                go.transform.SetParent(lights.transform);
            }
            lights.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f);
            var pos = mal.FTS.sceneHeader.Mscenepos.ToUnity() / 100;
            pos.y = -pos.y;
            lights.transform.position = pos;
        }

        private static void LoadInters(Level lvl)
        {
            var mat = MaterialsDatabase.PlaceholderMaterial;
            foreach (var inter in lvl.MediumArxLevel.DLF.inters)
            {
                UnityEngine.Debug.Log(inter.name);
                var intername = inter.name.ToLowerInvariant();
                var internameShort = intername.Replace("\\\\arkaneserver\\public\\arx\\graph\\obj3d\\interactive", "");
                var ftlName = intername.Replace("\\\\arkaneserver\\public\\arx\\", "\\game\\").Replace(".teo", ".ftl");
                UnityEngine.Debug.Log(ftlName);
                var filePath = EditorSettings.DataDir + ftlName;
                if (!File.Exists(filePath))
                {
                    UnityEngine.Debug.LogWarning("Couldnt find inter FTL file at " + filePath);
                    continue;
                }

                FTL_IO ftl = new FTL_IO();
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var s = FTL_IO.EnsureUnpacked(fs);
                    ftl.ReadFrom(s);
                }

                var m = ftl.CreateMesh();
                GameObject go = new GameObject(internameShort);
                var mf = go.AddComponent<MeshFilter>();
                mf.sharedMesh = m;
                var mr = go.AddComponent<MeshRenderer>();
                mr.material = MaterialsDatabase.PlaceholderMaterial;
                go.transform.localPosition = inter.position.ToUnity();

                var eul = inter.euler.ToUnity();
                eul = new Vector3(
                    eul.x,
                    -eul.y - 90, //might be wrong, but fixes rotation around y axis for most inters. good enough for preview purposes
                    eul.z
                    );

                go.transform.localEulerAngles = eul;
                go.transform.SetParent(lvl.LevelIntersObject.transform);
            }
            var pos = lvl.MediumArxLevel.FTS.sceneHeader.Mscenepos.ToUnity() / 100;
            pos.y = -pos.y;
            lvl.LevelIntersObject.transform.localPosition = pos;
            lvl.LevelIntersObject.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f);
        }

        private static void LoadPortals(Level lvl)
        {
            var mat = MaterialsDatabase.PlaceholderMaterial;

            foreach (var portal in lvl.MediumArxLevel.FTS.portals)
            {
                var go = new GameObject("portal " + portal.room_1 + "-" + portal.room_2);
                go.transform.SetParent(lvl.LevelPortalsObject.transform);
                var pos = portal.poly.vertices[0].pos.ToUnity();
                go.transform.localPosition = pos;
                var line = go.AddComponent<LineRenderer>();
                line.useWorldSpace = false;
                line.positionCount = 5;
                line.widthMultiplier = 0.1f;
                line.SetPosition(0, portal.poly.vertices[0].pos.ToUnity() - pos);
                line.SetPosition(1, portal.poly.vertices[1].pos.ToUnity() - pos);
                line.SetPosition(2, portal.poly.vertices[3].pos.ToUnity() - pos);
                line.SetPosition(3, portal.poly.vertices[2].pos.ToUnity() - pos);
                line.SetPosition(4, portal.poly.vertices[0].pos.ToUnity() - pos);
                line.material = mat;
            }

            lvl.LevelPortalsObject.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f);
        }

        private static void LoadNavGrid(Level lvl)
        {
            int i = 0;
            foreach (var a in lvl.MediumArxLevel.FTS.anchors)
            {
                var go = new GameObject("anchor" + i++);
                go.transform.SetParent(lvl.LevelNavGridObject.transform);
                go.transform.localPosition = a.pos.ToUnity();

                var mf = go.AddComponent<MeshFilter>();
                mf.sharedMesh = PrimitiveMeshHelper.GetSphereMesh();
                var mr = go.AddComponent<MeshRenderer>();
                mr.material = MaterialsDatabase.AnchorMaterial;
                go.transform.localScale = Vector3.one * 30;
            }
            lvl.LevelNavGridObject.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f);
            lvl.LevelNavGridObject.SetActive(false);
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
            var notFoundMaterialKey = new EditorMaterial("graph\\interface\\misc\\default[icon].bmp", PolyType.GLOW, 0);

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
                        var uv = vert.uv.ToUnity();
                        uv.y = 1 - uv.y;
                        UnityEngine.Color lightCol = UnityEngine.Color.magenta;
                        if (lightIndex >= lvl.MediumArxLevel.LLF.lightColors.Count)
                        {
                            UnityEngine.Debug.LogWarning($"Light color index {lightIndex} out of range! Filling with magenta color.");
                        }
                        else
                        {
                            lightCol = lvl.MediumArxLevel.LLF.lightColors[lightIndex++].ToUnity();
                        }
                        prim.vertices[i] = new EditableVertexInfo(vert.position.ToUnity(),
                            uv,
                            vert.normal.ToUnity(),
                            lightCol);
                    }

                    if (prim.IsTriangle)
                    {
                        //load 4th vertex manually as it has no lighting value and would break lighting otherwise
                        var lastVert = poly.vertices[3];
                        var uv = lastVert.uv.ToUnity();
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
