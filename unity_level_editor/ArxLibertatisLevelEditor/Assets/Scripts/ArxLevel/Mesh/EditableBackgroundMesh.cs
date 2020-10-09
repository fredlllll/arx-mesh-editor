using Assets.Scripts.ArxNative;
using Assets.Scripts.ArxNative.IO;
using Assets.Scripts.ArxNative.IO.FTS;
using Assets.Scripts.ArxNative.IO.LLF;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ArxLevel.Mesh
{
    public class EditableBackgroundMesh : MonoBehaviour
    {
        private void AddPolyToSubMesh(SubMeshData data, FTS_IO_EERIEPOLY poly, LLF_IO llf, ref int lightIndex)
        {
            int firstVert = data.verts.Count;

            bool isQuad = poly.type.HasFlag(PolyType.QUAD);

            int vertCount = isQuad ? 4 : 3;
            for (int i = 0; i < vertCount; i++)
            {
                var vert = poly.vertices[i];
                data.verts.Add(new Vector3(vert.posX, vert.posY, vert.posZ));
                data.uvs.Add(new Vector2(vert.texU, 1 - vert.texV));
                data.norms.Add(poly.normals[i].ToVector3());
                data.colors.Add(ArxIOHelper.FromBGRA(llf.lightColors[lightIndex++]));
            }

            data.indices.Add(firstVert);
            data.indices.Add(firstVert + 1);
            data.indices.Add(firstVert + 2);
            if (isQuad)
            {
                data.indices.Add(firstVert + 2);
                data.indices.Add(firstVert + 1);
                data.indices.Add(firstVert + 3);
            }
        }

        private UnityEngine.Mesh SubMeshDataToMesh(SubMeshData data)
        {
            UnityEngine.Mesh m = new UnityEngine.Mesh();

            m.vertices = data.verts.ToArray();
            m.uv = data.uvs.ToArray();
            m.normals = data.norms.ToArray();
            m.colors = data.colors.ToArray();
            m.triangles = data.indices.ToArray();

            return m;
        }

        public void LoadFrom(ArxLevelNative arxLevelNative)
        {
            gameObject.name = arxLevelNative.LastLoadName + "_mesh";

            var fts = arxLevelNative.FTS;
            var llf = arxLevelNative.LLF;

            var TexturePathLibrary = new TexturePathLibrary(fts);
            TexturePathLibrary.LoadTexturePaths();

            Dictionary<ArxMaterialKey, SubMeshData> subMeshes = new Dictionary<ArxMaterialKey, SubMeshData>();
            SubMeshData notFoundSubMesh = new SubMeshData(MaterialsDatabase.NotFound);
            subMeshes[new ArxMaterialKey("", PolyType.GLOW, 0)] = notFoundSubMesh; //so we can use it in a for over subMeshes later

            int lightIndex = 0; //for loading vertex colors from llf

            for (int c = 0; c < fts.cells.Length; c++)
            {
                var cell = fts.cells[c];
                for (int p = 0; p < cell.polygons.Length; p++)
                {
                    var poly = cell.polygons[p];

                    SubMeshData subMesh;
                    if (TexturePathLibrary.TryGetTexturePath(poly.tex, out string texturePath))
                    {
                        var key = new ArxMaterialKey(texturePath, poly.type, poly.transval);
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

                    AddPolyToSubMesh(subMesh, poly, llf, ref lightIndex);
                }
            }

            foreach (var kv in subMeshes)
            {
                var subMesh = kv.Value;

                GameObject matObj = new GameObject();

                UnityEngine.Mesh m = SubMeshDataToMesh(subMesh);

                var mf = matObj.AddComponent<MeshFilter>();
                mf.sharedMesh = m;
                var mr = matObj.AddComponent<MeshRenderer>();
                mr.sharedMaterial = subMesh.material;

                matObj.transform.SetParent(transform);
            }

            //TODO: add to parent from the outside
            /*lvl.transform.SetParent(level.LevelObject.transform);
            lvl.transform.localPosition = Vector3.zero;
            lvl.transform.localEulerAngles = Vector3.zero;
            lvl.transform.localScale = Vector3.one;*/


        }

        public void SaveTo(ArxLevelNative arxLevelNative)
        {
            //TODO: save to fts and llf
        }
    }
}
