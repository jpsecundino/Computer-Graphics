using System.Numerics;
using SixLabors.ImageSharp.Processing;

namespace AllianceEngine
{
    public class Light: Component
    {
        private Shader _shader;
        private Vector3 _color;

        public Light(Shader shader, Vector3 color)
        {
            _shader = shader;
            this._color = color;
        }

        public override void Update(double deltaTime)
        {
            _shader.SetUniform("uLightColor", _color);
            _shader.SetUniform("uLightPos", parent.Transform.Position);
        }
    }
}