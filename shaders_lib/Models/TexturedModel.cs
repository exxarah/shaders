using shaders_lib.Shaders;

namespace shaders_lib.Models
{
    /// <summary>
    /// Represents a 3d model and texture, paired together for ease of access
    /// </summary>
    public class TexturedModel
    {
        public RawModel Model;
        public Material Material;

        public TexturedModel(RawModel model, Material material)
        {
            Model = model;
            Material = material;
        }
    }
}