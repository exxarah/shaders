using System.Numerics;
using shaders_lib.Shaders;

namespace shaders_lib.Entities
{
    public class Light
    {
        public Vector3 Position { get; private set; }
        public Vector3 Color;

        public float Brightness;

        public float AmbientStrength;
        public float DiffuseStrength;
        public float SpecularStrength;

        public Light(Vector3 position, Vector3 color)
        {
            Position = position;
            Color = color;
            Brightness = 2.5f;
            AmbientStrength = 0.2f;
            DiffuseStrength = 0.5f;
            SpecularStrength = 1.0f;
        }

        public void SetUniforms(Shader shader)
        {
            shader.SetVector3("lightPosition", Position);
            shader.SetVector3("light.ambient", Brightness * new Vector3(AmbientStrength, AmbientStrength, AmbientStrength));
            shader.SetVector3("light.diffuse", Brightness * new Vector3(DiffuseStrength, DiffuseStrength, DiffuseStrength));
            shader.SetVector3("light.specular", Brightness * new Vector3(SpecularStrength, SpecularStrength, SpecularStrength));
        }
    }
}