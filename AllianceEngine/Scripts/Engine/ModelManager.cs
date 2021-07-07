using System;
using System.Collections.Generic;
using System.IO;

namespace AllianceEngine
{

    public struct ModelPaths
    {
        private const string baseFolder = "..\\..\\..\\Models";
        public readonly string modelPath;
        public readonly string mtlPath;
        public readonly string textureFolderPath;

        public ModelPaths(string modelName)
        {
            this.modelPath = Path.Combine(baseFolder,modelName,modelName + ".obj");
            this.mtlPath = Path.Combine(baseFolder,modelName, modelName + ".mtl");
            this.textureFolderPath = Path.Combine(baseFolder, modelName, "textures");
        }
    }

    public static class ModelManager
    {
        private static Dictionary<ModelType, Mesh> loadedMeshes = new();
        private static Dictionary<ModelType, ModelPaths> meshPaths = new() {
            { ModelType.Bear, new ModelPaths("bear") },
            { ModelType.Skybox, new ModelPaths("skycube") },
            { ModelType.Griffin, new ModelPaths("griffin") },
            { ModelType.Terrain, new ModelPaths("terrain") },
            { ModelType.Ship, new ModelPaths("ship") },
            { ModelType.FishermanHouse, new ModelPaths("fisherman") },
            { ModelType.Pirate, new ModelPaths("pirate") },
            { ModelType.Campfire, new ModelPaths("campfire") },
            { ModelType.PirateSword, new ModelPaths("piratesword") },
            { ModelType.Spyglass, new ModelPaths("spyglass") },
            { ModelType.Tent, new ModelPaths("tent") }
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

        public static void DisposeAllMeshes()
        {
            foreach(var mesh in loadedMeshes.Values)
            {
                mesh.Dispose();
            }

            loadedMeshes.Clear();
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
