using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using shaders_lib.Shaders;

namespace shaders_lib
{
    public class Renderer
    {
        private static readonly float Fov = 60;
        private static readonly float NearPlane = 0.1f;
        private static readonly float FarPlane = 100000000000f;
        
        private Color4 _clearDefault;
        private Vector2i _size = Vector2i.Zero;
        private Matrix4 _projectionMatrix;

        /// <summary>
        /// Initializes a renderer to use
        /// </summary>
        /// <param name="clearDefault">The background colour to use by default</param>
        public Renderer(Color4 clearDefault, Vector2i size)
        {
            this._clearDefault = clearDefault;
            Resize(size);
        }

        /// <summary>
        /// Resize the Renderer (used for aspect ratio)
        /// </summary>
        /// <param name="newSize">The new size</param>
        public void Resize(Vector2i newSize)
        {
            _size = newSize;
            CreateProjectionMatrix();
        }
        
        /// <summary>
        /// Prepare to render, set background colour, etc
        /// </summary>
        public void Prepare()
        {
            GL.ClearColor(_clearDefault);
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
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, model.Texture.Handle);
            
            // Set Uniforms here
            Matrix4 transformationMatrix =
                Util.Maths.CreateTransformationMatrix(entity.Position, entity.Rotation, entity.Scale);
            shader.SetMatrix4("transformationMatrix", transformationMatrix);
            shader.SetMatrix4("projectionMatrix", _projectionMatrix);

            GL.DrawElements(PrimitiveType.Triangles, rawModel.VertexCount, DrawElementsType.UnsignedInt, 0);
            
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);
            shader.Stop();
        }

        private void CreateProjectionMatrix()
        {
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), _size.X / (float) _size.Y, NearPlane, FarPlane);
            _projectionMatrix = Matrix4.Transpose(_projectionMatrix);
        }
    }
}