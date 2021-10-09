using OpenTK.Mathematics;

namespace shaders_lib.Shaders
{
    public class MetallicRoughnessMaterial : Material
    {
        public Texture BaseColourTexture;
        public Vector3 BaseColourFactor;
        public Texture MetallicRoughnessTexture;
        public float MetallicFactor;
        public float RoughnessFactor;
    }
}