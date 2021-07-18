using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AllianceEngine
{
    public static class ModelReader
    {
        public struct MeshObjectData
        {
            public Vector3[] vertices;
            public Vector2[] uvs;
            public Vector3[] normals;
            public int[] vertexIndices;
            public int[] uvIndices;
            public int[] normalsIndices;
            public Material material;
        }

        public static List<MeshObjectData> ReadAll(string filepath, string mtlFile = null, string texturesFolder = null)
        {

            Dictionary<string, Material> materials = ReadMaterials(mtlFile, texturesFolder);

            using (StreamReader file = File.OpenText(filepath))
            {
                List<MeshObjectData> meshes = new();

                while (!file.EndOfStream)
                {
                    List<Vector3> vertices = new();
                    List<Vector2> uvs = new();
                    List<Vector3> norms = new();
                    List<int> vertexIndices = new();
                    List<int> uvIndices = new();
                    List<int> normsIndices = new();
                    string materialName = "";
                    bool meshEmpty = true;

                    string line = file.ReadLine();

                    if (line == null)
                        break;

                    while (!file.EndOfStream)
                    {
                        line = file.ReadLine();

                        if (line == null)
                            break;

                        if (line.StartsWithOptimized("o "))
                        {
                            if (meshEmpty == false && materialName != "") //if we have info to fill the mesh
                            {
                                meshes.Add(new MeshObjectData()
                                {
                                    vertices = vertices.ToArray(),
                                    uvs = uvs.ToArray(),
                                    normals = norms.ToArray(),
                                    vertexIndices = vertexIndices.ToArray(),
                                    uvIndices = uvIndices.ToArray(),
                                    normalsIndices = normsIndices.ToArray(),
                                    material = materials[materialName]
                                });

                                meshEmpty = true;
                                vertices = new();
                                uvs = new();
                                vertexIndices = new();
                                uvIndices = new();
                                materialName = "";
                            }
                        }
                        else if (line.StartsWithOptimized("v "))
                        {
                            string[] verticesText = line[2..].Split(' ');
                            Vector3 newVertex = Vector3.Zero;

                            if (Single.TryParse(verticesText[0], NumberStyles.Float, CultureInfo.InvariantCulture,
                                out float axis))
                                newVertex.X = axis;
                            if (Single.TryParse(verticesText[1], NumberStyles.Float, CultureInfo.InvariantCulture,
                                out axis))
                                newVertex.Y = axis;
                            if (Single.TryParse(verticesText[2], NumberStyles.Float, CultureInfo.InvariantCulture,
                                out axis))
                                newVertex.Z = axis;

                            vertices.Add(newVertex);

                            meshEmpty = false;
                        }
                        else if (line.StartsWithOptimized("vt "))
                        {
                            string[] uvsText = line[3..].Split(' ');
                            Vector2 newUv = Vector2.Zero;

                            if (float.TryParse(uvsText[0], NumberStyles.Float, CultureInfo.InvariantCulture,
                                out float axis))
                                newUv.X = axis;
                            if (float.TryParse(uvsText[1], NumberStyles.Float, CultureInfo.InvariantCulture, out axis))
                                newUv.Y = axis;

                            uvs.Add(newUv);

                            meshEmpty = false;
                        }else if (line.StartsWithOptimized("vn "))
                        {
                            string[] vnsText = line[3..].Split(' ');
                            Vector3 newVn = Vector3.Zero;

                            if (float.TryParse(vnsText[0], NumberStyles.Float, CultureInfo.InvariantCulture,
                                out float axis))
                                newVn.X = axis;
                            if (float.TryParse(vnsText[1], NumberStyles.Float, CultureInfo.InvariantCulture, out axis))
                                newVn.Y = axis;
                            if (float.TryParse(vnsText[2], NumberStyles.Float, CultureInfo.InvariantCulture, out axis))
                                newVn.Z = axis;

                            norms.Add(newVn);

                            meshEmpty = false;
                        }
                        else if (line.StartsWithOptimized("f "))
                        {
                            string[] faces = line[2..].Split(' ');

                            foreach (var comb in faces)
                            {
                                string[] indices = comb.Split('/');
                                if (int.TryParse(indices[0], out int vertexIndex))
                                    vertexIndices.Add(vertexIndex);
                                if (int.TryParse(indices[1], out int uvIndex))
                                    uvIndices.Add(uvIndex);
                                if (int.TryParse(indices[2], out int vnIndex))
                                    normsIndices.Add(vnIndex);
                            }

                            meshEmpty = false;
                        }
                        else if (line.StartsWithOptimized("usemtl "))
                        {
                            materialName = line.Split(' ').Last();
                        }
                    }

                    meshes.Add(new MeshObjectData()
                    {
                        vertices = vertices.ToArray(),
                        uvs = uvs.ToArray(),
                        normals = norms.ToArray(),
                        vertexIndices = vertexIndices.ToArray(),
                        uvIndices = uvIndices.ToArray(),
                        normalsIndices = normsIndices.ToArray(),
                        material = materials[materialName]
                    });
                }

                return meshes;
            }

            throw new FileNotFoundException("Could not find file with path " + filepath);

        }

        private static Dictionary<string, Material> ReadMaterials(string mtlFile, string texturesFolder)
        {
            Dictionary<string, Material> materials = new();

            bool isFirstMaterial = true;
            
            using (StreamReader file = File.OpenText(mtlFile))
            {
                Material material = new Material("");
                
                string txtImgName = "";
                
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();

                    if (line == null)
                        break;

                    if (line.StartsWith("newmtl "))
                    {

                        if (!isFirstMaterial)
                        {
                            materials.Add(material.name, material);
                        }
                        
                        
                        material.name = line.Split(' ')[1];
                        
                        isFirstMaterial = false;

                    }else if (line.StartsWith("Ka "))
                    {
                        
                        string[] svector = line.Replace('.', ',').Split(" ")[1..4];
                        
                        material.ka = new Vector3( 
                            float.Parse(svector[0]),
                            float.Parse(svector[1]),
                            float.Parse(svector[2])
                            );

                    }else if (line.StartsWith("Kd "))
                    {
                        string[] svector = line.Replace('.', ',').Split(" ")[1..4];
                        
                        material.kd = new Vector3( 
                            float.Parse(svector[0]),
                            float.Parse(svector[1]),
                            float.Parse(svector[2])
                        );
                    }else if (line.StartsWith("Ks "))
                    {
                        string[] svector = line.Replace('.', ',').Split(" ")[1..4];
                        
                        material.ks = new Vector3( 
                            float.Parse(svector[0]),
                            float.Parse(svector[1]),
                            float.Parse(svector[2])
                        );
                        
                    }
                    else if (line.StartsWith("map_Kd "))
                    {
                        
                        txtImgName = line.Split(" ").Last();

                        if (txtImgName.Contains("\\"))
                        {
                            txtImgName = line.Split("\\").Last();    
                        }

                        string path = Path.Join(texturesFolder, txtImgName);
                        
                        Texture newTxt = new Texture(path);

                        material.texture = newTxt;
                    }
                    
                }
                    
                materials.Add(material.name, material);

                return materials;
            }

            throw new FileNotFoundException("Could not find file with path " + mtlFile);
        }
    }
}
