using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Linq;
using System.Numerics;
using Silk.NET.Maths;
using Tutorial;

namespace World_3D
{
    class Program
    {
        private static IWindow window;
        public static GL Gl;

        public const int Width = 800;
        public const int Height = 700;
        private static Shader Shader;
        private static Scene activeScene;

        private static void Main(string[] args)
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "LearnOpenGL with Silk.NET";
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

            Gl = GL.GetApi(window);
           
            Shader = new World_3D.Shader("C:\\Users\\pedro\\Documents\\Aulas\\cg\\T2\\Shaders\\shader.vert", "C:\\Users\\pedro\\Documents\\Aulas\\cg\\T2\\Shaders\\shader.frag");

            RenderPipeline rp = new RenderPipeline();

            Scene mainScene = new Scene(rp, Shader);

            Camera cameraComponent = new();
            World_3D.GameObject cameraObj = new();
            cameraObj.AddComponent(cameraComponent);
            Camera.mainCamera = cameraComponent;
            mainScene.AddGameObject(cameraObj);
               
            GameObject bear = new();
            MeshType[] meshes = { MeshType.Bear };
            bear.AddComponent(new Renderer(meshes, Shader));
            
            mainScene.AddGameObject(bear);

            activeScene = mainScene;

            activeScene.StartScene();
        }

        private static unsafe void OnUpdate(double deltaTime)
        {
            if (Input.Keyboard.IsKeyPressed(Key.L))
            {
                Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (Input.Keyboard.IsKeyPressed(Key.P))
            {
                Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            activeScene.UpdateScene(deltaTime);
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Enable(EnableCap.DepthTest);
            Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

            Shader.Use();
            Shader.SetUniform("uTexture0", 0);

            //Use elapsed time to convert to radians to allow our cube to rotate over time
            var difference = (float) (window.Time * 100);

            Shader.SetUniform("uView", Camera.mainCamera.View );
            Shader.SetUniform("uProjection", Camera.mainCamera.Projection);

            //We're drawing with just vertices and no indices, and it takes 36 vertices to have a six-sided textured cube
            //Gl.DrawElements(PrimitiveType.Triangles, IndicesCount, DrawElementsType.UnsignedInt, (void*)0);
            
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

        private static void KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Escape)
            {
                window.Close();
            }
        }
    }
}
