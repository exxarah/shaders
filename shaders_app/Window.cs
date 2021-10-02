﻿using System;
using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTK.Graphics.OpenGL;
using shaders_lib;

namespace shaders_app
{
    public class Window : GameWindow
    {
        private readonly float[] _vertices = new float[]
        {
            // Center of the screen is 0. whole screen is -1 to 1
            //x      y     z    // colors
             1.0f,  1.0f, 0.0f, 1.0f, 0.0f, 0.0f,
             1.0f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            -1.0f,  1.0f, 0.0f, 1.0f, 1.0f, 1.0f,
        };

        private readonly uint[] _indices = new uint[]
        {
            0, 1, 3,    // triangle 1
            1, 2, 3     // triangle 2
        };

        private readonly string fragShader = "mandelbrot";
        private readonly string vertShader = "shader";

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        private Shader _shader;
        private Stopwatch _timer;
        
        public Window(int width, int height, string title) :
            base(new GameWindowSettings(), new NativeWindowSettings() {
                Size = new Vector2i(width, height), 
                Title = title
            })
        { }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Set default clear colour
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // Make VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // Make VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            
            // Make EBO
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // Inform of format of data
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            
            // Please god someone save me from this awful path
            _shader = new Shader("../../../shaders/" + vertShader + ".vert", "../../../shaders/" + fragShader + ".frag");
            _shader.Use();

            _timer = new Stopwatch();
            _timer.Start();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _shader.Use();
            
            // Set uniforms
            // WARNING: If you get an error that this is not found, it's because GLSL is specifically checking if the
            // uniform is active (being used) and contributing to the final result! Otherwise, it is never added to the
            // dictionary or made accessible
            
            _shader.SetVector2("u_Resolution", Size);
            // _shader.SetFloat("u_Time", (float)_timer.Elapsed.TotalSeconds);

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}