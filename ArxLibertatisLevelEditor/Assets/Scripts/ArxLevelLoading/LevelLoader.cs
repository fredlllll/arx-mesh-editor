using Assets.Scripts.ArxLevel;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.ArxNative;
using Assets.Scripts.ArxNative.IO;
using Assets.Scripts.ArxNative.IO.FTL;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            //light debug:
            GameObject lights = new GameObject();
            foreach(var l in lvln.LLF.lights)
            {
                GameObject go = new GameObject();
                go.name = "light " + l.extras;
                go.transform.position = l.pos.ToVector3();
                var light = go.AddComponent<Light>();
                light.color = l.rgb.ToColor();
                light.intensity = l.intensity;
                light.range = l.fallEnd;
                go.transform.SetParent(lights.transform);
            }
            lights.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f);
            var pos = lvln.FTS.sceneHeader.Mscenepos.ToVector3() / 100;
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
            var fts = lvl.ArxLevelNative.FTS;
            Dictionary<int, int> tcToIndex = new Dictionary<int, int>();
            //texture indices
            for (int i = 0; i < fts.textureContainers.Length; i++)
            {
                tcToIndex[fts.textureContainers[i].tc] = i;
            }

            UnityEngine.Debug.Log("Texture containers loaded: " + fts.textureContainers.Length);

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
                        matKey = new EditorMaterial(texArxPath, poly.type, poly.transval); //TODO: speed up by using a pool of some sort?
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Couldnt find texture " + poly.tex);
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
                        //might want to add some code that detects if lightcolors isnt the right size, but only thing i can think of is try catch around this, which might be slow?
                        uint lightCol = lvl.ArxLevelNative.LLF.lightColors[lightIndex++];
                        prim.vertices[i] = new EditableVertexInfo(new Vector3(vert.posX, vert.posY, vert.posZ),
                            new Vector2(vert.texU, 1 - vert.texV),
                            poly.normals[i].ToVector3(),
                            ArxIOHelper.FromBGRA(lightCol));
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
