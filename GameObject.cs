using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KlipeioEngine
{
    public class GameObject
    {
        private uint _id;

        private Shader _shader;
        private Mesh _mesh;

        private bool _isEnabled = true;

        private Vector3 _position = Vector3.Zero;
        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _scale = Vector3.One;

        private bool disposedValue;

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
        
        public Mesh mesh
        {
            get{ return _mesh;}
        }

        public uint ID
        {
            get { return _id; }
        }

        public bool Enabled 
        { 
            get { return _isEnabled;} 
        }

        public GameObject(Shader shader)
        {
            _shader = shader;
            this._id = Game.GetUniqueID();
        }

        public GameObject(Shader shader, Mesh mesh)
        {
            this._id = Game.GetUniqueID();
            _shader = shader;
            _mesh = mesh;
        }

        public GameObject(Shader shader, float[] vertices, uint[] indices)
        {
            this._id = Game.GetUniqueID();
            _shader = shader;
            _mesh = new Mesh(vertices, indices, shader);
        }

        public GameObject(Shader shader, float[] vertices, Color4 color, uint[] indices)
        {
            this._id = Game.GetUniqueID();
            _shader = shader;
            _mesh = new Mesh(vertices, indices, color, shader);
        }


        public virtual void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public void SetPosition(Vector3 newPosition)
        {
            _position = newPosition;
        }

        public void SetRotation(Vector3 newRotation)
        {
            _rotation = newRotation;
        }

        public void SetScale(Vector3 newScale)
        {
            _scale = newScale;
        }

        public void Translate(Vector3 translate)
        {
            _position += translate;
        }

        public void Draw(Matrix4 view, Matrix4 projection)
        {
            if(_isEnabled == true)
            {

                Matrix4 model = Matrix4.CreateScale(_scale) 
                                    * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_rotation.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotation.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation.Z)) 
                                    * Matrix4.CreateTranslation(_position);

                _mesh.Draw(model, view, projection);
            }
        }
        public void Dispose()
        {
            _mesh.Dispose();
            _mesh = null;
            _shader = null;
        }
    }
}