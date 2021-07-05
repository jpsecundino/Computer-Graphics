using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;
using Silk.NET.Maths;

namespace World_3D
{
    static class Program
    {
        private static IWindow window;
        public static GL Gl { get => gl; private set => gl = value; }

        public const int Width = 800;
        public const int Height = 700;
        
        private static Shader Shader;
        private static Scene activeScene;
        private static GL gl;

        private static bool isPolygonModeLine = false;

        private static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(Width, Height);
            options.Title = "World 3D";
            window = Window.Create(options);

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;
            window.Closing += OnClose;

            window.Run();
        }

        private static void OnLoad()
        {
            Input.Initialize(window);
            Input.Keyboard.KeyDown += OnKeyDown;

            Gl = GL.GetApi(window);

            Shader = new Shader("..\\..\\..\\Shaders\\shader.vert", "..\\..\\..\\Shaders\\shader.frag");

            RenderPipeline rp = new();

            Scene mainScene = new(rp);

            var cameraObj = GameObjectFactory.CreateCamera(out Camera cameraComponent);
            Camera.SwitchMainCamera(cameraComponent);
            mainScene.AddGameObject(cameraObj);
            

            GameObject bear = new();
            MeshType[] meshes = { MeshType.Bear };
            bear.AddComponent(new Renderer(meshes, Shader));
            mainScene.AddGameObject(bear);

            var griffin = GameObjectFactory.CreateGriffin(Shader);
            mainScene.AddGameObject(griffin);

            GameObject terrain = new();
            terrain.AddComponent(new Renderer(new MeshType[] { MeshType.Terrain }, Shader));
            mainScene.AddGameObject(terrain);
            terrain.Transform.Scale = 1f;

            GameObject ship = new();
            ship.AddComponent(new Renderer(new MeshType[] { MeshType.Ship }, Shader));
            mainScene.AddGameObject(ship);

            GameObject house = new();
            house.AddComponent(new Renderer(new MeshType[] { MeshType.House }, Shader));
            mainScene.AddGameObject(house);


            var skybox = GameObjectFactory.CreateSkyBox(Shader);
            mainScene.AddGameObject(skybox);

            activeScene = mainScene;

            activeScene.StartScene();
        }

        private static unsafe void OnUpdate(double deltaTime)
        {
            activeScene.UpdateScene(deltaTime);
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Enable(EnableCap.DepthTest);
            Gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            Gl.ClearColor(System.Drawing.Color.Beige);

            Shader.Use();
            Shader.SetUniform("uTexture0", 0);
            Shader.SetUniform("uView", Camera.MainCamera.View );
            Shader.SetUniform("uProjection", Camera.MainCamera.Projection);
            
            activeScene.DrawObjects();
        }

        private static void OnClose()
        {
            Shader.Dispose();
        }

        private static void OnKeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.P)
            {
                Gl.PolygonMode(MaterialFace.FrontAndBack, isPolygonModeLine ? PolygonMode.Fill : PolygonMode.Line);
                isPolygonModeLine = !isPolygonModeLine;
            }
            if (key == Key.Escape)
            {
                window.Close();
            }
        }
    }
}
