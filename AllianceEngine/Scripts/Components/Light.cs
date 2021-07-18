using System.Numerics;
using SixLabors.ImageSharp.Processing;

namespace AllianceEngine
{
    public class Light: Component
    {
        private Shader _shader;
        private Vector3 _color;
        private uint _idx;
        private float _radius;

        public Light(Shader shader, Vector3 color, uint idx, float radius)
        {
            _shader = shader;
            this._color = color;
            _idx = idx;
            _radius = radius;
        }

        public override void Update(double deltaTime)
        {
            _shader.SetUniform("uLightColor" + _idx.ToString(), _color);
            _shader.SetUniform("uLightPos" + _idx.ToString(), parent.Transform.Position);
            _shader.SetUniform("uLightRadius" + _idx.ToString(), _radius);
        }
    }
}