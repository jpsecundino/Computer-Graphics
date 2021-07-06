using System;
using System.Numerics;
namespace World_3D
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
        private Transform _transform;
        
        private readonly Action<float> Rotate;
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
            _transform = parent.Transform;
        }

        public override void Update(double deltaTime)
        {
            float scaledSpeed = _speed * (float)deltaTime;
            _angle = (_angle + scaledSpeed) % 360;
            Rotate(scaledSpeed);
        }

        private void RotateAroundYZ(float scaledSpeed)
        {
            Vector3 pos = _transform.Position;
            pos.Z = MathF.Sin(_angle) * _radius;
            pos.Y = MathF.Cos(_angle) * _radius;
            _transform.Position = pos;

            _transform.Rotate(scaledSpeed, Vector3.UnitX);
        }

        private void RotateAroundXZ(float scaledSpeed)
        {
            Vector3 pos = _transform.Position;
            pos.Z = MathF.Sin(_angle) * _radius;
            pos.X = MathF.Cos(_angle) * _radius;
            _transform.Position = pos;

            _transform.Rotate(scaledSpeed, Vector3.UnitY);
        }
    }
}
