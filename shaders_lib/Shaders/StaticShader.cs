using OpenTK.Graphics.OpenGL;

namespace shaders_lib.Shaders
{
    public class StaticShader : Shader
    {
        private static readonly string VertexPath = "Assets/glsl/static.vert";
        private static readonly string FragmentPath = "Assets/glsl/static.frag";
        public StaticShader() : base(VertexPath, FragmentPath) { }
        
        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
        }
    }
}