﻿using UnityEngine;

namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public class EditableVertex
    {
        public readonly Vector3 position;
        public readonly Vector2 uv;
        public readonly Vector3 normal;
        public readonly Color color;

        public EditableVertex(Vector3 pos, Vector2 uv, Vector3 norm, Color color)
        {
            this.position = pos;
            this.uv = uv;
            this.normal = norm;
            this.color = color;
        }
    }
}
