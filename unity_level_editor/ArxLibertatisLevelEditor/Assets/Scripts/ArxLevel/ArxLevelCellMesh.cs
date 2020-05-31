using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ArxLevel
{
    /// <summary>
    /// making a mesh per cell so they can be selected for editing. individual hiding is also possible (hiding cell that is being edited so we can display the poly mesh while editing)
    /// </summary>
    public class ArxLevelCellMesh : ArxLevelMeshBase
    {
        class CellMeshData
        {
            public Dictionary<ArxMaterialKey, SubMeshData> subMeshes = new Dictionary<ArxMaterialKey, SubMeshData>();
            public SubMeshData notFoundSubMesh;

            public CellMeshData()
            {
                notFoundSubMesh = new SubMeshData(MaterialsDatabase.NotFound);
                subMeshes[new ArxMaterialKey("", PolyType.GLOW, 0)] = notFoundSubMesh;
            }

            public SubMeshData GetSubMeshData(string texArxPath, PolyType polyType, float transVal)
            {
                var key = new ArxMaterialKey(texArxPath, polyType, transVal);
                if (!subMeshes.TryGetValue(key, out SubMeshData data))
                {
                    data = new SubMeshData(ArxMaterialManager.GetMaterial(key));
                    subMeshes[key] = data;
                }
                return data;
            }
        }

        public GameObject cellMesh = new GameObject();

        public GameObject[][] cells;


        public ArxLevelCellMesh(ArxLevel level) : base(level)
        {

        }

        public void CreateMesh()
        {
            cells = new GameObject[level.FTS.sceneHeader.sizex][];
            CellMeshData[][] cellMeshDatas = new CellMeshData[cells.Length][];
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new GameObject[level.FTS.sceneHeader.sizez];
                cellMeshDatas[i] = new CellMeshData[level.FTS.sceneHeader.sizez];
                for (int j = 0; j < level.FTS.sceneHeader.sizez; j++)
                {
                    var tmp = cells[i][j] = new GameObject();
                    tmp.transform.SetParent(cellMesh.transform);
                    cellMeshDatas[i][j] = new CellMeshData();
                }
            }

            LoadTextures();

            //TODO: could parallelize this
            for (int x = 0; x < level.FTS.sceneHeader.sizex; x++)
            {
                for (int z = 0; z < level.FTS.sceneHeader.sizez; z++)
                {
                    int cellIndex = x * level.FTS.sceneHeader.sizez + z;

                    var cell = level.FTS.cells[cellIndex];
                    var cellMeshData = cellMeshDatas[x][z];

                    for (int i = 0; i < cell.polygons.Length; i++)
                    {
                        SubMeshData subMesh;

                        var poly = cell.polygons[i];
                        if (tcToIndex.TryGetValue(poly.tex, out int textureIndex))
                        {
                            subMesh = cellMeshData.GetSubMeshData(texArxPaths[textureIndex], poly.type, poly.transval);
                        }
                        else
                        {
                            if (poly.tex != 0)
                            {
                                Debug.Log("not found: " + poly.tex);
                            }
                            subMesh = cellMeshData.notFoundSubMesh;
                        }

                        AddPoly(subMesh, poly);
                    }
                }
            }

            //make meshes
            for (int x = 0; x < level.FTS.sceneHeader.sizex; x++)
            {
                for (int z = 0; z < level.FTS.sceneHeader.sizez; z++)
                {
                    var cellObject = cells[x][z];
                    var cellData = cellMeshDatas[x][z];

                    foreach (var kv in cellData.subMeshes)
                    {
                        var subMesh = kv.Value;

                        GameObject matObj = new GameObject();

                        Mesh m = SubMeshToMesh(subMesh);

                        var mf = matObj.AddComponent<MeshFilter>();
                        mf.sharedMesh = m;
                        var mr = matObj.AddComponent<MeshRenderer>();
                        mr.sharedMaterial = subMesh.material;

                        matObj.transform.SetParent(cellObject.transform);
                    }
                }
            }

            cellMesh.transform.SetParent(level.LevelObject.transform);
            cellMesh.transform.localPosition = Vector3.zero;
            cellMesh.transform.localEulerAngles = Vector3.zero;
            cellMesh.transform.localScale = Vector3.one;
        }
    }
}
