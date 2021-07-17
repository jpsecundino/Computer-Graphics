using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace AllianceEngine
{
    public sealed class Shader : IDisposable
    {
        private readonly uint _handle;
        private readonly GL _gl;

        private readonly Dictionary<string, int> _uniforms = new Dictionary<string, int>();

        public Shader(string vertexPath, string fragmentPath)
        {
            _gl = Program.Gl;

            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
            _handle = _gl.CreateProgram();
            _gl.AttachShader(_handle, vertex);
            _gl.AttachShader(_handle, fragment);
            _gl.LinkProgram(_handle);
            _gl.GetProgram(_handle, GLEnum.LinkStatus, out int status);
            if (status == 0)
            {
                throw new ArgumentException($"Program failed to link with error: {_gl.GetProgramInfoLog(_handle)}");
            }
            _gl.DetachShader(_handle, vertex);
            _gl.DetachShader(_handle, fragment);
            _gl.DeleteShader(vertex);
            _gl.DeleteShader(fragment);
        }

        public void Use()
        {
            _gl.UseProgram(_handle);
        }

        public void SetUniform(string name, int value)
        {
            _gl.Uniform1(GetUniformLocation(name), value);
        }

        public void SetUniform(string name, float value)
        {
            _gl.Uniform1(GetUniformLocation(name), value);
        }
        public void SetUniform(string name, Vector4 value)
        {
            _gl.Uniform4(GetUniformLocation(name), value);
        }

        public void SetUniform(string name, System.Drawing.Color value)
        {
            _gl.Uniform4(GetUniformLocation(name), value.R / 255f, value.G / 255f, value.B / 255f, value.A / 255f);
        }

        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            _gl.UniformMatrix4(GetUniformLocation(name), 1, false, (float*)&value);
        }
        
        public unsafe void SetUniform(string name, Vector3 value)
        {
            _gl.Uniform3(GetUniformLocation(name),value.X, value.Y,value.Z);
        }


        public void Dispose()
        {
            _gl.DeleteProgram(_handle);
        }

        private uint LoadShader(ShaderType type, string path)
        {
            string src = File.ReadAllText(path);
            uint handle = _gl.CreateShader(type);
            _gl.ShaderSource(handle, src);
            _gl.CompileShader(handle);
            string infoLog = _gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new ArgumentException($"Error compiling shader of type {type}, failed with error {infoLog}");
            }

            return handle;
        }

        private int GetUniformLocation(string name)
        {
            if (!_uniforms.TryGetValue(name, out int location))
            {
                location = _gl.GetUniformLocation(_handle, name);
                if (location != -1)
                    _uniforms[name] = location;
            }

            if (location == -1)
            {
                throw new ArgumentException($"{name} uniform not found on shader.");
            }

            return location;
        }
    }
}
