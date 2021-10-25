namespace shaders_lib.Models
{
    /// <summary>
    /// Represents a 3d Model stored in Memory
    /// </summary>
    public class RawModel
    {
        public string Name;
        public string Path;
        public readonly int Handle;
        public readonly int VertexCount;

        public RawModel(int handle, int vertexCount)
        {
            this.Handle = handle;
            this.VertexCount = vertexCount;
        }
    }
}