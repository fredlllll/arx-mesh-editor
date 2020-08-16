using Assets.Scripts.Data;
using Assets.Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ArxLevel
{
    /// <summary>
    /// a mesh trying to make display of the level as fast as possible by putting everything into as few meshes as possible
    /// </summary>
    public class ArxLevelBigMesh : ArxLevelMeshBase
    {
        public ArxLevelBigMesh(ArxLevel level) : base(level)
        {
        }

        public void CreateMesh()
        {
            CreateMeshBegin();

            var fts = level.ArxLevelNative.FTS;

            Dictionary<ArxMaterialKey, SubMeshData> subMeshes = new Dictionary<ArxMaterialKey, SubMeshData>();
            SubMeshData notFoundSubMesh = new SubMeshData(MaterialsDatabase.NotFound);
            subMeshes[new ArxMaterialKey("", PolyType.GLOW, 0)] = notFoundSubMesh; //so we can use it in a for over subMeshes later

            LoadTextures();

            for (int c = 0; c < fts.cells.Length; c++)
            {
                var cell = fts.cells[c];
                for (int p = 0; p < cell.polygons.Length; p++)
                {
                    var poly = cell.polygons[p];

                    SubMeshData subMesh;
                    if (tcToIndex.TryGetValue(poly.tex, out int textureIndex))
                    {
                        var key = new ArxMaterialKey(texArxPaths[textureIndex], poly.type, poly.transval);
                        if (!subMeshes.TryGetValue(key, out subMesh))
                        {
                            subMesh = new SubMeshData(ArxMaterialManager.GetMaterial(key));
                            subMeshes[key] = subMesh;
                        }
                    }
                    else
                    {
                        if (poly.tex != 0)
                        {
                            Debug.Log("not found: " + poly.tex);
                        }
                        subMesh = notFoundSubMesh; //use not found submesh
                    }

                    AddPoly(subMesh, poly);
                }
            }

            GameObject lvl = new GameObject(level.Name + "_mesh");

            //TODO: putting it all in one mesh has proven difficult cause im apparently too stupid to make the indices work properly...
            /*List<Vector3> meshVerts = new List<Vector3>();
            List<Vector2> meshUvs = new List<Vector2>();
            List<Vector3> meshNorms = new List<Vector3>();
            List<Color> meshColors = new List<Color>();
            for (int i = 0; i < subMeshVerts.Length; i++)
            {
                int indexOffset = meshVerts.Count;

                var verts = subMeshVerts[i];
                var uvs = subMeshUvs[i];
                var norms = subMeshNorms[i];
                var colors = subMeshColors[i];
                var indices = subMeshIndices[i];

                for (int j = 0; j < verts.Count; j++)
                {
                    meshVerts.Add(verts[j]);
                    meshUvs.Add(uvs[j]);
                    meshNorms.Add(norms[j]);
                    meshColors.Add(colors[j]);
                    indices[j] += indexOffset;
                }
            }

            Mesh m = new Mesh();
            m.vertices = meshVerts.ToArray();
            m.uv = meshUvs.ToArray();
            m.normals = meshNorms.ToArray();
            m.colors = meshColors.ToArray();

            m.subMeshCount = materials.Length;
            for (int i = 0; i < materials.Length; i++)
            {
                var indices = subMeshIndices[i];
                m.SetIndices(indices.ToArray(), MeshTopology.Triangles, i);
            }

            var mf = lvl.AddComponent<MeshFilter>();
            mf.sharedMesh = m;
            var mr = lvl.AddComponent<MeshRenderer>();
            mr.sharedMaterials = materials;
            m.RecalculateBounds();
            m.RecalculateTangents();
            m.Optimize();
            */

            //one mesh per material
            foreach (var kv in subMeshes)
            {
                var subMesh = kv.Value;

                GameObject matObj = new GameObject();

                Mesh m = SubMeshToMesh(subMesh);

                var mf = matObj.AddComponent<MeshFilter>();
                mf.sharedMesh = m;
                var mr = matObj.AddComponent<MeshRenderer>();
                mr.sharedMaterial = subMesh.material;

                matObj.transform.SetParent(lvl.transform);
            }

            lvl.transform.SetParent(level.LevelObject.transform);
            lvl.transform.localPosition = Vector3.zero;
            lvl.transform.localEulerAngles = Vector3.zero;
            lvl.transform.localScale = Vector3.one;
        }
    }
}
