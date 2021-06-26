namespace World_3D
{
    public static class GameObjectFactory
    {
        public static GameObject CreateSkyBox(Shader defaultShader)
        {
            GameObject skybox = new GameObject();
            skybox.transform.Scale = 25f;

            skybox.AddComponent(new Renderer(new MeshType[] { MeshType.Skybox }, defaultShader));
            skybox.AddComponent(new FollowCamera());

            return skybox;
        }
    }
}
