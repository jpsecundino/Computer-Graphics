using Silk.NET.Input;
using System;
using System.Numerics;
using Tutorial;

namespace World_3D
{
    class CameraMovement: Component
    {
        private Vector2 LastMousePosition;
        public float CameraYaw = -90f;
        public float CameraPitch = 0f;

        public override void Start()
        {
            Input.Mouse.Cursor.CursorMode = CursorMode.Raw;
        }

        public override void Update(double deltaTime)
        {
            var moveSpeed = 10f * (float)deltaTime;

            MoveCamera(moveSpeed);
            RotateCamera();
        }
        private unsafe void RotateCamera()
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

                Rotate(CameraYaw, CameraPitch);
            }
        }

        public void Rotate(float yaw = 0f, float pitch = 0f, float roll = 0f)
        {
            Vector3 dir = new();

            dir.X = MathF.Cos(MathHelper.DegreesToRadians(yaw)) * MathF.Cos(MathHelper.DegreesToRadians(pitch));
            dir.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            dir.Z = MathF.Sin(MathHelper.DegreesToRadians(yaw)) * MathF.Cos(MathHelper.DegreesToRadians(pitch));

            parent.Transform.Forward = Vector3.Normalize(dir);
        }

        private void MoveCamera(float moveSpeed)
        {
            if (Input.Keyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                parent.Transform.Position += moveSpeed * parent.Transform.Forward;
            }
            if (Input.Keyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                parent.Transform.Position -= moveSpeed * parent.Transform.Forward;
            }
            if (Input.Keyboard.IsKeyPressed(Key.A))
            {
                //Move left
                parent.Transform.Position -= Vector3.Normalize(Vector3.Cross(parent.Transform.Forward, parent.Transform.Up)) * moveSpeed;
            }
            if (Input.Keyboard.IsKeyPressed(Key.D))
            {
                //Move right
                parent.Transform.Position += Vector3.Normalize(Vector3.Cross(parent.Transform.Forward, parent.Transform.Up)) * moveSpeed;
            }
        }

    }
}
