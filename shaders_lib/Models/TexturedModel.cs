using shaders_lib.Textures;

namespace shaders_lib
{
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