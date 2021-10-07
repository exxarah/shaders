using System;
using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTK.Graphics.OpenGL;
using shaders_lib;
using shaders_lib.Entities;
using shaders_lib.Models;
using shaders_lib.Shaders;
using shaders_lib.Textures;

namespace shaders_app
{
    public class Window : GameWindow
    {
        private Loader _loader;
        private Renderer _renderer;

        private Camera _camera;
        private Light _light;
        
        private RawModel _cube;
        private Texture _texture;
        private TexturedModel _model;
        private Entity _entity;
        
        private StaticShader _shader;
        private Stopwatch _timer;

        public Window(int width, int height, string title, int fps) :
            base(new GameWindowSettings()
            {
                UpdateFrequency = fps
            }, new NativeWindowSettings()
            {
                Size = new Vector2i(width, height),
                Title = title
            })
        { }

        protected override void OnLoad()
        {
            base.OnLoad();

            _loader = new Loader();
            _renderer = new Renderer(new Color4(0.1f, 0.2f, 0.2f, 1.0f));

            _camera = new Camera(new Vector3(0, 0, 3), Size);
            _light = new Light(new Vector3(0, 0, 10), Vector3.One);
            
            _cube = _loader.LoadModel("suzanne.obj");
            _texture = _loader.LoadTexture("white.png");
            _model = new TexturedModel(_cube, _texture);
            _entity = new Entity(_model, new Vector3(0, 0, 0));
            
            _shader = new StaticShader();
            _shader.Start();

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

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _camera.Move(0, 0, -0.02f);
            }

            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _camera.Move(0, 0, 0.02f);
            }

            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _camera.Move(-0.02f, 0, 0);
            }

            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _camera.Move(0.02f, 0, 0);
            }

            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                _camera.Move(0, -0.02f, 0);
            }

            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                _camera.Move(0, 0.02f, 0);
            }
            
            _entity.Rotate(1, 1, 0);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            _renderer.Prepare();
            _shader.LoadCamera(_camera);
            _shader.LoadLight(_light);
            _renderer.Render(_entity, _shader);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _loader.CleanUp();
        }
    }
}