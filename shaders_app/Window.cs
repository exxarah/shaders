using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ImGuiNET;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using shaders_lib;
using shaders_lib.Entities;
using shaders_lib.Models;
using shaders_lib.Shaders;
using shaders_lib.Util;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;

namespace shaders_app
{
    public class Window : GameWindow
    {
        private Loader _loader;
        private Renderer _renderer;
        private ImGuiController _controller;

        private Camera _camera;
        private Light _light;
        
        private TexturedModel _model;
        private Entity _entity;
        
        private StaticShader _shader;
        private Stopwatch _timer;

        private int comboFileChoice = 0;
        private string[] comboFiles = new[] { "suzanne.obj", "sphere.obj", "cube.obj" };

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
            
            RawModel mesh = _loader.LoadModel("suzanne.obj");
            Texture texture = new Texture("diffuse", ImgLoader.LoadImage("white.png"));
            Material material = new PhongMaterial(texture);
            _model = new TexturedModel(mesh, material);
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
                OpenTK.Mathematics.Vector2 inputVector = (MouseState.Position - MouseState.PreviousPosition);
                _entity.Rotate(inputVector.Y, inputVector.X, 0f);
            }
            
            _entity.Rotate(0.5f, 0.5f, 0.5f);
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

            ImGui.Begin("Phong Material");
            ImGui.ColorEdit3("Diffuse Factor", ref ((PhongMaterial)_model.Material).DiffuseFactor);
            ImGui.ColorEdit3("Specular Factor", ref ((PhongMaterial)_model.Material).SpecularFactor);
            ImGui.SliderFloat("Shininess Factor", ref ((PhongMaterial)_model.Material).ShininessFactor, 10f, 50f);
            ImGui.End();

            ImGui.Begin("Light");
            ImGui.ColorEdit3("Colour", ref _light.Color);
            ImGui.SliderFloat("Brightness", ref _light.Brightness, 0f, 3f);
            ImGui.SliderFloat("Ambient Strength", ref _light.AmbientStrength, 0f, 1f);
            ImGui.SliderFloat("Diffuse Strength", ref _light.DiffuseStrength, 0f, 1f);
            ImGui.SliderFloat("Specular Strength", ref _light.SpecularStrength, 0f, 1f);
            ImGui.End();
            
            ImGui.Begin("Model");
            ImGui.Text(_model.Model.Name);
            bool newMesh = ImGui.Combo("Mesh Choice", ref comboFileChoice, comboFiles, comboFiles.Length);
            if (newMesh)
            {
                RawModel mesh = _loader.LoadModel(comboFiles[comboFileChoice]);
                _model.Model = mesh;
            }
            ImGui.Text($"Vertex Count: {_model.Model.VertexCount}");
            ImGui.End();
            
            ImGui.ShowAboutWindow();
            
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