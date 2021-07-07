using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using ImGuiNET;
using Silk.NET.Maths;
using Silk.NET.OpenGL.Extensions.ImGui;
using System.Numerics;

namespace World_3D
{
    static class Program
    {
        private static IWindow mainWindow;
        public static GL Gl { get => gl; private set => gl = value; }

        public const int Width = 1080;
        public const int Height = 1080;
        
        private static Shader Shader;
        private static Scene activeScene;
        private static GL gl;
        private static ImGuiController imGui;

        private static bool isPolygonModeLine = false;

        private static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(Width, Height);
            options.Title = "World 3D";
            mainWindow = Window.Create(options);

            mainWindow.Load += OnLoad;
            // mainWindow.Update += OnUpdateUI;
            mainWindow.Update += OnUpdate;
            mainWindow.Render += OnRender;
            mainWindow.Closing += OnClose;

            mainWindow.Load += OnLoadUI;
            mainWindow.Render += OnRenderUI;
            mainWindow.Closing += OnCloseUI;

            mainWindow.Run();
        }

        private static void OnCloseUI()
        {
            imGui.Dispose();
        }

        private static void OnRenderUI(double deltaTime)
        {
            imGui.Render();
        }

        private static void OnLoadUI()
        {
            imGui = new ImGuiController(gl, mainWindow, Input.InputContext);
            
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        }

        private static void OnLoad()
        {
            Input.Initialize(mainWindow);
            Input.Keyboard.KeyDown += OnKeyDown;

            Gl = GL.GetApi(mainWindow);

            Shader = new Shader("..\\..\\..\\Shaders\\shader.vert", "..\\..\\..\\Shaders\\shader.frag");

            RenderPipeline rp = new();

            Scene mainScene = new(rp);

            var cameraObj = GameObjectFactory.CreateCamera(out Camera cameraComponent);
            Camera.SwitchMainCamera(cameraComponent);
            mainScene.AddGameObject(cameraObj);

            GameObject fishermanHouse = new("house");
            fishermanHouse.AddComponent(new Renderer(ModelType.FishermanHouse, Shader));
            fishermanHouse.Transform.Position = new Vector3(25f, -0.7f, 11f);
            fishermanHouse.Transform.Rotation = new Vector3(3.5f, 0f, -0.1f);
            mainScene.AddGameObject(fishermanHouse);

            GameObject bear = new("bear");
            bear.AddComponent(new Renderer(ModelType.Bear, Shader));
            mainScene.AddGameObject(bear);
            
            GameObject pirate = new("pirate");
            pirate.AddComponent(new Renderer(ModelType.Pirate, Shader));
            pirate.Transform.Position = new Vector3(24f, -0.5f, 11.5f);
            pirate.Transform.Rotation = new Vector3(-83f, 0f, 0f);
            pirate.Transform.Scale = new Vector3(0.45f, 0.45f, 0.45f);
            mainScene.AddGameObject(pirate);
            
            GameObject campfire = new("campfire");
            campfire.AddComponent(new Renderer(ModelType.Campfire, Shader));
            campfire.Transform.Position = new Vector3(25f, -0.27f, 11f);
            campfire.Transform.Rotation = new Vector3(0f, 0f, -6f);
            campfire.Transform.Scale = new Vector3(10f, 10f, 10f);
            mainScene.AddGameObject(campfire);
            
            GameObject pirateSword = new("pirateSword");
            pirateSword.AddComponent(new Renderer(ModelType.PirateSword, Shader));
            pirateSword.Transform.Position = new Vector3(25.4f, -0.18f, 11f);
            pirateSword.Transform.Rotation = new Vector3(0f, 0f, -6f);
            pirateSword.Transform.Scale = new Vector3(0.2f, 0.2f, 0.2f);
            mainScene.AddGameObject(pirateSword);
            
            GameObject spyglass = new("spyglass");
            spyglass.AddComponent(new Renderer(ModelType.Spyglass, Shader));
            spyglass.Transform.Position = new Vector3(25.1f, -0.25f, 10.6f);
            spyglass.Transform.Rotation = new Vector3(11f, 0.2f, -2f);
            spyglass.Transform.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            mainScene.AddGameObject(spyglass);

            GameObject griffin = GameObjectFactory.CreateGriffin(Shader);
            mainScene.AddGameObject(griffin);
            
            GameObject terrain = new("terrain");
            terrain.AddComponent(new Renderer(ModelType.Terrain, Shader));
            mainScene.AddGameObject(terrain);
            terrain.Transform.Scale = Vector3.One;

            GameObject ship = GameObjectFactory.CreateShip(Shader);
            mainScene.AddGameObject(ship);
            
            var skybox = GameObjectFactory.CreateSkyBox(Shader);
            mainScene.AddGameObject(skybox);

            activeScene = mainScene;

            activeScene.StartScene();
        }

        private static unsafe void OnUpdate(double deltaTime)
        {   
            imGui.Update((float) deltaTime);
            activeScene.DrawHierarchy();
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
                mainWindow.Close();
            }
        }
    }
}
