using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KlipeioEngine
{
    public class Game : GameWindow
    {
        private Cube _cube1;
        private Cube _cube2;

        private Shader _shader;
        private Camera camera;

        #region buffer objects

        int _vertexBufferObject;
        int _vertexArrayObject;
        int _elementBufferObject; 

        #endregion

        /*#region cube data
        
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

        #endregion*/

        private static int _windowWidth;
        private static int _windowHeight;

        public static int WindowWidth 
        { 
            get { return _windowWidth; } 
        }
        public static int WindowHeight 
        { 
            get { return _windowHeight; } 
        }

        public Game(string title, int width, int height) : base(GameWindowSettings.Default, new NativeWindowSettings(){ ClientSize = (width, height), Title = title})
        {
            _windowWidth = width;
            _windowHeight = height;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            _windowWidth = e.Width;
            _windowHeight = e.Height;

            GL.Viewport(0,0, _windowWidth, _windowHeight);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            camera = new Camera(new Vector3(0.0f, 0.0f, 3.0f));
            _shader = new Shader("vertex.glsl", "fragment.glsl");

            _cube1 = new Cube(_shader); //TODO: Make so the shader is already accessible in the cube.
            _cube1.color = Color4.Blue;
            _cube2 = new Cube(_shader);
            _cube2.color = Color4.Red;
            _cube2.SetPosition(new Vector3(1,0,0));
            
            //Created the shader program
            /*

            //Generate a vertex array object
            _vertexArrayObject = GL.GenVertexArray();   

            //Generate a vertex buffer object
            _vertexBufferObject = GL.GenBuffer();

            //Generate the element array buffer object
            _elementBufferObject = GL.GenBuffer();

            //Bind the vertex array object
            GL.BindVertexArray(_vertexArrayObject);     

            //Bind the vertex buffer object
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            //Pass the vertices data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            //Retrieve the aPosition location index, that will later be passed to the vertex array object
            int aPosLocation = _shader.GetAttribLocation("aPosition");
            //Since i have now bound the vertices data, send the buffer data to the vertex array on location/index 0
            GL.VertexAttribPointer(aPosLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //then enable location 0
            GL.EnableVertexAttribArray(aPosLocation);

            //Bind the element array buffer object
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

            //Pass data to the bound element array buffer object
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length*sizeof(uint), _indices, BufferUsageHint.StaticDraw);  

            //unbind the vertex buffer object
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //unbind the element array object
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //use the shader program to draw out the relevant data
            _shader.Use();*/

            GL.Enable(EnableCap.DepthTest);

            CursorState = CursorState.Grabbed;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.5f, 0.5f, 1.0f, 1.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            //Code goes here.
            _shader.Use();

            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");

            //GL.Uniform4(vertexColorLocation, 1.0f, 0.0f, 0.0f, 1.0f);

            //GL.BindVertexArray(_vertexArrayObject);

            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            //_shader.SetMatrix4("model", model);
            //_shader.SetMatrix4("view", view);
            //_shader.SetMatrix4("projection", projection);

            //GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            _cube1.Draw(model, view, projection);
            _cube2.Draw(model, view, projection);

            Context.SwapBuffers();

            GL.BindVertexArray(0);

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            MouseState mouse = MouseState;
            KeyboardState keyboard = KeyboardState;

            camera.Update(keyboard, mouse, args);

            if(keyboard.IsKeyPressed(Keys.Escape))
            {
                Close();
            }

            //_cube.Translate(new Vector3(0.0f, 0.0f, 0.00001f));
            //_cube.SetRotation(_cube.Rotation + new Vector3(0.0f, 0.0f, 0.01f));
            //_cube.SetScale(_cube.Scale + Vector3.One * 0.0001f);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GL.DeleteBuffer(_vertexBufferObject);
            _shader.Dispose();
            _cube1.Dispose();
        }
    }
}