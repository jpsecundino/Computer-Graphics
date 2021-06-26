using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Silk.NET.Maths;
using World_3D;
using World_3D.CameraView;

namespace Tutorial
{
    class Program
    {
        private static IWindow window;
        public static GL Gl;
        public static IKeyboard primaryKeyboard;

        public const int Width = 800;
        public const int Height = 700;
        private static World_3D.Shader Shader;
        private static Scene activeScene;

        //Used to track change in mouse movement to allow for moving of the Camera
        private static Vector2 LastMousePosition;

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
            IInputContext input = window.CreateInput();
            primaryKeyboard = input.Keyboards.FirstOrDefault();
            if (primaryKeyboard != null)
            {
                primaryKeyboard.KeyDown += KeyDown;
            }
            for (int i = 0; i < input.Mice.Count; i++)
            {
                input.Mice[i].Cursor.CursorMode = CursorMode.Raw;
                input.Mice[i].MouseMove += OnMouseMove;
                input.Mice[i].Scroll += OnMouseWheel;
            }

            Gl = GL.GetApi(window);
           
            Shader = new World_3D.Shader("C:\\Users\\pedro\\Documents\\Aulas\\cg\\T2\\Shaders\\shader.vert", "C:\\Users\\pedro\\Documents\\Aulas\\cg\\T2\\Shaders\\shader.frag");

            RenderPipeline rp = new RenderPipeline();

            Scene mainScene = new Scene(rp, Shader);

            Camera cameraComponent = new();
            World_3D.CameraView.GameObject cameraObj = new();
            cameraObj.AddComponent(cameraComponent);
            Camera.mainCamera = cameraComponent;
            mainScene.AddGameObject(cameraObj);
               


            World_3D.CameraView.GameObject bear = new();
            MeshType[] meshes = { MeshType.Bear };
            bear.AddComponent(new Renderer(meshes, Shader));
            
            mainScene.AddGameObject(bear);

            activeScene = mainScene;
        }

        private static unsafe void OnUpdate(double deltaTime)
        {
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

        private static unsafe void OnMouseMove(IMouse mouse, Vector2 position)
        {
            var lookSensitivity = 0.1f;
            if (LastMousePosition == default) { LastMousePosition = position; }
            else
            {
                var xOffset = (position.X - LastMousePosition.X) * lookSensitivity;
                var yOffset = (position.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = position;

                Camera.mainCamera.CameraYaw += xOffset;
                Camera.mainCamera.CameraPitch -= yOffset;

                //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
                Camera.mainCamera.CameraPitch = Math.Clamp(Camera.mainCamera.CameraPitch, -89.0f, 89.0f);

                Camera.mainCamera.CameraDirection.X = MathF.Cos(MathHelper.DegreesToRadians(Camera.mainCamera.CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(Camera.mainCamera.CameraPitch));
                Camera.mainCamera.CameraDirection.Y = MathF.Sin(MathHelper.DegreesToRadians(Camera.mainCamera.CameraPitch));
                Camera.mainCamera.CameraDirection.Z = MathF.Sin(MathHelper.DegreesToRadians(Camera.mainCamera.CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(Camera.mainCamera.CameraPitch));
                Camera.mainCamera.CameraFront = Vector3.Normalize(Camera.mainCamera.CameraDirection);
            }
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
