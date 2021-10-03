using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTK.Graphics.OpenGL;
using shaders_lib;
using shaders_lib.Shaders;
using shaders_lib.Textures;

namespace shaders_app
{
    public class Window : GameWindow
    {
        private readonly float[] _vertices = {
            -0.5f, 0.5f, 0f,
            -0.5f, -0.5f, 0f,
            0.5f, -0.5f, 0f,
            0.5f, 0.5f, 0f,
        };

        private readonly float[] _uv =
        {
            0, 0,
            0, 1,
            1, 1,
            1, 0,
        };

        private readonly int[] _indices =
        {
            0, 1, 3,
            3, 1, 2,
        };

        private Loader _loader;
        private Renderer _renderer;
        private RawModel _square;
        private Texture _texture;
        private TexturedModel _model;
        private StaticShader _shader;
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

            _loader = new Loader();
            _renderer = new Renderer(new Color4(0.1f, 0.2f, 0.2f, 1.0f));

            _square = _loader.LoadToVAO(_vertices, _uv,  _indices);
            _texture = _loader.LoadTexture("default.png");
            _model = new TexturedModel(_square, _texture);
            
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
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            _renderer.Prepare();
            _shader.Start();
            
            // Set uniforms
            // WARNING: If you get an error that this is not found, it's because GLSL is specifically checking if the
            // uniform is active (being used) and contributing to the final result! Otherwise, it is never added to the
            // dictionary or made accessible
            
            // _shader.SetVector2("u_Resolution", Size);
            // _shader.SetFloat("u_Time", (float)_timer.Elapsed.TotalSeconds);

            _renderer.Render(_model);
            _shader.Stop();
            
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