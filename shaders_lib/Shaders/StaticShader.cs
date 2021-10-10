using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using shaders_lib.Entities;

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
            BindAttribute(2, "normal");
        }

        public void LoadEntity(Entity entity)
        {
            Matrix4 transformationMatrix =
                Util.Maths.CreateTransformationMatrix(entity.Position, entity.Rotation, entity.Scale);
            SetMatrix4("transformationMatrix", transformationMatrix);
        }

        public void LoadCamera(Camera camera)
        {
            SetMatrix4("projectionMatrix", camera.GetProjectionMatrix());
            SetMatrix4("viewMatrix", camera.GetViewMatrix());
        }
    }
}