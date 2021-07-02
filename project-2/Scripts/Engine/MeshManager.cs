using System;
using System.Collections.Generic;

namespace World_3D
{

    public struct MeshData
    {
        public readonly string modelPath;
        public readonly string texturePath;

        public MeshData(string modelPath, string texturePath)
        {
            this.modelPath = modelPath;
            this.texturePath = texturePath;
        }
    }

    public static class MeshManager
    {
        private static Dictionary<MeshType, Mesh> loadedMeshes = new();
        private static Dictionary<MeshType, MeshData> meshPaths = new() {
            { MeshType.Bear, new MeshData("..\\..\\..\\Models\\bear.obj", "..\\..\\..\\Models\\textures\\wizardTowerDiff.png") },
            { MeshType.Skybox, new MeshData("..\\..\\..\\Models\\skycube_blender.obj", "..\\..\\..\\Models\\textures\\skybox\\bluesunset_skybox.png") },
            { MeshType.Griffin, new MeshData("..\\..\\..\\Models\\griffin_animated.obj", "..\\..\\..\\Models\\textures\\griffon_Diff.png") },
        };

        public static Mesh GetMesh(MeshType meshType)
        {
            if (!loadedMeshes.TryGetValue(meshType, out Mesh returnMesh))
            {
                returnMesh = CreateMesh(meshType);

                loadedMeshes[meshType] = returnMesh;
            }

            return returnMesh;

        }

        private static Mesh CreateMesh(MeshType meshType)
        {
            Mesh newMesh;

            if (meshPaths.TryGetValue(meshType, out MeshData md))
            {
                newMesh = new Mesh(md.modelPath, md.texturePath);
            }
            else
            {
                throw new ArgumentException($"Model {meshType} not found");
            }

            return newMesh;
        }
    }
}
