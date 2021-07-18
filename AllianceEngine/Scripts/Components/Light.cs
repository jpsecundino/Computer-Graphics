using System.Numerics;
using SixLabors.ImageSharp.Processing;

namespace AllianceEngine
{
    public class Light: Component
    {
        private Shader _shader;

        public Light(Shader shader)
        {
            _shader = shader;
        }

        public override void Update(double deltaTime)
        {

            _shader.SetUniform("uLightPos", parent.Transform.Position);

        }
    }
}