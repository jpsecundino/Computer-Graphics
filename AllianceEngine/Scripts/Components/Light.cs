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
            _shader.SetUniform($"uLights[{_idx}].Color" , _color);
            _shader.SetUniform($"uLights[{_idx}].Pos" , parent.Transform.Position);
            _shader.SetUniform($"uLights[{_idx}].Radius", _radius);
        }
    }
}