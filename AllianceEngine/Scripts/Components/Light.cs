using System.Numerics;
using SixLabors.ImageSharp.Processing;

namespace AllianceEngine
{
    public class Light: Component
    {
        private Shader _shader;
        private Vector3 _color;
        private uint _idx;

        public Light(Shader shader, Vector3 color, uint idx)
        {
            _shader = shader;
            this._color = color;
            _idx = idx;
        }

        public override void Update(double deltaTime)
        {
            _shader.SetUniform("uLightColor" + _idx.ToString(), _color);
            _shader.SetUniform("uLightPos" + _idx.ToString(), parent.Transform.Position);
        }
    }
}