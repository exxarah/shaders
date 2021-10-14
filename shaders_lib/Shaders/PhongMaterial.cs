using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace shaders_lib.Shaders
{
    public class PhongMaterial : Material
    {
        public Texture DiffuseTexture;
        public Vector3 DiffuseFactor;
        public Vector3 SpecularFactor;
        public float ShininessFactor;

        public PhongMaterial(Texture diffuse)
        {
            DiffuseTexture = diffuse;
            DiffuseFactor = Vector3.One;
            SpecularFactor = new Vector3(0.5f, 0.5f, 0.5f);
            ShininessFactor = 32f;
        }

        public override void BindTextures()
        {
            base.BindTextures();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, DiffuseTexture.Handle);
        }

        public override void SetUniforms(Shader shader)
        {
            base.SetUniforms(shader);
            shader.SetVector3("material.diffuseFactor", DiffuseFactor);
            shader.SetVector3("material.specularFactor", SpecularFactor);
            shader.SetFloat("material.shininessFactor", ShininessFactor);
        }
    }
}