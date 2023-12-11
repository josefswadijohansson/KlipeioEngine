using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Drawing;

namespace KlipeioEngine
{
    public class Game : GameWindow
    {
        private static List<GameObject> _listOfGameObjects = new List<GameObject>();

        private double _totalTime = 0.0;
        private int _frameCount = 0;

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

            _listOfGameObjects = new List<GameObject>();

            List<GameObject> temp = new List<GameObject>();

            int index = 0;

            for(int x = 0; x < 200; x++)
            {
                for(int y = 0; y < 200; y++)
                {
                    Color4 theColor = Color4.Black;
                
                    if((x / 1 + y / 1) % 2 == 0)
                    {
                        theColor = Color4.Blue;
                    }
                    else 
                    {
                        theColor = Color4.Red;
                    }

                    GameObject newCube = new GameObject(_shader, Cube._vertices, theColor, Cube._indices);  //FIXME: IDK This uses a lot of data, maybe instead just make all vertices, indices and color data here.
                    newCube.SetPosition(new Vector3(x, 0, y));

                    temp.Add(newCube);
                    index++;
                }
            }

            GameObject combinedGameObject = new GameObject(_shader, Mesh.CombineMeshes(temp.ToArray(), _shader));   
            combinedGameObject.mesh.Color = Color.Blue;
            _listOfGameObjects.Add(combinedGameObject);

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


            for(int i = 0; i < _listOfGameObjects.Count; i++)
            {
                _listOfGameObjects[i].Draw(view, projection);
            }
            /*foreach(GameObject gameObject in _listOfGameObjects)
            {
                gameObject.Draw(view, projection);
            }*/

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
                GameObject g = GetGameobjectByID(12);
                if(g.mesh.Color == Color4.Blue)
                {
                    Console.WriteLine("Color is blue");
                    //g.SetEnabled(false);
                }
                else 
                {
                    Console.WriteLine("Color is not blue");
                }
                g.Translate(new Vector3(0,-1,0));
                //DestroyObject(g);
            }

            // FPS calculation

            double deltaTime = args.Time;

            // Update total time and frame count
            _totalTime += deltaTime;
            _frameCount++;

            // Update every second (1.0 second interval)
            if (_totalTime >= 1.0)
            {
                // Calculate FPS
                double fps = _frameCount / _totalTime;

                // Display FPS or use it as needed
                UpdateWindowTitle($"Klipeio Engine | FPS: {fps:F1} | Memory: {GC.GetTotalMemory(true) / (1024.0 * 1024.0):F2} MB");

                // Reset counters for the next second
                _totalTime = 0.0;
                _frameCount = 0;
            }

            //double fps = 1.0 / args.Time;

            // Update window title with FPS
            //UpdateWindowTitle($"Klipeio Engine | FPS: {fps:F1}");


            //_cube.Translate(new Vector3(0.0f, 0.0f, 0.00001f));
            //_cube.SetRotation(_cube.Rotation + new Vector3(0.0f, 0.0f, 0.01f));
            //_cube.SetScale(_cube.Scale + Vector3.One * 0.0001f);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _shader.Dispose();

            //TODO: Below needs to be made so i dispose all objects in the world.
            foreach(GameObject gameObject in _listOfGameObjects)
            {
                gameObject.Dispose();
                
            }

            _listOfGameObjects.Clear();
        }
    
        public static GameObject GetGameobjectByID(uint id)
        {
            foreach(GameObject gameObject in _listOfGameObjects)
            {
                if(gameObject.ID == id)
                {
                    return gameObject;
                }
            }

            return null;
        }

        public static uint GetUniqueID()
        {
            return (uint)_listOfGameObjects.Count;
        }
    
        public static  void DestroyObject(GameObject gameObject)
        {
            _listOfGameObjects.RemoveAt((int)gameObject.ID);
            gameObject.Dispose();   //FIXME: Maybe this isn't effienct, find a better way, maybe just dont showcase it
        }

        private void UpdateWindowTitle(string title)
        {
            // Update the window title
            Title = title;
        }
    
        private void InitializeObject(GameObject gameObject)
        {
            _listOfGameObjects.Add(gameObject);
        }
    }
}