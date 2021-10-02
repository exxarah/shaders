using OpenTK.Graphics.OpenGL;

namespace shaders_lib.Shaders
{
    public class StaticShader : Shader
    {
        // Please god someone save me from this awful path
        private static readonly string VertexPath = "../../../shaders/" + "shader" + ".vert";
        private static readonly string FragmentPath = "../../../shaders/" + "shader" + ".frag";
        public StaticShader() : base(VertexPath, FragmentPath) { }
        
        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
        }
    }
}