using System;
using System.Numerics;

namespace AllianceEngine
{
    public class LimitMovementVolume : Component
    {
        private readonly Vector3 center = Vector3.Zero;
        private readonly Vector3 dimensions = Vector3.One;
        

        public LimitMovementVolume(Vector3 center, float halfWidth, float halfHeight, float halfDepth)
        {
            this.center = center;
            dimensions = new Vector3(halfWidth, halfHeight, halfDepth);
        }

        public LimitMovementVolume(Vector3 center, Vector3 halfDimensions)
        {
            this.center = center;
            this.dimensions = halfDimensions;
        }

        public LimitMovementVolume(Vector3 halfDimensions)
        {
            this.dimensions = halfDimensions;
        }

        public override void Update(double deltaTime)
        {
            Vector3 position = parent.Transform.Position;

            // check x barrier
            if (position.X > center.X + dimensions.X)
                position.X = center.X + dimensions.X;
            else if(position.X < center.X - dimensions.X)
                position.X = center.X - dimensions.X;
            
            // check y barrier
            if (position.Y > center.Y + dimensions.Y)
                position.Y = center.Y + dimensions.Y;
            else if(position.Y < center.Y - dimensions.Y)
                position.Y = center.Y - dimensions.Y;
            
            // check z barrier
            if (position.Z > center.Z + dimensions.Z)
                position.Z = center.Z + dimensions.Z;
            else if(position.Z < center.Z - dimensions.Z)
                position.Z = center.Z - dimensions.Z;

            parent.Transform.Position = position;
        }
    }
}
