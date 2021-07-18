using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AllianceEngine
{
    public sealed class Mesh : IDisposable
    {
        private struct MeshBuffers
        {
            public int[] vertexIndices;
            public int[] uvIndices;
            public int[] normalsIndices;
            public Vector3[] vertices;
            public Vector2[] uvs;
            public Vector3[] normals;

            public MeshBuffers(int[] vertexIndices, int[] uvIndices, int[] normalsIndices, Vector3[] vertices, Vector2[] uvs, Vector3[] normals)
            {
                this.vertexIndices = vertexIndices;
                this.uvIndices = uvIndices;
                this.normalsIndices = normalsIndices;
                this.vertices = vertices;
                this.uvs = uvs;
                this.normals = normals;
            }
        }

        private struct MaterialInfo
        {
            public Material material;
            public int idxCount;

            public MaterialInfo(Material material, int idxCount)
            {
                this.material = material;
                this.idxCount = idxCount;
            }
        }
        
        private readonly VertexArrayObject<float, uint> VAO;
        private readonly BufferObject<float> VBO;
        private readonly BufferObject<uint> EBO;
        private readonly VAObuffers buffers;
        private readonly List<MaterialInfo> _materials = new();
        
        public Mesh(List<ModelReader.MeshObjectData> meshObjects)
        {
            MeshBuffers meshBuffers =  ConcatenateBuffers(meshObjects);
            
            buffers = CreateVAOBuffers(meshBuffers);

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
            //VAO.VertexAttributePointer(2, 3, VertexAttribPointerType.Float, 8, 5);
            
        }

        public void Draw(Shader shader)
        {
            VAO.Bind();
            EBO.Bind();

            unsafe
            {
                int offset = 0;
                foreach (MaterialInfo matInfo in _materials)
                {
                    matInfo.material.Bind(shader);
                    Program.Gl.DrawElements(GLEnum.Triangles, (uint) matInfo.idxCount, GLEnum.UnsignedInt, (void*) (offset * sizeof(uint)));
                    offset += matInfo.idxCount;
                }
                
            }
        }
        
        private static VAObuffers CreateVAOBuffers(MeshBuffers meshBuffers)
        {
            List<VertexObject> vertices = new();
            List<uint> indices = new();

            Dictionary<Tuple<int, int, int>, uint> indexBufferSet = new(meshBuffers.vertexIndices.Length);

            for (int i = 0; i < meshBuffers.vertexIndices.Length; i++)
            {
                var triple = new Tuple<int, int, int>(meshBuffers.vertexIndices[i], meshBuffers.uvIndices[i], meshBuffers.normalsIndices[i]);

                if (!indexBufferSet.ContainsKey(triple))
                {
                    var vertex = new VertexObject
                    {
                        // Wavefront indexing is 1 based
                        position = meshBuffers.vertices[triple.Item1 - 1],
                        uv = meshBuffers.uvs[triple.Item2 - 1],
                        normal = meshBuffers.normals[triple.Item3 - 1]
                        
                    };
                    
                    vertices.Add(vertex);
                    
                    indexBufferSet.Add(triple, (uint)(vertices.Count - 1));
                }

                indices.Add(indexBufferSet[triple]);
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
            int[] normalsIndices = Array.Empty<int>();
            Vector3[] vertices = Array.Empty<Vector3>();
            Vector2[] uvs = Array.Empty<Vector2>();
            Vector3[] normals = Array.Empty<Vector3>();

            foreach (var obj in meshObjects)
            {
                _materials.Add(new MaterialInfo(obj.material, obj.vertexIndices.Length));

                vertexIndices = vertexIndices.Concat(obj.vertexIndices).ToArray();
                uvIndices = uvIndices.Concat(obj.uvIndices).ToArray();
                normalsIndices = normalsIndices.Concat(obj.normalsIndices).ToArray();
                vertices = vertices.Concat(obj.vertices).ToArray();
                uvs = uvs.Concat(obj.uvs).ToArray();
                normals = normals.Concat(obj.normals).ToArray();
            }

            MeshBuffers meshBuffers = new(vertexIndices, uvIndices, normalsIndices, vertices, uvs, normals);

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

            foreach(MaterialInfo textureInfo in _materials)
            {
                textureInfo.material.texture.Dispose();
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
        public Vector3 normal;

        public override string ToString()
        {
            return $"{position} | {uv} | {normal}";
        }
    }
}
