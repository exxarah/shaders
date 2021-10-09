using OpenTK.Mathematics;

namespace shaders_lib.Shaders
{
    public abstract class Material
    {
        public Texture NormalsTexture;
        public float NormalsFactor;
        public Texture OcclusionTexture;
        public float OcclusionFactor;
        public Texture EmissiveTexture;
        public Vector3 EmissiveFactor;

        public virtual void BindTextures()
        {
            // Bind standard Material Textures here
        }

        public virtual void SetUniforms(Shader shader)
        {
            // Set standard material uniforms here
        }
    }
}