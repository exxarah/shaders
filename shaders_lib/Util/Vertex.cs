using OpenTK.Mathematics;

namespace shaders_lib.Util
{
    public class Vertex
    {
        private const int NoIndex = -1;

        public int Index { get; private set; }
        public Vector3 Position { get; private set; }
        public float Length => Position.Length;
        
        public int TextureIndex { get; set; } = NoIndex;
        public int NormalIndex { get; set; } = NoIndex;
        public Vertex DuplicateVertex { get; set; } = null;

        public bool IsSet => TextureIndex != NoIndex && NormalIndex != NoIndex;

        public Vertex(int index, Vector3 position)
        {
            Index = index;
            Position = position;
        }

        public bool SameTextureAndNormal(int textureIndex, int normalIndex)
        {
            return textureIndex == TextureIndex && normalIndex == NormalIndex;
        }
    }
}