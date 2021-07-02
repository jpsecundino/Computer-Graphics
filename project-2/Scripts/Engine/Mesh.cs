using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace World_3D
{
    public class Mesh : IDisposable
    {
        private VertexArrayObject<float, uint> VAO;
        private BufferObject<float> VBO;
        private BufferObject<uint> EBO;
        private Texture _texture;
        private VAObuffers buffers;

        public Mesh(string meshFilePath, string textureFilePath)
        {
            WavefrontReader.WavefrontData data = WavefrontReader.ReadAll(meshFilePath);
            buffers = Mesh.CreateVAOBuffers(data);

            float[] vertices = new float[buffers.vertices.Length * 5];

            {
                int i = 0;
                foreach (var obj in buffers.vertices)
                {
                    vertices[i] = obj.position.X;
                    vertices[i + 1] = obj.position.Y;
                    vertices[i + 2] = obj.position.Z;
                    vertices[i + 3] = obj.uv.X;
                    vertices[i + 4] = obj.uv.Y;
                    i += 5;
                }
            }

            EBO = new BufferObject<uint>(buffers.indices, BufferTargetARB.ElementArrayBuffer);
            VBO = new BufferObject<float>(vertices, BufferTargetARB.ArrayBuffer);
            VAO = new VertexArrayObject<float, uint>(VBO, EBO);
            
            _texture = new Texture(textureFilePath);

            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
            VAO.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

        }

        public void Draw(Shader shader)
        {
            _texture.Bind();
            VAO.Bind();
            EBO.Bind();

            unsafe
            {
                Program.Gl.DrawElements(GLEnum.Triangles, (uint)buffers.indices.Length, GLEnum.UnsignedInt, (void*)0);
            }
        }

        
        public static VAObuffers CreateVAOBuffers(WavefrontReader.WavefrontData data)
        {
            List<VertexObject> vertices = new();
            List<uint> indices = new();

            Dictionary<KeyValuePair<int, int>, uint> indexBufferSet = new(data.vertexIndices.Length);

            for (int i = 0; i < data.vertexIndices.Length; i++)
            {
                var pair = KeyValuePair.Create(data.vertexIndices[i], data.uvIndices[i]);

                if (!indexBufferSet.ContainsKey(pair))
                {
                    var vertex = new VertexObject
                    {
                        // Wavefront indexing is 1 based
                        position = data.vertices[pair.Key - 1],
                        uv = data.uvs[pair.Value - 1]
                    };
                    vertices.Add(vertex);
                    indexBufferSet.Add(pair, (uint)(vertices.Count - 1));
                }

                indices.Add(indexBufferSet[pair]);
            }

            return new VAObuffers
            {
                vertices = vertices.ToArray(),
                indices = indices.ToArray()
            };
        }

        public void PrintInfo()
        {
            System.Console.WriteLine("Vertices");
            foreach (var vertex in buffers.vertices)
            {
                System.Console.WriteLine(vertex);
            }

            System.Console.WriteLine("Indices");
            foreach (var index in buffers.indices)
            {
                System.Console.WriteLine(index);
            }
        }

        public void Dispose()
        {
            VBO.Dispose();
            EBO.Dispose();
            VAO.Dispose();
            _texture.Dispose();
        }

        public struct VAObuffers
        {
            public VertexObject[] vertices;
            public uint[] indices;
        }
    }

    public struct VertexObject
    {
        public Vector3 position;
        public Vector2 uv;

        public override string ToString()
        {
            return $"{position} | {uv}";
        }
    }
}
