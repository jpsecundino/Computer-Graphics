using System.Numerics;

namespace World_3D
{
    public class Transform
    {
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public float Scale { get; set; } = 1f;

        public Vector3 Forward { get; set; }
        public Vector3 Up { get; set; } = Vector3.UnitY;

        //Note: The order here does matter.
        public Matrix4x4 ModelMatrix => Matrix4x4.Identity * Matrix4x4.CreateFromQuaternion(Rotation) * Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateTranslation(Position);

        public void Rotate(float angle, Vector3 axis)
        {
            var q = Quaternion.CreateFromAxisAngle(axis, angle);
            Rotation = q * Rotation;
        }
    }
}
