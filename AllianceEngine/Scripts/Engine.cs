using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using ImGuiNET;
using Silk.NET.Maths;
using Silk.NET.OpenGL.Extensions.ImGui;
using System.Numerics;
using System.Drawing;

namespace AllianceEngine
{
    static class Program
    {
        private static IWindow mainWindow;
        public static GL Gl { get => gl; private set => gl = value; }

        public static int Width { get; private set; } = 700;
        public static int Height { get; private set; } = 700;
        
        public static Shader Shader;
        private static Scene activeScene;
        private static GL gl;
        private static ImGuiController imGui;

        private static bool isPolygonModeLine = false;

        private static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(Width, Height);
            options.Title = "Alliance Engine";
            mainWindow = Window.Create(options);
            
            mainWindow.Load += OnLoad;
            mainWindow.Update += OnUpdate;
            mainWindow.Render += OnRender;
            mainWindow.Closing += OnClose;

            mainWindow.Load += OnLoadUI;
            mainWindow.Render += OnRenderUI;
            mainWindow.Closing += OnCloseUI;

            mainWindow.Resize += OnMainWindowResize;

            mainWindow.Run();
        }

        private static void OnMainWindowResize(Vector2D<int> size)
        {
            gl.Viewport(size);
            Width = size.X;
            Height = size.Y;
        }

        private static void OnLoadUI()
        {
            imGui = new ImGuiController(gl, mainWindow, Input.InputContext);
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            UI.Initialize();
        }

        private static Vector4 ambientLight = new(1);
        private static void OnRenderUI(double deltaTime)
        {
            ImGui.Begin("Lighting Settings");
            ImGui.ColorPicker4("Ambient Light", ref ambientLight);
            ImGui.End();

            imGui.Render();
        }

        private static void OnCloseUI()
        {
            imGui.Dispose();
        }

        private static void OnLoad()
        {
            mainWindow.Center();

            // Prepare Input
            Input.Initialize(mainWindow);
            Input.Keyboard.KeyDown += OnKeyDown;

            // Get OpenGL API (context)
            Gl = GL.GetApi(mainWindow);

            // Initialize Default Shader
            Shader = new Shader("..\\..\\..\\Shaders\\ambientLight.vert", "..\\..\\..\\Shaders\\ambientLight.frag");

            // Create RenderPipeline and Main Scene
            activeScene = new Scene(new RenderPipeline());
            PopulateScene();

            activeScene.StartScene();
        }

        private static unsafe void OnUpdate(double deltaTime)
        {
            imGui.Update((float)deltaTime);
            activeScene.DrawHierarchy();
            activeScene.UpdateScene(deltaTime);
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Enable(EnableCap.DepthTest);
            Gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            Gl.ClearColor(System.Drawing.Color.Beige);

            Shader.Use();
            
            Shader.SetUniform("uLightColor", ambientLight);
            Shader.SetUniform("uView", Camera.MainCamera.View);
            Shader.SetUniform("uProjection", Camera.MainCamera.Projection);
            
            activeScene.DrawObjects();
        }

        private static void OnClose()
        {
            Shader.Dispose();
            ModelManager.DisposeAllMeshes();
        }

        private static void PopulateScene()
        {
            activeScene.AddGameObject( GameObjectFactory.CreateCamera(out Camera cameraComponent));
            activeScene.AddGameObject( GameObjectFactory.CreateTerrain(Shader));
            // activeScene.AddGameObject( GameObjectFactory.CreatePirate(Shader));
            // activeScene.AddGameObject( GameObjectFactory.CreateBear(Shader));
            // activeScene.AddGameObject( GameObjectFactory.CreateGriffin(Shader));
            // activeScene.AddGameObject( GameObjectFactory.CreateShip(Shader));
            // activeScene.AddGameObject( GameObjectFactory.CreateSkyBox(Shader));
            // activeScene.AddGameObject( GameObjectFactory.CreateCampfire(Shader));
            // activeScene.AddGameObject( GameObjectFactory.CreatePirateSword(Shader));
            // activeScene.AddGameObject( GameObjectFactory.CreateSpyGlass(Shader));
            // activeScene.AddGameObject( GameObjectFactory.CreateTent(Shader));

            Camera.SwitchMainCamera(cameraComponent);
        }

        private static void OnKeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            // Toggle Polygon Mode
            if (key == Key.P)
            {
                Gl.PolygonMode(MaterialFace.FrontAndBack, isPolygonModeLine ? PolygonMode.Fill : PolygonMode.Line);
                isPolygonModeLine = !isPolygonModeLine;
            }

            // Toggle Fullscreen
            if(key == Key.F11)
            {
                mainWindow.WindowState = mainWindow.WindowState == WindowState.Normal ? WindowState.Fullscreen : WindowState.Normal;
            }

            // Close window on Escape
            if (key == Key.Escape)
            {
                mainWindow.Close();
            }
        }
    }
}
