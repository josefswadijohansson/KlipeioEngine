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

            for(int x = 0; x < 100; x++)
            {
                for(int y = 0; y < 100; y++)
                {
                    Color4 theColor = Color4.Black;
                
                    if(index % 2 == 0)
                    {
                        theColor = Color4.Blue;
                    }
                    else 
                    {
                        theColor = Color4.Red;
                    }

                    GameObject newCube = new GameObject(_shader, Cube._vertices, theColor, Cube._indices);
                    newCube.SetPosition(new Vector3(x, 0, y));

                    temp.Add(newCube);
                    index++;
                }
            }

            /*for(int i = 0; i < 10000; i++)
            {
                Color4 theColor = Color4.Black;
                
                if(i % 2 == 0)
                {
                    theColor = Color4.Blue;
                }
                else 
                {
                    theColor = Color4.Red;
                }

                GameObject newCube = new GameObject(_shader, Cube._vertices, theColor, Cube._indices);
                newCube.SetPosition(new Vector3(i, 0, 0));

                temp.Add(newCube);
            }*/

            //_listOfGameObjects.AddRange(temp);
            GameObject combinedGameObject = new GameObject(_shader, Mesh.CombineMeshes(temp.ToArray(), _shader));
            combinedGameObject.mesh.Color = Color.Blue;
            _listOfGameObjects.Add(combinedGameObject);

            /*for(int i = 0; i < 10; i++)
            {
                Sphere.GenerateSphere(1.0f, 8, out float[] vertices, out uint[] indices);
                GameObject newSphere = new GameObject(_shader, vertices, indices);
                newSphere.SetPosition(new Vector3(i, 0, 2));

                if(i % 2 == 0)
                {
                    newSphere.mesh.Color = Color4.Blue;

                    _listOfGameObjects.Add(newSphere);
                }
                else 
                {
                    newSphere.mesh.Color = Color4.Red;

                    _listOfGameObjects.Add(newSphere);
                }
            }*/
            
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

            foreach(GameObject gameObject in _listOfGameObjects)
            {
                gameObject.Draw(view, projection);
            }

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
    }
}