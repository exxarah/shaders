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
            _renderer = new Renderer(new Color4(0.1f, 0.2f, 0.2f, 1.0f), Size);

            _square = _loader.LoadToVAO(_vertices, _uv,  _indices);
            _texture = _loader.LoadTexture("default.png");
            _model = new TexturedModel(_square, _texture);
            _entity = new Entity(_model, new Vector3(0, 0, -1));
            
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

            _entity.Move(0, 0, (float)(-1 * e.Time));
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            _renderer.Prepare();
            _renderer.Render(_entity, _shader);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            GL.Viewport(0, 0, Size.X, Size.Y);
            _renderer.Resize(Size);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _loader.CleanUp();
        }
    }
}