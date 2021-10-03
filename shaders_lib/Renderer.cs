using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using shaders_lib.Shaders;

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
        /// Draw an Entity to the screen
        /// </summary>
        /// <param name="entity">The entity to draw</param>
        /// <param name="shader">The shader to draw with</param>
        public void Render(Entity entity, StaticShader shader)
        {
            shader.Start();
            TexturedModel model = entity.Model;
            var rawModel = model.Model;
            GL.BindVertexArray(rawModel.Handle);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            Matrix4 transformationMatrix =
                Util.Maths.CreateTransformationMatrix(entity.Position, entity.Rotation, entity.Scale);
            shader.SetMatrix4("transformationMatrix", transformationMatrix);
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, model.Texture.Handle);

            GL.DrawElements(PrimitiveType.Triangles, rawModel.VertexCount, DrawElementsType.UnsignedInt, 0);
            
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);
            shader.Stop();
        }
    }
}