using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using shaders_lib.Entities;
using shaders_lib.Models;
using shaders_lib.Shaders;

namespace shaders_lib
{
    /// <summary>
    /// Contains OpenGL Rendering code, abstracts different methods of rendering away from application specific loops
    /// </summary>
    public class Renderer
    {
        private Color4 _clearDefault;

        /// <summary>
        /// Initializes a renderer to use
        /// </summary>
        /// <param name="clearDefault">The background colour to use by default</param>
        public Renderer(Color4 clearDefault)
        {
            this._clearDefault = clearDefault;
        }

        /// <summary>
        /// Prepare to render, set background colour, etc
        /// </summary>
        public void Prepare()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(_clearDefault);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        /// <summary>
        /// Draw an Entity to the screen
        /// </summary>
        /// <param name="entity">The entity to draw</param>
        /// <param name="shader">The shader to draw with</param>
        /// <param name="camera">The camera to get projection and view from</param>
        /// <param name="light">The light to get brightness from</param>
        public void Render(Entity entity, StaticShader shader, Camera camera, Light light)
        {
            shader.Start();
            TexturedModel model = entity.Model;
            var rawModel = model.Model;
            GL.BindVertexArray(rawModel.Handle);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            
            model.Material.BindTextures();

            // Set Uniforms here
            model.Material.SetUniforms(shader);
            shader.LoadEntity(entity);
            shader.LoadCamera(camera);
            shader.LoadLight(light);

            GL.DrawElements(PrimitiveType.Triangles, rawModel.VertexCount, DrawElementsType.UnsignedInt, 0);
            
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);
            shader.Stop();
        }
    }
}