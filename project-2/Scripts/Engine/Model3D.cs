using System.Collections.Generic;

namespace World_3D
{
    public class Model3D
    {
        private readonly List<Mesh> _meshes = new();

        public Model3D(string meshFilePath, string mtlFilePath, string texturesFolderPath)
        {
            List<ModelReader.MeshData> data = ModelReader.ReadAll(meshFilePath, mtlFilePath, texturesFolderPath);
            
            foreach (var meshData in data)
            {
                _meshes.Add(new Mesh(meshData));
            }
        }

        public void Draw()
        {
            foreach (var mesh in _meshes)
            {
                mesh.Draw();
            }
        }
    }
}