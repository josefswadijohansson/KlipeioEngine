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
        private List<GameObject> listOfGameObjects = new List<GameObject>();

        private Cube _cube1;
        private Cube _cube2;
        private Sphere _sphere;

        private Shader _shader;
        private Camera camera;

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

            listOfGameObjects = new List<GameObject>();

            /*Cube newCube = new Cube(_shader);
            newCube._mesh.Color = Color4.Blue;

            listOfGameObjects.Add();*/

            _cube1 = new Cube(_shader); //TODO: Make so the shader is already accessible in the cube.
            _cube1.mesh.Color = Color4.Blue;
            _cube2 = new Cube(_shader);
            _cube2.mesh.Color = Color4.Red;
            _cube2.SetPosition(new Vector3(1,0,0));

            _sphere = new Sphere(_shader);
            _sphere.SetPosition(new Vector3(-1, 0, 0));
            
            GL.Enable(EnableCap.DepthTest);

            CursorState = CursorState.Grabbed;
        }
        
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.5f, 0.5f, 1.0f, 1.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            //Code goes here.

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            _cube1.Draw(view, projection);
            _cube2.Draw(view, projection);
            _sphere.Draw(view, projection);

            Context.SwapBuffers();

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

            if(keyboard.IsKeyPressed(Keys.F1))
            {
                _cube1.SetEnabled(false);
            }

            //_cube.Translate(new Vector3(0.0f, 0.0f, 0.00001f));
            //_cube.SetRotation(_cube.Rotation + new Vector3(0.0f, 0.0f, 0.01f));
            //_cube.SetScale(_cube.Scale + Vector3.One * 0.0001f);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _shader.Dispose();

            //TODO: Below needs to be made so i dispose all objects in the world.
            _cube1.Dispose();
            _cube2.Dispose();
            _sphere.Dispose();
        }
    }
}