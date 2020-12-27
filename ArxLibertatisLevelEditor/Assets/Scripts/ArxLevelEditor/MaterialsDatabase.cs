using UnityEngine;

namespace Assets.Scripts
{
    public static class MaterialsDatabase
    {
        public static Material TEST { get; private set; }
        public static Material ArxMat { get; private set; }
        public static Material ArxMatDoubleSided { get; private set; }
        public static Material ArxMatTransparent { get; private set; }
        public static Material ArxMatDoubleSidedTransparent { get; private set; }
        public static Material NotFound { get; private set; }
        public static Material GizmoMaterial { get; private set; }
        public static Material VertexMaterial { get; private set; }

        static MaterialsDatabase()
        {
            ArxMat = Object.Instantiate(Resources.Load<Material>("Materials/ArxMat"));
            ArxMatDoubleSided = Object.Instantiate(Resources.Load<Material>("Materials/ArxMatDoubleSided"));
            ArxMatTransparent = Object.Instantiate(Resources.Load<Material>("Materials/ArxMatTransparent"));
            ArxMatDoubleSidedTransparent = Object.Instantiate(Resources.Load<Material>("Materials/ArxMatDoubleSidedTransparent"));

            NotFound = Object.Instantiate(Resources.Load<Material>("Materials/NotFound"));
            TEST = Object.Instantiate(Resources.Load<Material>("Materials/TEST"));
            GizmoMaterial = Object.Instantiate(Resources.Load<Material>("Materials/Gizmo"));
            VertexMaterial = Object.Instantiate(Resources.Load<Material>("Materials/Vertex"));
        }
    }
}
