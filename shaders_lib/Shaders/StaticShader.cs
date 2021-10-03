using OpenTK.Graphics.OpenGL;

namespace shaders_lib.Shaders
{
    /// <summary>
    /// A simple shader that doesn't do anything too fancy
    /// </summary>
    public class StaticShader : Shader
    {
        private static readonly string VertexPath = "Assets/glsl/static.vert";
        private static readonly string FragmentPath = "Assets/glsl/static.frag";
        public StaticShader() : base(VertexPath, FragmentPath) { }
        
        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCoord");
        }
    }
}