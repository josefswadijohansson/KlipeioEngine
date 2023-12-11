using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KlipeioEngine
{
    //TODO: Cleanup the drawing so unessary faces is not drawn
    public class Mesh
    {
        private Shader _shader;
        private Color4 _color;

        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _vertexColorObject;
        private int _elementBufferObject;

        private float[] _vertices;
        private uint[] _indices;

        private float[] _colorData;

        public float[] Vertices
        {
            get { return _vertices;}
        }

        public uint[] Indices
        {
            get { return _indices; }
        }

        public float[] ColorData
        {
            get { return _colorData;}
        }

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

            List<float> colorData = new List<float>();

            for(int i = 0; i < vertices.Length; i++)
            {
                colorData.Add(Color4.White.R);
                colorData.Add(Color4.White.G);
                colorData.Add(Color4.White.B);
            }

            _colorData = colorData.ToArray();

            InitializeBuffers();
        }

        public Mesh(float[] vertices, uint[] indicies, Color4 color, Shader shader)
        {
            this._vertices = vertices;
            this._indices = indicies;

            this._shader = shader;

            _color = color;

            SetColor(color);

            InitializeBuffers();
        }

        public Mesh(float[] vertices, uint[] indicies, float[] colorData, Shader shader)
        {
            this._vertices = vertices;
            this._indices = indicies;

            this._shader = shader;
            this._colorData = colorData;

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

            _vertexColorObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexColorObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _colorData.Length * sizeof(float), _colorData, BufferUsageHint.StaticDraw);

            int aColorLocation = _shader.GetAttribLocation("aColor");
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void Draw(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            _shader.Use();

            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            //GL.Uniform4(vertexColorLocation, _color);

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
            GL.DeleteBuffer(_vertexColorObject);
            _shader.Dispose();

            _vertices = new float[0];
            _indices = new uint[0];
            _colorData = new float[0];

            _shader = null;
        }
    
        public static Mesh CombineMeshes(GameObject[] gameObjects, Shader shader)
        {
            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();
            List<float> colorData = new List<float>();

            int currentIndexOffset = 0;

            foreach(GameObject gameObject in gameObjects)
            {
                Vector3 position = gameObject.Position;
                Mesh gameObject_Mesh = gameObject.mesh;
                Color4 mesh_color = gameObject.mesh.Color;

                //Add code here
                float[] gameObjectVertices = gameObject_Mesh.Vertices;

                for(int i = 0; i < gameObjectVertices.Length; i++)
                {
                    vertices.Add(gameObjectVertices[i] + position.X);
                    colorData.Add(mesh_color.R);
                    i++;
                    vertices.Add(gameObjectVertices[i] + position.Y);
                    colorData.Add(mesh_color.G);
                    i++;
                    vertices.Add(gameObjectVertices[i] + position.Z);
                    colorData.Add(mesh_color.B);
                }

                uint[] gameObjectIndices = gameObject_Mesh.Indices;
                for (int i = 0; i < gameObjectIndices.Length; i++)
                {
                    indices.Add((uint)(gameObjectIndices[i] + currentIndexOffset));
                }

                currentIndexOffset += gameObjectVertices.Length / 3;
            }

            float[] combinedVertices = vertices.ToArray();
            uint[] combinedIndices = indices.ToArray();
            
            //return DeduplicateMesh(new Mesh(combinedVertices, combinedIndices, colorData.ToArray(), shader), shader);

            return new Mesh(combinedVertices, combinedIndices, colorData.ToArray(), shader);
        }

        /// <summary>
        /// This will assign the a color to the whole mesh
        /// Dont use this if you want to preserve the color of the a complicated mesh
        /// </summary>
        /// <param name="newColor"></param>
        public void SetColor(Color4 newColor)
        {
            List<float> colorData = new List<float>();

            for(int i = 0; i < _vertices.Length; i++)
            {
                colorData.Add(newColor.R);
                colorData.Add(newColor.G);
                colorData.Add(newColor.B);
            }

            _colorData = colorData.ToArray();
        }
        
        public static Mesh DeduplicateMesh(Mesh mesh, Shader shader)
        {
            Dictionary<Vector3, uint> uniqueVertices = new Dictionary<Vector3, uint>();

            List<float> deduplicatedVertices = new List<float>();
            List<uint> deduplicatedIndices = new List<uint>();
            List<float> deduplicatedColorData = new List<float>();

            int verticesCount = mesh.Vertices.Length;
            int indicesCount = mesh.Vertices.Length;

            float[] vertices = mesh.Vertices;
            uint[] indices = mesh.Indices;
            float[] colorData = mesh.ColorData;

            for(int i = 0; i < indices.Length; i++)
            {
                float vertexX = vertices[indices[i] * 3];
                float vertexY = vertices[indices[i] * 3 + 1];
                float vertexZ = vertices[indices[i] * 3 + 2];

                float colorR = colorData[indices[i] * 3];
                float colorG = colorData[indices[i] * 3 + 1];
                float colorB = colorData[indices[i] * 3 + 2];

                if (!uniqueVertices.TryGetValue(new Vector3(vertexX, vertexY, vertexZ), out uint newIndex))
                {
                    newIndex = (uint)(deduplicatedVertices.Count / 3);
                    uniqueVertices.Add(new Vector3(vertexX, vertexY, vertexZ), newIndex);

                    deduplicatedVertices.Add(vertexX);
                    deduplicatedVertices.Add(vertexY);
                    deduplicatedVertices.Add(vertexZ);

                    deduplicatedColorData.Add(colorR);
                    deduplicatedColorData.Add(colorG);
                    deduplicatedColorData.Add(colorB);
                }

                deduplicatedIndices.Add(newIndex);
            }

            return new Mesh(deduplicatedVertices.ToArray(), deduplicatedIndices.ToArray(), deduplicatedColorData.ToArray(), shader);
        }

        public void AddMeshData(float[] vertices, uint[] indices, float[] colorData, Vector3 position)
        {
            //TODO: Not yet implemented
        }
    
    }
}