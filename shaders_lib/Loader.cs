using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using shaders_lib.Models;
using shaders_lib.Shaders;
using shaders_lib.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace shaders_lib
{
    /// <summary>
    /// A handy class to take care of loading resources. Keeps track of OpenGL handles to ensure they're cleaned up,
    /// and gives a place to store different loading methods (eg, OBJ vs FBX, PNG vs JPG)
    /// </summary>
    public class Loader
    {
        private const string ModelPath = "Assets/models/";

        private List<int> _vaos = new List<int>();
        private List<int> _vbos = new List<int>();
        private List<int> _textures = new List<int>();

        /// <summary>
        /// Create a model from arrays of data
        /// </summary>
        /// <param name="positions">The x, y, z coordinates of all the vertices</param>
        /// <param name="uvs">The UV Texture Coordinates of the vertices</param>
        /// <param name="indices">The indices of vertices, how they relate to each other and form triangles</param>
        /// <returns>The new model</returns>
        public RawModel LoadToVao(float[] positions, float[] uvs, int[] indices, float[] normals)
        {
            int vaoID = CreateVAO();
            BindIndicesBuffer(indices);
            StoreDataInAttributeList(0, 3, positions);
            StoreDataInAttributeList(1, 2, uvs);
            StoreDataInAttributeList(2, 3, normals);
            UnbindVAO();
            return new RawModel(vaoID, indices.Length);
        }

        public RawModel LoadToVao(ModelData data)
        {
            return LoadToVao(data.Vertices, data.Uvs, data.Indices, data.Normals);
        }

        public RawModel LoadModel(string fileName)
        {
            ModelData data;
            try
            {
                data = ParseFile(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred while loading Model({fileName}).\n\n{e}");
                throw;
            }
            RawModel model = LoadToVao(data);
            model.Name = data.Name;
            model.Path = fileName;
            return model;
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

            foreach (var texture in _textures)
            {
                GL.DeleteTexture(texture);
            }
        }

        #region VAO/VBO

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
        /// <param name="coordinateSize">The number of entries per vertex</param>
        /// <param name="data">The data to be stored</param>
        private void StoreDataInAttributeList(int attributeNumber, int coordinateSize, float[] data)
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
                coordinateSize,
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

        #endregion

        #region Models

        public ModelData ParseFile(string fileName)
        {
            string path = ModelPath + fileName;
            string fileType = fileName.Split('.').Last();

            ModelData data;
            
            if (fileType == "obj")
            {
                data = ObjLoader.Parse(path);
            }
            else
            {
                throw new ModelFormatException();
            }

            return data;
        }

        #endregion
    }
}