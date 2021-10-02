using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace shaders_lib.Shaders
{
    /// <summary>
    /// Represents an OpenGL Program, consisting of a number of Shaders
    /// </summary>
    public abstract class Shader
    {
        public readonly int Handle;
        
        private readonly Dictionary<string, int> _uniformLocations;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shader"/> class.
        /// </summary>
        /// <param name="vertPath">Path to the desired Vertex Shader</param>
        /// <param name="fragPath">Path to the desired Fragment Shader</param>
        protected Shader(string vertPath, string fragPath)
        {
            int vertexShader = LoadShader(vertPath, ShaderType.VertexShader);
            int fragmentShader = LoadShader(fragPath, ShaderType.FragmentShader);

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            LinkProgram(Handle);
            BindAttributes();

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            _uniformLocations = new Dictionary<string, int>();
            for (int i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                var location = GL.GetUniformLocation(Handle, key);
                _uniformLocations.Add(key, location);
            }
        }

        protected abstract void BindAttributes();

        protected void BindAttribute(int attribute, String variableName)
        {
            GL.BindAttribLocation(Handle, attribute, variableName);
        }

        /// <summary>
        /// Loads a shader from a specified path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int LoadShader(string path, ShaderType type)
        {
            string shaderSource = File.ReadAllText(path);
            var shader = GL.CreateShader(type);
            GL.ShaderSource(shader, shaderSource);
            CompileShader(shader);
            return shader;
        }

        /// <summary>
        /// Wraps GL.CompileShader() to include error checking and reporting
        /// </summary>
        /// <param name="shader">The shader to compile</param>
        /// <exception cref="Exception">ShaderInfoLog, in case of compile failure</exception>
        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infolog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred while compiling Shader({shader}).\n\n{infolog}");
            }
        }

        /// <summary>
        /// Wraps GL.LinkProgram() to include error checking and reporting
        /// </summary>
        /// <param name="program">The program to link</param>
        /// <exception cref="Exception">ProgramInfoLog, in case of link failure</exception>
        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                var infolog = GL.GetProgramInfoLog(program);
                throw new Exception($"Error occurred while compiling Program({program}).\n\n{infolog}");
            }
        }

        /// <summary>
        /// Wraps GL.UseProgram to enable easy object-oriented use and access
        /// </summary>
        public void Start()
        {
            GL.UseProgram(Handle);
        }

        /// <summary>
        /// Wraps GL.UseProgram with an input of 0, to unbind the current shader
        /// </summary>
        public void Stop()
        {
            GL.UseProgram(0);
        }

        /// <summary>
        /// Wraps GL.GetAttribLocation to enable easy object-oriented use and access
        /// </summary>
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        /// <summary>
        /// Set a uniform int on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        
        /// <summary>
        /// Set a uniform float on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        
        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }
        
        /// <summary>
        /// Set a uniform vector2 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetVector2(string name, Vector2 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform2(_uniformLocations[name], data);
        }
        
        /// <summary>
        /// Set a uniform vector3 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform vector4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to be set</param>
        public void SetVector4(string name, Vector4 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform4(_uniformLocations[name], data);
        }
    }
}