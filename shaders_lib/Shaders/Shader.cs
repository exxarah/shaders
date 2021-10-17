using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace shaders_lib.Shaders
{
    public struct UniformFieldInfo
    {
        public int Location;
        public string Name;
        public int Size;
        public ActiveUniformType Type;
    }
    
    /// <summary>
    /// Represents an OpenGL Program, consisting of a number of Shaders
    /// </summary>
    public class Shader : IDisposable
    {
        public readonly string Name;
        public int Program { get; private set; }
        
        private readonly Dictionary<string, int> _uniformLocations = new Dictionary<string, int>();
        private bool _initialised = false;
        private readonly (ShaderType Type, string Path)[] _files;

        public Shader(string name, string vertexShader, string fragmentShader)
        {
            Name = name;
            _files = new[]
            {
                (ShaderType.VertexShader, vertexShader),
                (ShaderType.FragmentShader, fragmentShader),
            };
            Program = CreateProgram(name, _files);
        }

        /// <summary>
        /// Inherited from <see cref="IDisposable"/>, cleans up OpenGL resources.
        /// </summary>
        public void Dispose()
        {
            if (_initialised)
            {
                GL.DeleteProgram(Program);
                _initialised = false;
            }
        }

        /// <summary>
        /// Wraps GL.UseProgram to enable easy object-oriented use and access
        /// </summary>
        public void Start()
        {
            GL.UseProgram(Program);
        }

        /// <summary>
        /// Wraps GL.UseProgram with an input of 0, to unbind the current shader
        /// </summary>
        public void Stop()
        {
            GL.UseProgram(0);
        }

        /// <summary>
        /// Fetch information on all related Uniforms for this shader
        /// </summary>
        /// <returns>An array of <see cref="UniformFieldInfo"/> objects, containing information on all active uniforms</returns>
        public UniformFieldInfo[] GetUniforms()
        {
            GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out int uniformCount);

            UniformFieldInfo[] uniforms = new UniformFieldInfo[uniformCount];

            for (int i = 0; i < uniformCount; i++)
            {
                string name = GL.GetActiveUniform(Program, i, out int size, out ActiveUniformType type);
                UniformFieldInfo fieldInfo;
                fieldInfo.Location = GetUniformLocation(name);
                fieldInfo.Name = name;
                fieldInfo.Size = size;
                fieldInfo.Type = type;

                uniforms[i] = fieldInfo;
            }

            return uniforms;
        }

        /// <summary>
        /// Get the location of a uniform on this shader, will try to access it through the dictionary if possible,
        /// otherwise it'll find it and then save it for access at a later time
        /// </summary>
        /// <param name="uniform">The name of the uniform to be found</param>
        /// <returns>The location of the given uniform</returns>
        public int GetUniformLocation(string uniform)
        {
            if (_uniformLocations.TryGetValue(uniform, out int location) == false)
            {
                location = GL.GetUniformLocation(Program, uniform);
                _uniformLocations.Add(uniform, location);

                if (location == -1)
                {
                    Console.WriteLine($"The uniform '{uniform}' does not exist in the shader '{Name}'!");
                }
            }

            return location;
        }

        /// <summary>
        /// Creates a program, and returns the location for later access
        /// </summary>
        /// <param name="name">The name to be given to the shader</param>
        /// <param name="shaderPaths">The various glsl files to be used</param>
        /// <returns>The location of the new program</returns>
        private int CreateProgram(string name, params (ShaderType type, string path)[] shaderPaths)
        {
            Util.GlUtil.CreateProgram(name, out int program);

            int[] shaders = new int[shaderPaths.Length];
            for (int i = 0; i < shaderPaths.Length; i++)
            {
                shaders[i] = CompileShader(name, shaderPaths[i].type, shaderPaths[i].path);
            }

            foreach (var shader in shaders)
            {
                GL.AttachShader(program, shader);
            }
            
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string info = GL.GetProgramInfoLog(program);
                Console.WriteLine($"GL.LinkProgram had info log [{name}]:\n{info}");
            }

            foreach (var shader in shaders)
            {
                GL.DetachShader(program, shader);
                GL.DeleteShader(shader);
            }

            _initialised = true;
            
            return program;
        }

        /// <summary>
        /// Load a shader from a file, and compile it for use
        /// </summary>
        /// <param name="name">The name of the shader</param>
        /// <param name="type">The type, eg vertex/fragment</param>
        /// <param name="path">The path of the shader file</param>
        /// <returns>The location of the new shader</returns>
        private int CompileShader(string name, ShaderType type, string path)
        {
            Util.GlUtil.CreateShader(type, name, out int shader);
            string source = File.ReadAllText(path);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);
            
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string info = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"GL.CompileShader for shader '{name}' [{type}] had info log:\n{info}");
            }

            return shader;
        }
        
        /// <summary>
        /// Set a uniform int on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetInt(string name, int data)
        {
            GL.Uniform1(GetUniformLocation(name), data);
        }
        
        /// <summary>
        /// Set a uniform float on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetFloat(string name, float data)
        {
            GL.Uniform1(GetUniformLocation(name), data);
        }

        /// <summary>
        /// Set a uniform float on this shader, in response to a boolean
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetBoolean(string name, bool data)
        {
            if (data)
            {
                GL.Uniform1(GetUniformLocation(name), 1.0f);
            }
            else
            {
                GL.Uniform1(GetUniformLocation(name), 0.0f);
            }
        }

        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        /// <param name="transpose">Whether or not to transpose the matrix</param>
        public void SetMatrix4(string name, Matrix4 data, bool transpose=true)
        {
            GL.UniformMatrix4(GetUniformLocation(name), transpose, ref data);
        }
        
        /// <summary>
        /// Set a uniform vector2 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetVector2(string name, Vector2 data)
        {
            GL.Uniform2(GetUniformLocation(name), data);
        }
        
        /// <summary>
        /// Set a uniform vector3 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetVector3(string name, Vector3 data)
        {
            GL.Uniform3(GetUniformLocation(name), data);
        }

        /// <summary>
        /// Set a uniform vector4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetVector4(string name, Vector4 data)
        {
            GL.Uniform4(GetUniformLocation(name), data);
        }
    }
}