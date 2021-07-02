namespace World_3D
{
    public static class GameObjectFactory
    {
        public static GameObject CreateSkyBox(Shader defaultShader)
        {
            GameObject skybox = new();
            skybox.Transform.Scale = Camera.mainCamera.FarPlaneDistance / 2f;

            skybox.AddComponent(new Renderer(new MeshType[] { MeshType.Skybox }, defaultShader));
            skybox.AddComponent(new FollowCamera());

            return skybox;
        }
    }
}
