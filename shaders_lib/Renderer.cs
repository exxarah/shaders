using OpenTK.Graphics.OpenGL;

namespace shaders_lib
{
    public class Renderer
    {
        public void Prepare()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Render(Model model)
        {
            GL.BindVertexArray(model.ID);
            GL.EnableVertexAttribArray(0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.VertexCount);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
    }
}