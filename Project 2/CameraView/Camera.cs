using System.Numerics;
using Tutorial;

namespace World_3D
{
    public class Camera : Component
    {
        public float FarPlaneDistance { get; private set; } = 100.0f;
        public float NearPlaneDistance { get; private set; } = 0.1f;

        public static Camera mainCamera;

        //Setup the camera's location, directions, and movement speed
        public float CameraZoom = 45f;

        public Matrix4x4 View => Matrix4x4.CreateLookAt(parent.transform.Position, parent.transform.Position + parent.transform.Forward, parent.transform.Up);
        public Matrix4x4 Projection => Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(CameraZoom), Program.Width / Program.Height, NearPlaneDistance, FarPlaneDistance);
    }
}
