using System;
using System.Numerics;

namespace World_3D
{
    public static class GameObjectFactory
    {
        public static GameObject CreateSkyBox(Shader defaultShader)
        {
            GameObject skybox = new("skybox");
            skybox.AddComponent(new Renderer(ModelType.Skybox, defaultShader));
            skybox.AddComponent(new Skybox());

            return skybox;
        }

        public static GameObject CreateGriffin(Shader shader)
        {
            GameObject griffin = new("griffin");
            griffin.AddComponent(new Renderer(ModelType.Griffin, shader));
            griffin.AddComponent(new LoopMovement(type: LoopMovement.LoopType.YZ));

            return griffin;
        }

        public static GameObject CreateShip(Shader shader)
        {
            GameObject ship = new("ship");
            ship.AddComponent(new Renderer(ModelType.Ship, shader));
            ship.Transform.Position = new Vector3(11f, -2.5f, 16f);

            return ship;
        }
        
        public static GameObject CreateBear(Shader shader)
        {
            GameObject bear = new("bear");
            bear.AddComponent(new Renderer(ModelType.Bear, shader));

            return bear;
        }
        public static GameObject CreatePirate(Shader shader)
        {
            GameObject pirate = new("pirate");
            pirate.AddComponent(new Renderer(ModelType.Pirate, shader));
            
            return pirate;
        }

        public static GameObject CreateFishermanHouse(Shader shader)
        {
            GameObject fishermanHouse = new("house");
            fishermanHouse.AddComponent(new Renderer(ModelType.FishermanHouse, shader));
            fishermanHouse.Transform.Position = new Vector3(25f, -0.7f, 11f);
            fishermanHouse.Transform.Rotation = new Vector3(3.5f, 0f, -0.1f);
            return fishermanHouse;
        }

        public static GameObject CreateCamera(out Camera cameraComponent)
        {
            GameObject cameraObj = new("camera");
            cameraComponent = new();
            cameraObj.AddComponent(cameraComponent);
            cameraObj.AddComponent(new CameraMovement());
            cameraObj.AddComponent(new CameraZoom(cameraComponent));
            cameraObj.AddComponent(new BlockMovementVolume(new Vector3(50, 25, 50)));
            return cameraObj;
        }

        public static GameObject CreateTerrain(Shader shader)
        {
            GameObject terrain = new("terrain");
            terrain.AddComponent(new Renderer(ModelType.Terrain, shader));

            return terrain;
        }
    }
}
