using System.Numerics;

namespace World_3D
{
    public static class GameObjectFactory
    {
        public static GameObject CreateSkyBox(Shader defaultShader)
        {
            GameObject skybox = new("skybox");
            skybox.Transform.Scale = new Vector3(Camera.MainCamera.FarPlaneDistance / 2f);

            skybox.AddComponent(new Renderer(ModelType.Skybox, defaultShader));
            skybox.AddComponent(new FollowCamera());

            return skybox;
        }

        public static GameObject CreateGriffin(Shader shader)
        {
            GameObject griffin = new("griffin");
            griffin.AddComponent(new Renderer(ModelType.Griffin, shader));
            griffin.AddComponent(new LoopMovement(type: LoopMovement.LoopType.YZ));
            griffin.Transform.Position += Vector3.UnitX * 3f;

            return griffin;
        }

        public static GameObject CreateShip(Shader shader)
        {
            GameObject ship = new("ship");
            ship.AddComponent(new Renderer(ModelType.Ship, shader));
            ship.Transform.Position = new Vector3(11f, -2.5f, 16f);

            return ship;
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
    }
}
