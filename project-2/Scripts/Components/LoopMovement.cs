using System;
using System.Numerics;
namespace AllianceEngine
{
    public class LoopMovement : Component
    {
        public enum LoopType
        {
            YZ,
            XZ
        }

        private readonly float _speed;
        private readonly float _radius;

        private readonly Action<float> Rotate;
        private Vector3 _initialPos = Vector3.Zero;
        private float _angle = 0f;

        public LoopMovement(float speed = 5f, float radius = 3f, LoopType type = LoopType.XZ)
        {
            _speed = speed;
            _radius = radius;

            switch (type)
            {
                case LoopType.YZ:
                    Rotate = RotateAroundYZ;
                    break;
                case LoopType.XZ:
                    Rotate = RotateAroundXZ;
                    break;
            }
        }

        public override void Start()
        {
            _initialPos = parent.Transform.Position;
        }

        public override void Update(double deltaTime)
        {
            float scaledSpeed = _speed * (float)deltaTime;
            _angle = (_angle + scaledSpeed) % 360;
            Rotate(scaledSpeed);
        }

        private void RotateAroundYZ(float scaledSpeed)
        {
            Vector3 pos = parent.Transform.Position;
            pos.Z = _initialPos.Z + MathF.Sin(_angle) * _radius;
            pos.Y = _initialPos.Y + MathF.Cos(_angle) * _radius;
            parent.Transform.Position = pos;

            parent.Transform.Rotate(scaledSpeed, Vector3.UnitX);
        }

        private void RotateAroundXZ(float scaledSpeed)
        {
            Vector3 pos = parent.Transform.Position;
            pos.Z = _initialPos.Z + MathF.Sin(_angle) * _radius;
            pos.X = _initialPos.X + MathF.Cos(_angle) * _radius;
            parent.Transform.Position = pos;

            parent.Transform.Rotate(scaledSpeed, -Vector3.UnitX);
        }
    }
}
