using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace shaders_lib
{
    public class Loader
    {
        private List<int> _vaos = new List<int>();
        private List<int> _vbos = new List<int>();
        
        public Model LoadToVAO(float[] positions)
        {
            int vaoID = CreateVAO();
            StoreDataInAttributeList(0, positions);
            UnbindVAO();
            return new Model(vaoID, positions.Length/3);
        }

        public void CleanUp()
        {
            foreach (var vao in _vaos)
            {
                GL.DeleteVertexArray(vao);
            }

            foreach (var vbo in _vbos)
            {
                GL.DeleteBuffer(vbo);
            }
        }

        private int CreateVAO()
        {
            int vertexArrayObject = GL.GenVertexArray();
            _vaos.Add(vertexArrayObject);
            GL.BindVertexArray(vertexArrayObject);
            return vertexArrayObject;
        }

        private void StoreDataInAttributeList(int attributeNumber, float[] data)
        {
            int vertexBufferObject = GL.GenBuffer();
            _vbos.Add(vertexBufferObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                data.Length * sizeof(float),
                data,
                BufferUsageHint.StaticDraw
                );
            GL.VertexAttribPointer(
                attributeNumber,
                3,
                VertexAttribPointerType.Float,
                false,
                0,
                0
                );
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }
    }
}