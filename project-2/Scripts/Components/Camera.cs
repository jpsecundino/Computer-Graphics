using System.Numerics;

namespace World_3D
{
    public class Camera : Component
    {
        public static Camera MainCamera { get => mainCamera; }
        private static Camera mainCamera = null;
        
        public float FarPlaneDistance { get; private set; } = 100.0f;
        public float NearPlaneDistance { get; private set; } = 0.1f;
        public float CameraZoom { get; set; } = 45f;

        public Matrix4x4 View => Matrix4x4.CreateLookAt(parent.Transform.Position, parent.Transform.Position
                                                                                   + parent.Transform.Forward, parent.Transform.Up);
        public Matrix4x4 Projection => Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians * CameraZoom, Program.Width / Program.Height, NearPlaneDistance, FarPlaneDistance);


        public static void SwitchMainCamera(Camera camera)
        {
            mainCamera = camera;
        }

    }
}
