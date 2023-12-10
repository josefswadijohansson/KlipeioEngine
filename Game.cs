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
        Shader _shader;

        #region buffer objects

        int _vertexBufferObject;
        int _vertexArrayObject;
        int _elementBufferObject; 

        #endregion

        #region  rectangle data

        float[] _vertices = {
            0.5f,  0.5f, 0.0f,  // top right    - 0
            0.5f, -0.5f, 0.0f,  // bottom right - 1
            -0.5f, -0.5f, 0.0f,  // bottom left - 2
            -0.5f,  0.5f, 0.0f   // top left    - 3
        };

        uint[] _indices = 
        {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        #endregion

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
            Vector4 vec = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            Matrix4 trans = Matrix4.CreateTranslation(1f, 1f, 0.0f);
            vec *= trans;
            Console.WriteLine("{0}, {1}, {2}", vec.X, vec.Y, vec.Z);

            base.OnLoad();

            //Created the shader program
            _shader = new Shader("vertex.glsl", "fragment.glsl");

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
            _shader.Use();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.5f, 0.5f, 1.0f, 1.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            //Code goes here.
            _shader.Use();

            int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");

            GL.Uniform4(vertexColorLocation, 1.0f, 0.0f, 0.0f, 1.0f);

            GL.BindVertexArray(_vertexArrayObject);

            Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), _windowWidth / _windowHeight, 0.1f, 100.0f);

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", view);
            _shader.SetMatrix4("projection", projection);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();

            GL.BindVertexArray(0);

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            MouseState mouse = MouseState;
            KeyboardState keyboard = KeyboardState;

            if(keyboard.IsKeyPressed(Keys.Escape))
            {
                Close();
            }

            // Update any game logic here
            // ...
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GL.DeleteBuffer(_vertexBufferObject);
            _shader.Dispose();
        }
    }
}