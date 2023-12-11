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
        private bool _isEnabled = true;

        private Vector3 _position = Vector3.Zero;
        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _scale = Vector3.One;

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

        public bool Enabled 
        { 
            get { return _isEnabled;} 
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

    }
}