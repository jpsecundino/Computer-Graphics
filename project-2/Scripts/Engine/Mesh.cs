using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting;

namespace World_3D
{
    public sealed class Mesh : IDisposable
    {
        private struct MeshBuffers
        {
            public int[] vertexIndices;
            public int[] uvIndices;
            public Vector3[] vertices;
            public Vector2[] uvs;

            public MeshBuffers(int[] vertexIndices, int[] uvIndices, Vector3[] vertices, Vector2[] uvs)
            {
                this.vertexIndices = vertexIndices;
                this.uvIndices = uvIndices;
                this.vertices = vertices;
                this.uvs = uvs;
            }
        }

        private struct TextureInfo
        {
            public Texture texture;
            public int idxCount;

            public TextureInfo(Texture texture, int idxCount)
            {
                this.texture = texture;
                this.idxCount = idxCount;
            }
        }
        
        private readonly VertexArrayObject<float, uint> VAO;
        private readonly BufferObject<float> VBO;
        private readonly BufferObject<uint> EBO;
        private readonly VAObuffers buffers;
        private readonly List<TextureInfo> _textures = new();
        
        public Mesh(List<ModelReader.MeshObjectData> meshObjects)
        {
            MeshBuffers meshBuffers =  ConcatenateBuffers(meshObjects);
            
            buffers = Mesh.CreateVAOBuffers(meshBuffers);

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

            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
            VAO.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);
            
        }

        public void Draw()
        {
            VAO.Bind();
            EBO.Bind();

            unsafe
            {
                int offset = 0;
                foreach (TextureInfo textureInfo in _textures)
                {
                    textureInfo.texture.Bind();
                    Program.Gl.DrawElements(GLEnum.Triangles, (uint) textureInfo.idxCount, GLEnum.UnsignedInt, (void*) (offset * sizeof(uint)));
                    offset += textureInfo.idxCount;
                }
                
            }
        }
        
        private static VAObuffers CreateVAOBuffers(MeshBuffers meshBuffers)
        {
            List<VertexObject> vertices = new();
            List<uint> indices = new();

            Dictionary<KeyValuePair<int, int>, uint> indexBufferSet = new(meshBuffers.vertexIndices.Length);

            for (int i = 0; i < meshBuffers.vertexIndices.Length; i++)
            {
                var pair = KeyValuePair.Create(meshBuffers.vertexIndices[i], meshBuffers.uvIndices[i]);

                if (!indexBufferSet.ContainsKey(pair))
                {
                    var vertex = new VertexObject
                    {
                        // Wavefront indexing is 1 based
                        position = meshBuffers.vertices[pair.Key - 1],
                        uv = meshBuffers.uvs[pair.Value - 1]
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

        private MeshBuffers ConcatenateBuffers(List<ModelReader.MeshObjectData> meshObjects)
        {
            
            int[] vertexIndices = Array.Empty<int>();
            int[] uvIndices = Array.Empty<int>();
            Vector3[] vertices = Array.Empty<Vector3>();
            Vector2[] uvs = Array.Empty<Vector2>();

            foreach (var obj in meshObjects)
            {
                _textures.Add(new TextureInfo(obj.texture, obj.vertexIndices.Length));

                vertexIndices = vertexIndices.Concat(obj.vertexIndices).ToArray();
                uvIndices = uvIndices.Concat(obj.uvIndices).ToArray();
                vertices = vertices.Concat(obj.vertices).ToArray();
                uvs = uvs.Concat(obj.uvs).ToArray();
            }

            MeshBuffers meshBuffers = new(vertexIndices, uvIndices, vertices, uvs);

            return meshBuffers;
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

            foreach(TextureInfo textureInfo in _textures)
            {
                textureInfo.texture.Dispose();
            }
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
