﻿using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using shaders_lib.Textures;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace shaders_lib
{
    public class Loader
    {
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
        public RawModel LoadToVAO(float[] positions, float[] uvs, int[] indices)
        {
            int vaoID = CreateVAO();
            BindIndicesBuffer(indices);
            StoreDataInAttributeList(0, 3, positions);
            StoreDataInAttributeList(1, 2, uvs);
            UnbindVAO();
            return new RawModel(vaoID, indices.Length);
        }

        /// <summary>
        /// Create a texture from a file
        /// </summary>
        /// <param name="fileName">The image to be loaded as a texture</param>
        /// <param name="format">The format of the image</param>
        /// <returns>The new texture</returns>
        /// <exception cref="Exception">Incase the TextureLoader is unable to properly load the provided image</exception>
        public Texture LoadTexture(string fileName)
        {
            string path = "Assets/textures/" + fileName;
            int textureID = CreateTexture();
            LoadImage(path);
            TextureParameters();
            UnbindTexture();
            return new Texture(textureID);
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

        #region Textures

        /// <summary>
        /// Create a Texture2D and bind it for further use
        /// </summary>
        /// <returns>The handle for the texture</returns>
        private int CreateTexture()
        {
            int textureObject = GL.GenTexture();
            _textures.Add(textureObject);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureObject);
            return textureObject;
        }

        /// <summary>
        /// Load an image into the currently bound Texture2D
        /// </summary>
        /// <param name="path">The image to load</param>
        private void LoadImage(string path)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(path);
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            var pixels = BuildPixelArray(image);
            
            GL.TexImage2D(
                TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                image.Width, image.Height,
                0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels
                );
        }

        /// <summary>
        /// Set the Parameters for the texture (if you don't do some of this, it breaks and is just black)
        /// </summary>
        private void TextureParameters()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        /// <summary>
        /// Unbind the current Texture2D by binding 0
        /// </summary>
        private void UnbindTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        
        /// <summary>
        /// Convert ImageSharp's image format into a byte array for use by OpenGL
        /// </summary>
        /// <param name="image">The loaded Image from ImageSharp</param>
        /// <returns>A byte array of format RGBA</returns>
        private byte[] BuildPixelArray(Image<Rgba32> image)
        {
            var pixels = new List<byte>(4 * image.Width * image.Height);
            for (int y = 0; y < image.Height; y++)
            {
                var row = image.GetPixelRowSpan(y);
                for (int x = 0; x < image.Width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }
            return pixels.ToArray();
        }

        #endregion
    }
}