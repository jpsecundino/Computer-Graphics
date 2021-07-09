using System;
using System.Numerics;
using Silk.NET.Input;

namespace AllianceEngine
{
    public class Transform
    {
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;

        public Vector3 Forward { get; set; } = Vector3.UnitZ;
        public Vector3 Up { get; set; } = Vector3.UnitY;

        //Note: The order here does matter.
        public Matrix4x4 ModelMatrix => Matrix4x4.Identity * Matrix4x4.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) * Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateTranslation(Position);

        public void Rotate(float angle, Vector3 axis)
        {
            Rotation += axis * angle;
        }
        
    }
}