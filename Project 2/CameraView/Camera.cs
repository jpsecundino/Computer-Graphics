using Silk.NET.Input;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tutorial;

namespace World_3D.CameraView
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

        public Matrix4x4 View => Matrix4x4.CreateLookAt(parent.transform.Position, parent.transform.Position + CameraFront, CameraUp);
        public Matrix4x4 Projection => Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(CameraZoom), Tutorial.Program.Width / Tutorial.Program.Height, 0.1f, 100.0f);

        public override void Update(double deltaTime)
        {
            var moveSpeed = 10f * (float) deltaTime;

            if (Tutorial.Program.primaryKeyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                parent.transform.Position += moveSpeed * CameraFront;
            }
            if (Tutorial.Program.primaryKeyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                parent.transform.Position -= moveSpeed * CameraFront;
            }
            if (Tutorial.Program.primaryKeyboard.IsKeyPressed(Key.A))
            {
                //Move left
                parent.transform.Position -= Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp)) * moveSpeed;
            }
            if (Tutorial.Program.primaryKeyboard.IsKeyPressed(Key.D))
            {
                //Move right
                parent.transform.Position += Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp)) * moveSpeed;
            }
            if (Tutorial.Program.primaryKeyboard.IsKeyPressed(Key.L))
            {
                Tutorial.Program.Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (Tutorial.Program.primaryKeyboard.IsKeyPressed(Key.P))
            {
                Tutorial.Program.Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
        }
 
    }
}
