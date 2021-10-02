using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace shaders_lib
{
    public class Renderer
    {
        private readonly Color4 clearDefault;

        /// <summary>
        /// Initializes a renderer to use
        /// </summary>
        /// <param name="clearDefault">The background colour to use by default</param>
        public Renderer(Color4 clearDefault)
        {
            this.clearDefault = clearDefault;
        }
        
        /// <summary>
        /// Prepare to render, set background colour, etc
        /// </summary>
        public void Prepare()
        {
            GL.ClearColor(clearDefault);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        /// <summary>
        /// Draw a Model to the screen
        /// </summary>
        /// <param name="model">The Model to draw</param>
        public void Render(Model model)
        {
            GL.BindVertexArray(model.ID);
            GL.EnableVertexAttribArray(0);
            GL.DrawElements(PrimitiveType.Triangles, model.VertexCount, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
    }
}