namespace shaders_lib
{
    /// <summary>
    /// Represents a 3d Model stored in Memory
    /// </summary>
    public class Model
    {
        public readonly int ID;
        public readonly int VertexCount;

        public Model(int id, int vertexCount)
        {
            this.ID = id;
            this.VertexCount = vertexCount;
        }
    }
}