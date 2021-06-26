using System.Numerics;

namespace World_3D
{
    public class Transform
    {
        //A transform abstraction.
        //For a transform we need to have a position, a scale, and a rotation,
        //depending on what application you are creating, the type for these may vary.

        //Here we have chosen a vec3 for position, float for scale and quaternion for rotation,
        //as that is the most normal to go with.
        //Another example could have been vec3, vec3, vec4, so the rotation is an axis angle instead of a quaternion

        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);

        public Vector3 Forward { get; set; } = new Vector3(0.0f, 0.0f, -1.0f);

        public Vector3 Up { get; set; } = Vector3.UnitY;

        public float Scale { get; set; } = 1f;

        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        //Note: The order here does matter.
        public Matrix4x4 ModelMatrix => Matrix4x4.Identity * Matrix4x4.CreateFromQuaternion(Rotation) * Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateTranslation(Position);

        public void Rotate(float yaw = 0f, float pitch = 0f, float roll = 0f)
        {
            Rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
            Forward = RotateVec3(Forward, Rotation);
        }

        private Vector3 RotateVec3(Vector3 v, Quaternion q)
        {
            Matrix4x4 rotMat = Matrix4x4.CreateFromQuaternion(q);
            return Vector3.Transform(v, rotMat);
        }
    }
}
