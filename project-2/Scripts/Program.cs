using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
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

            Scene mainScene = new(rp, Shader);

            GameObject cameraObj = new();
            Camera cameraComponent = new();
            cameraObj.AddComponent(cameraComponent);
            cameraObj.AddComponent(new CameraMovement());
            Camera.mainCamera = cameraComponent;
            
            cameraObj.AddComponent(new BlockMovementVolume(new Vector3(20, 20, 20)));
            mainScene.AddGameObject(cameraObj);
            
            GameObject bear = new();
            MeshType[] meshes = { MeshType.Bear };
            bear.AddComponent(new Renderer(meshes, Shader));
            mainScene.AddGameObject(bear);

            GameObject griffin = new();
            griffin.AddComponent(new Renderer(new MeshType[] { MeshType.Griffin }, Shader));
            griffin.Transform.Position += Vector3.UnitX * 3f;
            mainScene.AddGameObject(griffin);

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
            Shader.SetUniform("uView", Camera.mainCamera.View );
            Shader.SetUniform("uProjection", Camera.mainCamera.Projection);
            
            activeScene.DrawObjects();
        }

        private static unsafe void OnMouseWheel(IMouse mouse, ScrollWheel scrollWheel)
        {
            //We don't want to be able to zoom in too close or too far away so clamp to these values
            Camera.mainCamera.CameraZoom = Math.Clamp(Camera.mainCamera.CameraZoom - scrollWheel.Y, 1.0f, 45f);
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
