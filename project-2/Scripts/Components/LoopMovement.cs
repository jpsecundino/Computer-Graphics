using System;
using System.Numerics;
namespace World_3D
{
    public class LoopMovement : Component
    {
        private readonly float _speed;
        private readonly float _radius;
        private float angle;

        public LoopMovement(float speed = 5f, float radius = 3f)
        {
            _speed = speed;
            _radius = radius;
        }

        public override void Update(double deltaTime)
        {
            float scaledSpeed = _speed * (float)deltaTime;
            angle = (angle + scaledSpeed) % 360;


            Vector3 pos = parent.Transform.Position;
            pos.Z = MathF.Sin(angle) * _radius;
            pos.Y = MathF.Cos(angle) * _radius;
            parent.Transform.Position = pos;

            parent.Transform.Rotate(scaledSpeed, Vector3.UnitX);
        }
    }
}
