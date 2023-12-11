using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KlipeioEngine
{
    public class Mesh
    {
        private Shader _shader;
        private Color4 _color;

        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;

        private float[] _vertices;
        private uint[] _indices;

        public Color4 Color 
        { 
            get {return _color;}
            set { _color = value; }
        }

        public Mesh(float[] vertices, uint[] indicies, Shader shader)
        {
            this._vertices = vertices;
            this._indices = indicies;

            this._shader = shader;

            InitializeBuffers();
        }

        private void InitializeBuffers()
        {
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            int aPosLocation = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(aPosLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(aPosLocation);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void Draw(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            _shader.Use();

            int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");

            GL.Uniform4(vertexColorLocation, _color);

            GL.BindVertexArray(_vertexArrayObject);

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", view);
            _shader.SetMatrix4("projection", projection);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            _shader.Dispose();
        }
    }
}