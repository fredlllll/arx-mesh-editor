using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class PrimitiveMeshHelper
    {
        private static Mesh _cubeMesh;
        private static Mesh _sphereMesh;
        private static Mesh _capsuleMesh;
        private static Mesh _cylinderMesh;
        private static Mesh _planeMesh;
        private static Mesh _quadMesh;

        public static Mesh GetCubeMesh()
        {
            if (_cubeMesh == null) _cubeMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
            return _cubeMesh;
        }

        public static Mesh GetSphereMesh()
        {
            if (_sphereMesh == null) _sphereMesh = Resources.GetBuiltinResource<Mesh>("New-Sphere.fbx");
            return _sphereMesh;
        }

        public static Mesh GetCapsuleMesh()
        {
            if (_capsuleMesh == null) _capsuleMesh = Resources.GetBuiltinResource<Mesh>("New-Capsule.fbx");
            return _capsuleMesh;
        }

        public static Mesh GetCylinderMesh()
        {
            if (_cylinderMesh == null) _cylinderMesh = Resources.GetBuiltinResource<Mesh>("New-Cylinder.fbx");
            return _cylinderMesh;
        }

        public static Mesh GetPlaneMesh()
        {
            if (_planeMesh == null) _planeMesh = Resources.GetBuiltinResource<Mesh>("New-Plane.fbx");
            return _planeMesh;
        }

        public static Mesh GetQuadMesh()
        {
            if (_quadMesh == null) _quadMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
            return _quadMesh;
        }
    }
}
