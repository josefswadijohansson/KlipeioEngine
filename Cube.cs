using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KlipeioEngine
{
    public class Cube
    {
        #region cube data
        private float[] _vertices = 
        {
            -0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f
        };

        private uint[] _indices = //8 vertices list
        {
            //front face
            //top triangle
            0, 1, 2,
            //bottom triangle
            2, 3, 0,

            //Right face
            1, 5, 6,
            6, 2, 1,

            //Back face
            5, 4, 7,
            7, 6, 5,

            //Left face
            4, 0, 3,
            3, 7, 4,

            //Bottom face
            3, 2, 7,
            7, 6, 2,

            //Top face
            4, 5, 1,
            1, 0, 4,
        };

        #endregion

        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;

        private Vector3 _position = Vector3.Zero;
        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _scale = Vector3.One;

        private Shader _shader;

        public Vector3 Position
        {
            get { return _position; }
        }

        public Vector3 Rotation
        {
            get { return _rotation; }
        }

        public Vector3 Scale
        {
            get { return _scale; }
        }

        public Color4 color
        {
            get; set;
        } 

        public Cube(Shader shader)
        {
            _shader = shader;

            // Initialize buffers and shaders
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

        private void InitializeShader()
        {
            _shader = new Shader("vertex.glsl", "fragment.glsl");
        }

        public void Draw(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            _shader.Use();

            int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");

            GL.Uniform4(vertexColorLocation, color);

            GL.BindVertexArray(_vertexArrayObject);
            
            Matrix4 modelMatrix = Matrix4.CreateScale(_scale) 
                                  * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_rotation.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotation.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation.Z)) 
                                  * Matrix4.CreateTranslation(_position);

            _shader.SetMatrix4("model", modelMatrix);
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

        public void SetPosition(Vector3 newPos)
        {
            _position = newPos;
        }
        
        public void Translate(Vector3 translate)
        {
            _position += translate;
        }

        public void SetRotation(Vector3 newRotation)
        {
            _rotation = newRotation;
        }

        public void SetScale(Vector3 newScale)
        {
            _scale = newScale;
        }
    
    }
}