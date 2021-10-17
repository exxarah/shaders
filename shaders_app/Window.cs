using System;
using System.Diagnostics;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTK.Graphics.OpenGL4;
using shaders_lib;
using shaders_lib.Entities;
using shaders_lib.Models;
using shaders_lib.Shaders;
using shaders_lib.Util;

namespace shaders_app
{
    public class Window : GameWindow
    {
        private Loader _loader;
        private Renderer _renderer;
        private ImGuiController _controller;

        private Camera _camera;
        private Light _light;
        
        private RawModel _cube;
        private Texture _texture;
        private Material _material;
        private TexturedModel _model;
        private Entity _entity;
        
        private StaticShader _shader;
        private Stopwatch _timer;

        private const float mouseAcceleration = 10f;

        public Window(int width, int height, string title, int fps) :
            base(new GameWindowSettings()
            {
                UpdateFrequency = fps
            }, new NativeWindowSettings()
            {
                Size = new Vector2i(width, height),
                Title = title,
                APIVersion = new Version(4, 5)
            })
        { }

        protected override void OnLoad()
        {
            base.OnLoad();

            _loader = new Loader();
            _renderer = new Renderer(new Color4(0.1f, 0.2f, 0.2f, 1.0f));
            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);

            _camera = new Camera(new Vector3(0, 0, 3), ClientSize);
            _light = new Light(new Vector3(0, 0, 10), Vector3.One);
            
            _cube = _loader.LoadModel("cube.obj");
            _texture = new Texture("diffuse", ImgLoader.LoadImage("white.png"));
            _material = new PhongMaterial(_texture);
            _model = new TexturedModel(_cube, _material);
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

            if (MouseState.IsButtonDown(MouseButton.Middle))
            {
                Vector2 inputVector = (MouseState.Position - MouseState.PreviousPosition);
                _entity.Rotate(inputVector.Y, inputVector.X, 0f);
            }
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            
            _controller.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            
            _controller.MouseScroll(e.Offset);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            _controller.Update(this, (float)args.Time);
            
            _renderer.Prepare();
            _renderer.Render(_entity, _shader, _camera, _light);

            ImGui.ShowDemoWindow();
            _controller.Render();
            
            GlUtil.CheckGlError("End of frame");

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            _controller.WindowResized(ClientSize.X, ClientSize.Y);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _loader.CleanUp();
        }
    }
}