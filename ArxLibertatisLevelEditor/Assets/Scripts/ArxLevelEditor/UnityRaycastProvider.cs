using ArxLibertatisEditorIO.MediumIO;
using ArxLibertatisEditorIO.Util;
using ArxLibertatisLightingCalculatorLib;
using ArxLibertatisLightingCalculatorLib.RayCasting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor
{
    public class UnityRaycastProvider : IRaycastProvider
    {
        private GameObject collisionObject;
        private MeshCollider meshCollider;

        public void Initialize(LightingCalculatorDistanceAngleShadowBase lc, MediumArxLevel mal)
        {
            collisionObject = new GameObject("LightingCollisionMesh");
            collisionObject.hideFlags = HideFlags.HideAndDontSave;

            UnityEngine.Mesh mesh = BuildCombinedMesh(lc,mal);

            meshCollider = collisionObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.cookingOptions = MeshColliderCookingOptions.None;

            collisionObject.isStatic = true;
        }

        public RaycastHitData Raycast(System.Numerics.Vector3 origin, System.Numerics.Vector3 direction, float maximumT)
        {
            //unity raycast normalizes direction, so we have to manually calculate the max length
            float length = direction.Length();

            bool hit = Physics.Raycast(
                new Vector3(origin.X, origin.Y, origin.Z),
                new Vector3(direction.X, direction.Y, direction.Z),
                out RaycastHit hitInfo,
                maximumT*length,
                ~0,
                QueryTriggerInteraction.Ignore);

            return new RaycastHitData
            {
                HitCount = hit ? 1 : 0
            };
        }

        public void Dispose()
        {
            if (collisionObject != null)
            {
                Object.Destroy(collisionObject);
                collisionObject = null;
            }
        }

        bool SkipPolygon(LightingCalculatorDistanceAngleShadowBase lc, PolyType pt)
        {
            var polyTypesToSkip = lc.GetPolyTypesToSkip();
            for (int k = 0; k < polyTypesToSkip.Length; ++k)
            {
                if (pt.HasFlag(polyTypesToSkip[k]))
                {
                    return true;
                }
            }
            return false;
        }

        private UnityEngine.Mesh BuildCombinedMesh(LightingCalculatorDistanceAngleShadowBase lc, MediumArxLevel mal)
        {
            var mesh = new UnityEngine.Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0; i < mal.FTS.cells.Count; ++i)
            {
                var c = mal.FTS.cells[i];
                for (int j = 0; j < c.polygons.Count; ++j)
                {
                    var p = c.polygons[j];
                    if (SkipPolygon(lc, p.polyType))
                    {
                        continue;
                    }

                    var v0 = p.vertices[0].position;
                    var v1 = p.vertices[1].position;
                    var v2 = p.vertices[2].position;
                    var v3 = p.vertices[3].position;

                    vertices.Add(new Vector3(v0.X, v0.Y, v0.Z));
                    vertices.Add(new Vector3(v1.X, v1.Y, v1.Z));
                    vertices.Add(new Vector3(v2.X, v2.Y, v2.Z));
                    //backface vertices for double sided collision
                    vertices.Add(new Vector3(v0.X, v0.Y, v0.Z));
                    vertices.Add(new Vector3(v2.X, v2.Y, v2.Z));
                    vertices.Add(new Vector3(v1.X, v1.Y, v1.Z));
                    if (p.polyType.HasFlag(PolyType.QUAD))
                    {
                        vertices.Add(new Vector3(v1.X, v1.Y, v1.Z));
                        vertices.Add(new Vector3(v2.X, v2.Y, v2.Z));
                        vertices.Add(new Vector3(v3.X, v3.Y, v3.Z));
                        //backface vertices for double sided collision
                        vertices.Add(new Vector3(v2.X, v2.Y, v2.Z));
                        vertices.Add(new Vector3(v1.X, v1.Y, v1.Z));
                        vertices.Add(new Vector3(v3.X, v3.Y, v3.Z));
                    }
                }
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = Enumerable.Range(0, vertices.Count).ToArray();


            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
