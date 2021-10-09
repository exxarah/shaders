namespace shaders_lib.Shaders
{
    /// <summary>
    /// Represents an Image Texture, to be applied to a model
    /// </summary>
    public class Texture
    {
        public readonly int Handle;

        public Texture(int handle)
        {
            Handle = handle;
        }
    }
}