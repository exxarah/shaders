using shaders_lib.Textures;

namespace shaders_lib
{
    /// <summary>
    /// Represents a 3d model and texture, paired together for ease of access
    /// </summary>
    public class TexturedModel
    {
        public readonly RawModel Model;
        public readonly Texture Texture;

        public TexturedModel(RawModel model, Texture texture)
        {
            Model = model;
            Texture = texture;
        }
    }
}