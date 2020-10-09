namespace Assets.Scripts.ArxLevelEditor.Mesh
{
    public abstract class EditablePrimitive
    {
        public readonly EditableVertex[] vertices;
        byte current = 0;

        public EditablePrimitive(byte size)
        {
            vertices = new EditableVertex[size];
        }

        public void AddVertex(EditableVertex v)
        {
            vertices[current++] = v;
        }
    }
}
