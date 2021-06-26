using Silk.NET.Input;
using Silk.NET.OpenGL;
using System;
using System.Numerics;
using Tutorial;

namespace World_3D
{
    public class Camera : Component
    {
        public static Camera mainCamera;


        //Setup the camera's location, directions, and movement speed
        public Vector3 CameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        public Vector3 CameraUp = Vector3.UnitY;
        public Vector3 CameraDirection = Vector3.Zero;
        public float CameraYaw = -90f;
        public float CameraPitch = 0f;
        public float CameraZoom = 45f;
        private Vector2 LastMousePosition;


        public Matrix4x4 View => Matrix4x4.CreateLookAt(parent.transform.Position, parent.transform.Position + CameraFront, CameraUp);
        public Matrix4x4 Projection => Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(CameraZoom), Program.Width / Program.Height, 0.1f, 100.0f);

        public override void Start()
        {
            Input.Mouse.Cursor.CursorMode = CursorMode.Raw;
        }

        public override void Update(double deltaTime)
        {
            var moveSpeed = 10f * (float)deltaTime;

            MoveCamera(moveSpeed);
            OnMouseMove();
        }

        private unsafe void OnMouseMove()
        {
            var lookSensitivity = 0.1f;

            Vector2 mousePos = Input.Mouse.Position;

            if (LastMousePosition == default)
            {
                LastMousePosition = mousePos;
            }
            else
            {
                var xOffset = (mousePos.X - LastMousePosition.X) * lookSensitivity;
                var yOffset = (mousePos.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = mousePos;

                CameraYaw += xOffset;
                CameraPitch -= yOffset;

                //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
                CameraPitch = Math.Clamp(CameraPitch, -89.0f, 89.0f);

                CameraDirection.X = MathF.Cos(MathHelper.DegreesToRadians(CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(CameraPitch));
                CameraDirection.Y = MathF.Sin(MathHelper.DegreesToRadians(CameraPitch));
                CameraDirection.Z = MathF.Sin(MathHelper.DegreesToRadians(CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(CameraPitch));
                CameraFront = Vector3.Normalize(CameraDirection);
            }
        }

        private void MoveCamera(float moveSpeed)
        {
            if (Input.Keyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                parent.transform.Position += moveSpeed * CameraFront;
            }
            if (Input.Keyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                parent.transform.Position -= moveSpeed * CameraFront;
            }
            if (Input.Keyboard.IsKeyPressed(Key.A))
            {
                //Move left
                parent.transform.Position -= Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp)) * moveSpeed;
            }
            if (Input.Keyboard.IsKeyPressed(Key.D))
            {
                //Move right
                parent.transform.Position += Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp)) * moveSpeed;
            }
        }
    }
}
