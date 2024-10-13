using ArxLibertatisEditorIO.MediumIO.FTS;
using ArxLibertatisEditorIO.RawIO;
using Assets.Scripts.ArxLevelEditor;
using Assets.Scripts.ArxLevelEditor.Mesh;
using Assets.Scripts.ArxNative.IO;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ArxLevelLoading
{
    public static class LevelSaver
    {
        public static void SaveLevel(Level level, string name)
        {
            var lvln = level.MediumArxLevel;

            Vector3 camPos = LevelEditor.EditorCamera.transform.position * 100;
            camPos.y *= -1;
            lvln.DLF.header.positionEdit = camPos.ToNumerics();
            lvln.DLF.header.eulersEdit = LevelEditor.EditorCamera.transform.eulerAngles.ToNumerics(); //TODO: might have to fix rotation because different handedness?
            lvln.DLF.header.eulersEdit = level.LevelOffset.ToNumerics();

            SaveMesh(level);

            var ral = new RawArxLevel();
            lvln.SaveTo(ral).SaveLevel(name, false);
        }

        class LevelCell
        {
            public readonly int x, z;
            public readonly List<Tuple<EditorMaterial, EditablePrimitiveInfo>> primitives = new List<Tuple<EditorMaterial, EditablePrimitiveInfo>>();

            public LevelCell(int x, int z)
            {
                this.x = x;
                this.z = z;
            }

            public void AddPrimitive(EditorMaterial mat, EditablePrimitiveInfo prim)
            {
                primitives.Add(new Tuple<EditorMaterial, EditablePrimitiveInfo>(mat, prim));
            }
        }

        static Vector2Int GetPrimitiveCellPos(EditablePrimitiveInfo prim)
        {
            Vector2 pos = Vector2.zero;

            int vertCount = prim.VertexCount;
            for (int i = 0; i < vertCount; i++)
            {
                var v = prim.vertices[i];
                pos += new Vector2(v.position.x, v.position.z);
            }
            pos /= vertCount;
            return new Vector2Int((int)(pos.x / 100), (int)(pos.y / 100));
        }

        static void SaveMesh(Level level)
        {
            // create texture containers

            HashSet<string> uniqueTexturePaths = new HashSet<string>();

            foreach (var kv in level.EditableLevelMesh.MaterialMeshes)
            {
                if (kv.Value.PrimitiveCount > 0) //only add if we have primitives for this material
                {
                    //TODO: remove that later
                    //var texPath = kv.Key.TexturePath.Replace(EditorSettings.DataDir, "");
                    uniqueTexturePaths.Add(kv.Key.TexturePath);
                }
            }

            var fts = level.MediumArxLevel.FTS;
            fts.textureContainers.Clear();
            int i = 1; //nothing speaks against just using a normal index for tc, i dont know why they ever used random ints, has to be 1 based
            Dictionary<string, int> texPathToTc = new Dictionary<string, int>();
            foreach (var path in uniqueTexturePaths)
            {
                var texPath = path.Replace(ArxLibertatisEditorIO.ArxPaths.DataDir, "");
                fts.textureContainers.Add(new ArxLibertatisEditorIO.MediumIO.FTS.TextureContainer() { texturePath = texPath, containerId = i });
                texPathToTc[path] = i;
                i++;
            }

            //create cells
            int sizex = fts.sceneHeader.sizex;
            int sizez = fts.sceneHeader.sizez;
            LevelCell[] cells = new LevelCell[sizex * sizez];
            for (int z = 0, index = 0; z < sizez; z++)
            {
                for (int x = 0; x < sizex; x++, index++)
                {
                    //int index = ArxIOHelper.XZToCellIndex(x, z, sizex, sizez);
                    var cell = new LevelCell(x, z);
                    cells[index] = cell;
                }
            }

            //add primitives to cells
            Vector2Int maxPos = new Vector2Int(sizex - 1, sizez - 1); //if clamp is inclusive we have to sub 1
            foreach (var kv in level.EditableLevelMesh.MaterialMeshes)
            {
                foreach (var prim in kv.Value.Primitives)
                {
                    var cellpos = GetPrimitiveCellPos(prim);
                    cellpos.Clamp(Vector2Int.zero, maxPos);
                    var cell = cells[ArxIOHelper.XZToCellIndex(cellpos.x, cellpos.y, sizex, sizez)];
                    cell.AddPrimitive(kv.Key, prim);
                }
            }

            AutoDictionary<short, Room> roomPolyDatas = new AutoDictionary<short, Room>(_ => new Room());
            /*List<FTS_IO_EP_DATA>[] roomPolyDatas = new List<FTS_IO_EP_DATA>[fts.rooms.Count];
            for (i = 0; i < fts.rooms.Count; ++i)
            {
                roomPolyDatas[i] = new List<FTS_IO_EP_DATA>();
            }*/

            var llf = level.MediumArxLevel.LLF;
            llf.lightColors.Clear();

            //put primitves into polys in their cells
            for (int z = 0, index = 0; z < sizez; z++)
            {
                for (int x = 0; x < sizex; x++, index++)
                {
                    //int index = ArxIOHelper.XZToCellIndex(x, z, sizex, sizez);
                    var myCell = cells[index];
                    var ftsCell = fts.cells[index];
                    for (i = 0; i < myCell.primitives.Count; i++)
                    {
                        var tup = myCell.primitives[i];
                        var mat = tup.Item1;
                        var prim = tup.Item2;
                        var poly = new Polygon();
                        //copy data over
                        poly.area = prim.area;
                        poly.norm = prim.norm.ToNumerics();
                        poly.norm2 = prim.norm2.ToNumerics();
                        poly.room = prim.room;
                        poly.polyType = prim.polyType; //this is completely ignoring mat polytype atm, but it should be sync with prim type anyway

                        if (poly.room >= 0)
                        {
                            var polyData = new RoomPolygon();
                            polyData.cell_x = (short)x;
                            polyData.cell_z = (short)z;
                            polyData.idx = (short)i;
                            roomPolyDatas[poly.room].polygons.Add(polyData);
                        }

                        //copy vertices
                        for (int j = 0; j < 4; j++) //always save all 4 vertices regardless of if its a triangle or quad
                        {
                            var vert = prim.vertices[j];
                            var polyVert = poly.vertices[j];
                            polyVert.position = vert.position.ToNumerics();
                            polyVert.normal = vert.normal.ToNumerics();
                            polyVert.uv = vert.uv.ToNumerics();
                            polyVert.uv.Y = 1 - polyVert.uv.Y;
                        }

                        llf.lightColors.Add(prim.vertices[0].color.ToArxIo());
                        llf.lightColors.Add(prim.vertices[1].color.ToArxIo());
                        llf.lightColors.Add(prim.vertices[2].color.ToArxIo());
                        if (poly.polyType.HasFlag(ArxLibertatisEditorIO.Util.PolyType.QUAD))
                        {
                            llf.lightColors.Add(prim.vertices[3].color.ToArxIo());
                        }

                        //set material stuff
                        poly.textureContainerId = texPathToTc[mat.TexturePath];//keyerrors should not be possible on this
                        poly.transVal = mat.TransVal;
                        //polytype is set from primitive

                        ftsCell.polygons.Add(poly);
                    }
                    fts.cells[index] = ftsCell;
                }
            }

            var maxRoom = roomPolyDatas.Keys.Max();
            fts.rooms.Clear();
            for (i = 0; i < maxRoom + 1; i++)
            {
                if (roomPolyDatas.TryGetValue((short)i, out var room))
                {
                    fts.rooms.Add(room);
                }
                else
                {
                    fts.rooms.Add(new Room());
                }
            }
        }
    }
}
