using System;
using System.Numerics;
using SixLabors.ImageSharp.Processing.Processors.Filters;

namespace AllianceEngine
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
            griffin.Transform.Position = new Vector3(11f, 1.5f, 16f);
            griffin.AddComponent(new Renderer(ModelType.Griffin, shader));

            griffin.AddComponent(new LoopMovement(type: LoopMovement.LoopType.XZ, speed: 3f, radius: 5f));


            return griffin;
        }

        public static GameObject CreateShip(Shader shader)
        {
            GameObject ship = new("ship");
            ship.AddComponent(new Renderer(ModelType.Ship, shader));
            ship.Transform.Position = new Vector3(11f, -2.5f, 16f);

            return ship;
        }
        
        public static GameObject CreateSun(Shader shader)
        {
            GameObject light = new("sun");

            light.Transform.Position = new Vector3(15f, 14f, -1000f);
            light.AddComponent(new Light(shader, new Vector3(0.9f,0.9f,0.9f), 1, float.MaxValue));
            light.AddComponent(new Renderer(ModelType.Cube, shader));
            
            return light;
        }
        
        public static GameObject CreateBear(Shader shader)
        {
            GameObject bear = new("bear");
            
            bear.AddComponent(new Renderer(ModelType.Bear, shader));
            bear.Transform.Position = new Vector3(11f, -0.7f, 18f);
            bear.Transform.Rotation = new Vector3(-15f, 0f, 0f);
            bear.Transform.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            
            return bear;
        }
        public static GameObject CreatePirate(Shader shader)
        {
            GameObject pirate = new("pirate");
            
            pirate.AddComponent(new Renderer(ModelType.Pirate, shader));
            pirate.Transform.Position = new Vector3(24f, -0.5f, 11.3f);
            pirate.Transform.Rotation = new Vector3(-78f, 0f, 0f) * MathHelper.DegreesToRadians;
            pirate.Transform.Scale = new Vector3(0.3f, 0.3f, 0.3f);
            
            return pirate;
        }
        
        public static GameObject CreateCamera(out Camera cameraComponent)
        {
            GameObject cameraObj = new("camera");
            cameraComponent = new();
            cameraObj.AddComponent(cameraComponent);
            cameraObj.AddComponent(new CameraControls(cameraComponent));
            cameraObj.AddComponent(new LimitMovementVolume(new Vector3(11f, 12.5f, 16f), new Vector3(25, 12.5f, 25)));
            cameraObj.Transform.Position = new Vector3(35.876f, 4.425f, 12.543f);
            return cameraObj;
        }

        public static GameObject CreateTerrain(Shader shader)
        {
            GameObject terrain = new("terrain");
            terrain.AddComponent(new Renderer(ModelType.Terrain, shader));

            return terrain;
        }
        
        public static GameObject CreateCube(Shader shader)
        {
            GameObject cube = new("cube");
            cube.AddComponent(new Renderer(ModelType.Cube, shader));

            return cube;
        }

        public static GameObject CreateCampfire(Shader shader)
        {
                        
            GameObject campfire = new("campfire");
            campfire.AddComponent(new Renderer(ModelType.Campfire, shader));
            campfire.AddComponent(new Light(shader, new Vector3(1,0,0), 2, 2f));
            campfire.Transform.Position = new Vector3(25f, -0.27f, 11f);
            campfire.Transform.Rotation = new Vector3(0f, 0f, -6f);
            campfire.Transform.Scale = new Vector3(10f, 10f, 10f);

            return campfire;
        }
        
        public static GameObject CreatePirateSword(Shader shader)
        {
                        
            GameObject pirateSword = new("pirateSword");
            
            pirateSword.AddComponent(new Renderer(ModelType.PirateSword, shader));
            pirateSword.Transform.Position = new Vector3(25.4f, -0.18f, 11f);
            pirateSword.Transform.Rotation = new Vector3(0f, 0f, -6f);
            pirateSword.Transform.Scale = new Vector3(0.2f, 0.2f, 0.2f);

            return pirateSword;
        }
        
        public static GameObject CreateSpyGlass(Shader shader)
        {
                        
            GameObject spyglass = new("spyglass");
            
            spyglass.AddComponent(new Renderer(ModelType.Spyglass, shader));
            spyglass.Transform.Position = new Vector3(25.1f, -0.25f, 10.6f);
            spyglass.Transform.Rotation = new Vector3(11f, 0.2f, -2f);
            spyglass.Transform.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            return spyglass;
        }
        
        
        public static GameObject CreateTent(Shader shader)
        {
                        
            GameObject tent = new("tent");
            
            tent.AddComponent(new Renderer(ModelType.Tent, shader));
            tent.Transform.Position = new Vector3(24f,-0.7f,11.2f);
            tent.Transform.Rotation = new Vector3(-73.0f,0f,0f) * MathHelper.DegreesToRadians;
            tent.Transform.Scale = new Vector3(1.5f,1.5f,1.5f);

            return tent;
        }
        
    }
}
