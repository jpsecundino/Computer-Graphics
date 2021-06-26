using System.Numerics;

namespace World_3D
{
    public class Rotator : Component
    {
        private float _speed = 10f;

        public Rotator(float speed = 10f)
        {
            _speed = speed;
        }

        public override void Update(double deltaTime)
        {
            if(Input.Keyboard.IsKeyPressed(Silk.NET.Input.Key.Left))
                parent.transform.Rotate(_speed * (float) deltaTime, Vector3.UnitY);
            if(Input.Keyboard.IsKeyPressed(Silk.NET.Input.Key.Right))
                parent.transform.Rotate(-_speed * (float) deltaTime, Vector3.UnitY);
        }
    }
}
