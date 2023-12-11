using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KlipeioEngine
{
    //FIXME: Add so it instead is a general object class, so you can universally create whatever object(Sphere, hexagon, cube, cylinder, etc) and all of these properties will follow

    public class Cube : GameObject
    {
        public Mesh _mesh;

        #region cube data
        private readonly float[] _vertices = 
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

        private readonly uint[] _indices = //8 vertices list
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

        #region buffer objects

        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;

        #endregion
        
        private Shader _shader;

        public Color4 color
        {
            get; set;
        } 

        public Cube(Shader shader)
        {
            _shader = shader;

            _mesh = new Mesh(_vertices, _indices, shader);
            // Initialize buffers and shaders
            //InitializeBuffers();
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

        public void Draw(Matrix4 view, Matrix4 projection)
        {
            if(this.Enabled == true)
            {

                /*_shader.Use();

                int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");

                GL.Uniform4(vertexColorLocation, color);

                GL.BindVertexArray(_vertexArrayObject);*/
                
                Matrix4 model = Matrix4.CreateScale(Scale) 
                                    * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z)) 
                                    * Matrix4.CreateTranslation(Position);

                /*_shader.SetMatrix4("model", model);
                _shader.SetMatrix4("view", view);
                _shader.SetMatrix4("projection", projection);

                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

                GL.BindVertexArray(0);*/
                _mesh.Draw(model, view, projection);
            }
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            _shader.Dispose();
        }

    }
}