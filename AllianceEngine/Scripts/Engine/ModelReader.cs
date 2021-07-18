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
                            Vector3 newVertex = new(
                                float.Parse(verticesText[0], CultureInfo.InvariantCulture),
                                float.Parse(verticesText[1], CultureInfo.InvariantCulture),
                                float.Parse(verticesText[2], CultureInfo.InvariantCulture)
                            );

                            vertices.Add(newVertex);
                            meshEmpty = false;
                        }
                        else if (line.StartsWithOptimized("vt "))
                        {
                            string[] uvsText = line[3..].Split(' ');
                            Vector2 newUv = new(
                                float.Parse(uvsText[0], CultureInfo.InvariantCulture),
                                float.Parse(uvsText[1], CultureInfo.InvariantCulture)
                            );

                            uvs.Add(newUv);
                            meshEmpty = false;

                        }
                        else if (line.StartsWithOptimized("vn "))
                        {
                            string[] vnsText = line[3..].Split(' ');
                            Vector3 newVn = new (
                                float.Parse(vnsText[0], CultureInfo.InvariantCulture),
                                float.Parse(vnsText[1], CultureInfo.InvariantCulture),
                                float.Parse(vnsText[2], CultureInfo.InvariantCulture)
                            );
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
                        
                        string[] svector = line.Split(" ")[1..4];
                        
                        material.ka = new Vector3( 
                            float.Parse(svector[0], CultureInfo.InvariantCulture),
                            float.Parse(svector[1], CultureInfo.InvariantCulture),
                            float.Parse(svector[2], CultureInfo.InvariantCulture)
                            );

                    }else if (line.StartsWith("Kd "))
                    {
                        string[] svector = line.Split(" ")[1..4];
                        
                        material.kd = new Vector3( 
                            float.Parse(svector[0], CultureInfo.InvariantCulture),
                            float.Parse(svector[1], CultureInfo.InvariantCulture),
                            float.Parse(svector[2], CultureInfo.InvariantCulture)
                        );
                    }else if (line.StartsWith("Ks "))
                    {
                        string[] svector = line.Split(" ")[1..4];
                        
                        material.ks = new Vector3( 
                            float.Parse(svector[0], CultureInfo.InvariantCulture),
                            float.Parse(svector[1], CultureInfo.InvariantCulture),
                            float.Parse(svector[2], CultureInfo.InvariantCulture)
                        );
                        
                    }else if (line.StartsWith("Ns "))
                    {
                        material.ns = float.Parse(line.Replace('.', ',').Split(" ").Last());
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
