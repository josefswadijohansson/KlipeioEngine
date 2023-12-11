using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KlipeioEngine
{
    public class Sphere
    {
        private Mesh _mesh;

        private float[] _vertices;
        private uint[] _indices;

        public Sphere(Shader shader)
        {
            GenerateSphere(1.0f, 8, out _vertices, out _indices);

            _mesh = new Mesh(_vertices, _indices, shader);
        }

        /*public void Draw(Matrix4 view, Matrix4 projection)
        {
            if(this.Enabled == true)
            {

                Matrix4 model = Matrix4.CreateScale(Scale) 
                                    * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z)) 
                                    * Matrix4.CreateTranslation(Position);

                _mesh.Draw(model, view, projection);
            }
        }*/

        public static void GenerateSphere(float radius, int segments, out float[] vertices, out uint[] indices)
        {
            vertices = new float[(segments + 1) * (segments + 1) * 3];
            indices = new uint[segments * segments * 6];

            int vertexIndex = 0;
            int indexIndex = 0;

            for (int i = 0; i <= segments; i++)
            {
                float phi = (float)i / segments * (float)Math.PI;
                for (int j = 0; j <= segments; j++)
                {
                    float theta = (float)j / segments * 2 * (float)Math.PI;

                    float x = radius * (float)Math.Sin(phi) * (float)Math.Cos(theta);
                    float y = radius * (float)Math.Cos(phi);
                    float z = radius * (float)Math.Sin(phi) * (float)Math.Sin(theta);

                    vertices[vertexIndex++] = x;
                    vertices[vertexIndex++] = y;
                    vertices[vertexIndex++] = z;

                    if (i < segments && j < segments)
                    {
                        int currentRow = i * (segments + 1);
                        int nextRow = (i + 1) * (segments + 1);

                        indices[indexIndex++] = (uint)(currentRow + j);
                        indices[indexIndex++] = (uint)(nextRow + j);
                        indices[indexIndex++] = (uint)(nextRow + j + 1);

                        indices[indexIndex++] = (uint)(currentRow + j);
                        indices[indexIndex++] = (uint)(nextRow + j + 1);
                        indices[indexIndex++] = (uint)(currentRow + j + 1);
                    }
                }
            }
        }

    }
}