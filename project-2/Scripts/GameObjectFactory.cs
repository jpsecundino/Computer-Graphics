using System.Numerics;

namespace World_3D
{
    public static class GameObjectFactory
    {
        public static GameObject CreateSkyBox(Shader defaultShader)
        {
            GameObject skybox = new();
            skybox.Transform.Scale = Camera.MainCamera.FarPlaneDistance / 2f;

            skybox.AddComponent(new Renderer(new MeshType[] { MeshType.Skybox }, defaultShader));
            skybox.AddComponent(new FollowCamera());

            return skybox;
        }

        public static GameObject CreateGriffin(Shader shader)
        {
            GameObject griffin = new();
            griffin.AddComponent(new Renderer(new MeshType[] { MeshType.Griffin }, shader));
            griffin.AddComponent(new LoopMovement());
            griffin.Transform.Position += Vector3.UnitX * 3f;

            return griffin;
        }

        public static GameObject CreateCamera(out Camera cameraComponent)
        {
            GameObject cameraObj = new();
            cameraComponent = new();
            cameraObj.AddComponent(cameraComponent);
            cameraObj.AddComponent(new CameraMovement());
            cameraObj.AddComponent(new CameraZoom(cameraComponent));
            cameraObj.AddComponent(new BlockMovementVolume(new Vector3(50, 10, 50)));
            return cameraObj;
        }
    }
}
