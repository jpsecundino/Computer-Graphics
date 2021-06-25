using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace World_3D
{
    public static class WavefrontReader
    {
        public struct WavefrontData
        {
            public Vector3[] vertices;
            public Vector2[] uvs;
            public int[] vertexIndices;
            public int[] uvIndices;
        }

        public static WavefrontData ReadAll(string filepath)
        {
            using (StreamReader file = File.OpenText(filepath))
            {
                List<Vector3> vertices = new();
                List<Vector2> uvs = new();
                List<int> vertexIndices = new();
                List<int> uvIndices = new();

                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    if (line == null)
                        break;

                    if (line.StartsWith("v "))
                    {
                        string[] verticesText = line[2..].Split(' ');
                        Vector3 newVertex = Vector3.Zero;

                        if (Single.TryParse(verticesText[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float axis))
                            newVertex.X = axis;
                        if (Single.TryParse(verticesText[1], NumberStyles.Float, CultureInfo.InvariantCulture, out axis))
                            newVertex.Y = axis;
                        if (Single.TryParse(verticesText[2], NumberStyles.Float, CultureInfo.InvariantCulture, out axis))
                            newVertex.Z = axis;

                        vertices.Add(newVertex);
                    }
                    else if (line.StartsWith("vt "))
                    {
                        string[] uvsText = line[3..].Split(' ');
                        Vector2 newUv = Vector2.Zero;

                        if (float.TryParse(uvsText[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float axis))
                            newUv.X = axis;
                        if (float.TryParse(uvsText[1], NumberStyles.Float, CultureInfo.InvariantCulture, out axis))
                            newUv.Y = axis;

                        uvs.Add(newUv);
                    }
                    else if (line.StartsWith("f "))
                    {
                        string[] faces = line[2..].Split(' ');
                        foreach (var comb in faces)
                        {
                            string[] indices = comb.Split('/');
                            if (int.TryParse(indices[0], out int vertexIndex))
                                vertexIndices.Add(vertexIndex);
                            if (int.TryParse(indices[1], out int uvIndex))
                                uvIndices.Add(uvIndex);
                        }
                    }
                }

                return new WavefrontData()
                {
                    vertices = vertices.ToArray(),
                    uvs = uvs.ToArray(),
                    vertexIndices = vertexIndices.ToArray(),
                    uvIndices = uvIndices.ToArray()
                };
            }

            throw new FileNotFoundException("Could not find file with path " + filepath);
        }
    }
}
