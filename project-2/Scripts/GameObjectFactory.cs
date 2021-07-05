using System.Numerics;

namespace World_3D
{
    public static class GameObjectFactory
    {
        public static GameObject CreateSkyBox(Shader defaultShader)
        {
            GameObject skybox = new();
            skybox.Transform.Scale = Camera.MainCamera.FarPlaneDistance / 2f;

            skybox.AddComponent(new Renderer(new ModelType[] { ModelType.Skybox }, defaultShader));
            skybox.AddComponent(new FollowCamera());

            return skybox;
        }

        public static GameObject CreateGriffin(Shader shader)
        {
            GameObject griffin = new();
            griffin.AddComponent(new Renderer(new ModelType[] { ModelType.Griffin }, shader));
            griffin.AddComponent(new LoopMovement());
            griffin.Transform.Position += Vector3.UnitX * 3f;

            return griffin;
        }
    }
}
