using System.Numerics;

namespace World_3D
{
    public static class GameObjectFactory
    {
        public static GameObject CreateSkyBox(Shader defaultShader)
        {
            GameObject skybox = new();
            skybox.Transform.Scale = Camera.MainCamera.FarPlaneDistance / 2f;

            skybox.AddComponent(new Renderer(ModelType.Skybox, defaultShader));
            skybox.AddComponent(new FollowCamera());

            return skybox;
        }

        public static GameObject CreateGriffin(Shader shader)
        {
            GameObject griffin = new();
            griffin.AddComponent(new Renderer(ModelType.Griffin, shader));
            griffin.AddComponent(new LoopMovement(type: LoopMovement.LoopType.YZ));
            griffin.Transform.Position += Vector3.UnitX * 3f;

            return griffin;
        }

        public static GameObject CreateShip(Shader shader)
        {
            GameObject ship = new();
            ship.AddComponent(new Renderer(ModelType.Ship, shader));
            ship.Transform.Position = new Vector3(7f, -2f, 20f);
            ship.AddComponent(new LoopMovement(speed: 3f, radius: 2f, type: LoopMovement.LoopType.XZ));

            return ship;
        }

        public static GameObject CreateCamera(out Camera cameraComponent)
        {
            GameObject cameraObj = new();
            cameraComponent = new();
            cameraObj.AddComponent(cameraComponent);
            cameraObj.AddComponent(new CameraMovement());
            cameraObj.AddComponent(new CameraZoom(cameraComponent));
            cameraObj.AddComponent(new BlockMovementVolume(new Vector3(50, 25, 50)));
            return cameraObj;
        }
    }
}
