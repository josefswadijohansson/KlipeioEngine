using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace KlipeioEngine
{
    public class Camera
    {
        private float _speed = 8f;
        private float _sensitivity = 60f;
        private float _pitch;
        private float _yaw = -90.0f;

        private bool _firstMove = true;
        public Vector2 _lastPos;

        private Vector3 _position = Vector3.Zero; //FIXME : Make a parent class that contains position, rotation and scale, that this camera class derives from
        public Vector3 Position
        {
            get { return _position; }
        }   

        private Vector3 _right = Vector3.UnitX;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _front = -Vector3.UnitZ;

        public Camera(Vector3 position)
        {
            this._position = position;
        }

        public Matrix4 GetViewMatrix()
        {
            //Vector3 position = new Vector3(0.0f, 0.0f,  3.0f);
            //Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
            //Vector3 up = new Vector3(0.0f, 1.0f,  0.0f);

            return Matrix4.LookAt(_position, _position + _front, _up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Game.WindowWidth / Game.WindowHeight, 0.1f, 100.0f);
        }

        public void SetPosition(Vector3 newPos)
        {
            _position = newPos;
        }

        public void Translate(Vector3 translate)
        {
            _position += translate;  //FIXME: I dont know maybe this is not correct way to translate
        }

        private void UpdateVectors()
        {
            if(_pitch > 89.0f)
            {
                _pitch = 89.0f;
            }

            if (_pitch < -89.0f)
            {
                _pitch = -89.0f;
            }

            _front.X = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
            _front.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            _front.Z = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));

            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }
        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            if(input.IsKeyDown(Keys.W))
            {
                _position += _front * _speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                _position -= _right * _speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                _position -= _front * _speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                _position += _right * _speed * (float)e.Time;
            }

            if (input.IsKeyDown(Keys.Space))
            {
                _position.Y += _speed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _position.Y -= _speed * (float)e.Time;
            }

            if(_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;

                _lastPos = new Vector2(mouse.X, mouse.Y);

                _yaw += deltaX * _sensitivity * (float)e.Time;
                _pitch -= deltaY * _sensitivity * (float)e.Time;

                UpdateVectors();
            }
        }
        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            InputController(input, mouse, e);
        }

    }
}