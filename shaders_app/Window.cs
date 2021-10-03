using System;
using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using OpenTK.Graphics.OpenGL;
using shaders_lib;
using shaders_lib.Entities;
using shaders_lib.Shaders;
using shaders_lib.Textures;

namespace shaders_app
{
    public class Window : GameWindow
    {
        float[] _vertices = {			
            -0.5f,0.5f,-0.5f,	
            -0.5f,-0.5f,-0.5f,	
            0.5f,-0.5f,-0.5f,	
            0.5f,0.5f,-0.5f,		
				
            -0.5f,0.5f,0.5f,	
            -0.5f,-0.5f,0.5f,	
            0.5f,-0.5f,0.5f,	
            0.5f,0.5f,0.5f,
				
            0.5f,0.5f,-0.5f,	
            0.5f,-0.5f,-0.5f,	
            0.5f,-0.5f,0.5f,	
            0.5f,0.5f,0.5f,
				
            -0.5f,0.5f,-0.5f,	
            -0.5f,-0.5f,-0.5f,	
            -0.5f,-0.5f,0.5f,	
            -0.5f,0.5f,0.5f,
				
            -0.5f,0.5f,0.5f,
            -0.5f,0.5f,-0.5f,
            0.5f,0.5f,-0.5f,
            0.5f,0.5f,0.5f,
				
            -0.5f,-0.5f,0.5f,
            -0.5f,-0.5f,-0.5f,
            0.5f,-0.5f,-0.5f,
            0.5f,-0.5f,0.5f
				
        };
		
        float[] _uvs = {
				
            0,0,
            0,1,
            1,1,
            1,0,			
            0,0,
            0,1,
            1,1,
            1,0,			
            0,0,
            0,1,
            1,1,
            1,0,
            0,0,
            0,1,
            1,1,
            1,0,
            0,0,
            0,1,
            1,1,
            1,0,
            0,0,
            0,1,
            1,1,
            1,0

				
        };
		
        int[] _indices = {
            0,1,3,	
            3,1,2,	
            4,5,7,
            7,5,6,
            8,9,11,
            11,9,10,
            12,13,15,
            15,13,14,	
            16,17,19,
            19,17,18,
            20,21,23,
            23,21,22

        };

        private Loader _loader;
        private Renderer _renderer;

        private Camera _camera;
        
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
            _renderer = new Renderer(new Color4(0.1f, 0.2f, 0.2f, 1.0f));

            _camera = new Camera(new Vector3(0, 0, 3), Size);

            _square = _loader.LoadToVAO(_vertices, _uvs,  _indices);
            _texture = _loader.LoadTexture("default.png");
            _model = new TexturedModel(_square, _texture);
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
            _renderer.Render(_entity, _shader, _camera);

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