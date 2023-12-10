using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KlipeioEngine
{
    public class Shader : IDisposable
    {
        private int _handle;

        public int Handle { get {return _handle;} }

        private bool disposedValue = false;

        public Shader(string vertexFileName, string fragmentFileName)
        {
            string vertexFilePath =     $"../../../GLSL/{vertexFileName}";
            string fragmentFilePath =   $"../../../GLSL/{fragmentFileName}";

            string vertexShaderSource = File.ReadAllText(vertexFilePath);
            string fragmentShaderSource = File.ReadAllText(fragmentFilePath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            GL.CompileShader(vertexShader);

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertex_success);
            if(vertex_success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(fragmentShader);

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragment_success);
            if(fragment_success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }

            _handle = GL.CreateProgram();

            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);

            GL.LinkProgram(_handle);

            GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int link_success);
            if(link_success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(_handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }

        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(_handle, attribName);
        }

        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        /// <remarks>
        ///   <para>
        ///   The matrix is transposed before being sent to the shader.
        ///   </para>
        /// </remarks>
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            int uniformLocation = GL.GetUniformLocation(_handle, name);
            GL.UniformMatrix4(uniformLocation, true, ref data);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(_handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}