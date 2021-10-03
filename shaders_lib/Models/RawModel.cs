namespace shaders_lib
{
    /// <summary>
    /// Represents a 3d Model stored in Memory
    /// </summary>
    public class RawModel
    {
        public readonly int Handle;
        public readonly int VertexCount;

        public RawModel(int handle, int vertexCount)
        {
            this.Handle = handle;
            this.VertexCount = vertexCount;
        }
    }
}