using System;
using System.Collections.Generic;

namespace World_3D
{

    public struct ModelPaths
    {
        public readonly string modelPath;
        public readonly string textureFolderPath;
        public readonly string mtlPath;

        public ModelPaths(string modelPath, string textureFolderPath, string mtlPath)
        {
            this.modelPath = modelPath;
            this.textureFolderPath = textureFolderPath;
            this.mtlPath = mtlPath;
        }
    }

    public static class ModelManager
    {
        private static Dictionary<ModelType, Mesh> loadedMeshes = new();
        private static Dictionary<ModelType, ModelPaths> meshPaths = new() {
            { ModelType.Bear, new ModelPaths("..\\..\\..\\Models\\bear.obj", "..\\..\\..\\Models\\textures\\wizardTowerDiff.png","..\\..\\..\\Models\\textures" ) },
            { ModelType.Skybox, new ModelPaths("..\\..\\..\\Models\\skycube_blender.obj", "..\\..\\..\\Models\\textures\\skybox\\bluesunset_skybox.png", "..\\..\\..\\Models\\textures") },
            { ModelType.Griffin, new ModelPaths("..\\..\\..\\Models\\griffin_animated.obj", "..\\..\\..\\Models\\textures\\griffon_Diff.png", "..\\..\\..\\Models\\textures") },
            { ModelType.Terrain, new ModelPaths("..\\..\\..\\Models\\terrain.obj", "..\\..\\..\\Models\\textures\\terrain.png", "..\\..\\..\\Models\\textures") },
            { ModelType.Ship, new ModelPaths("..\\..\\..\\Models\\ship.obj", "..\\..\\..\\Models\\textures\\ship.png", "..\\..\\..\\Models\\textures") },
            { ModelType.House, new ModelPaths("..\\..\\..\\Models\\house.obj", "..\\..\\..\\Models\\textures\\house.png", "..\\..\\..\\Models\\textures") },
            { ModelType.FishermanHouse, new ModelPaths("..\\..\\..\\Models\\fisherman.obj", "..\\..\\..\\Models\\textures\\fisherman", "..\\..\\..\\Models\\fisherman.mtl") },
        };

        public static Mesh GetModel(ModelType modelType)
        {
            if (!loadedMeshes.TryGetValue(modelType, out Mesh returnMesh))
            {
                returnMesh = CreateModel(modelType);

                loadedMeshes[modelType] = returnMesh;
            }

            return returnMesh;

        }

        private static Mesh CreateModel(ModelType modelType)
        {
            Mesh newModel;

            if (meshPaths.TryGetValue(modelType, out ModelPaths md))
            {
                newModel = new Mesh(ModelReader.ReadAll(md.modelPath, md.mtlPath, md.textureFolderPath));
            }
            else
            {
                throw new ArgumentException($"Model {modelType} not found");
            }

            return newModel;
        }
    }
}
