using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KlipeioEngine
{
    //FIXME: Add so it instead is a general object class, so you can universally create whatever object(Sphere, hexagon, cube, cylinder, etc) and all of these properties will follow

    public class Cube
    {
        private Mesh _mesh;

        #region cube data
        public static readonly float[] _vertices = 
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

        public static readonly uint[] _indices = //8 vertices list
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

        public Mesh mesh 
        { 
            get {return _mesh;}
        }

        public Cube(Shader shader)
        {
            //_shader = shader;

            //_mesh = new Mesh(_vertices, _indices, shader);
        }
    }
}