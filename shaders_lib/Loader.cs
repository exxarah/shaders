using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace shaders_lib
{
    public class Loader
    {
        private List<int> _vaos = new List<int>();
        private List<int> _vbos = new List<int>();
        
        public Model LoadToVAO(float[] positions, int[] indices)
        {
            int vaoID = CreateVAO();
            BindIndicesBuffer(indices);
            StoreDataInAttributeList(0, positions);
            UnbindVAO();
            return new Model(vaoID, indices.Length);
        }

        /// <summary>
        /// To be called on Window.OnUnload(), ensures clean removal of all VAOs and VBOs
        /// </summary>
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

        /// <summary>
        /// Create a new VAO
        /// </summary>
        /// <returns>int reference to the new VAO</returns>
        private int CreateVAO()
        {
            int vertexArrayObject = GL.GenVertexArray();
            _vaos.Add(vertexArrayObject);
            GL.BindVertexArray(vertexArrayObject);
            return vertexArrayObject;
        }

        /// <summary>
        /// Create a VBO and store it in the currently bound VAO
        /// </summary>
        /// <param name="attributeNumber">The position to store data in</param>
        /// <param name="data">The data to be stored</param>
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

        /// <summary>
        /// Unbind VAO in a more readable way
        /// </summary>
        private void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Creates a VBO specifically for use as an element index array
        /// </summary>
        /// <param name="indices">The order of indices</param>
        private void BindIndicesBuffer(int[] indices)
        {
            int vboID = GL.GenBuffer();
            _vbos.Add(vboID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboID);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(int),
                indices,
                BufferUsageHint.StaticDraw
                );
        }
    }
}