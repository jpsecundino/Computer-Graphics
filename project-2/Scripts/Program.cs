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

        private static void OnUpdateUI(double deltaTime)
        {
            imGui.Update((float)deltaTime);
        }

        private static void OnLoadUI()
        {
            imGui = new ImGuiController(gl, mainWindow, Input.InputContext);
            
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        }

        static GameObject ship;
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
            fishermanHouse.AddComponent(new ImguiTransform());
            fishermanHouse.Transform.Position = new Vector3(26f, -0.5f, 12f);
            mainScene.AddGameObject(fishermanHouse);

            GameObject bear = new("bear");
            bear.AddComponent(new Renderer(ModelType.Bear, Shader));
            mainScene.AddGameObject(bear);

            GameObject griffin = GameObjectFactory.CreateGriffin(Shader);
            mainScene.AddGameObject(griffin);
            
            GameObject terrain = new("terrain");
            terrain.AddComponent(new Renderer(ModelType.Terrain, Shader));
            mainScene.AddGameObject(terrain);
            terrain.Transform.Scale = 1f;

            ship = GameObjectFactory.CreateShip(Shader);
            mainScene.AddGameObject(ship);
            
            var skybox = GameObjectFactory.CreateSkyBox(Shader);
            mainScene.AddGameObject(skybox);

            activeScene = mainScene;

            activeScene.StartScene();
        }

        private static unsafe void OnUpdate(double deltaTime)
        {   
            imGui.Update((float) deltaTime);
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
