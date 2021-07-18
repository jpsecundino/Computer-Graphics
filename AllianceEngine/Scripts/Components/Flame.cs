using System;
using System.Diagnostics;
using System.Numerics;

namespace AllianceEngine
{
    public class Flame: Light
    {
        private float intensityNoise;
        private Random rand;
        
        public Flame(Shader shader, Vector3 color, float radius, float ia, float il, float @is, float intensityNoise) : base(shader, color, radius, ia, il, @is)
        {
            this.intensityNoise = intensityNoise;
            rand = new Random();
        }

        public override void Update(double deltaTime)
        {
            
            float randNormal = RandNormal(0.1f, intensityNoise );

            Il = Math.Clamp( Il + (float) (2*rand.NextDouble() - 1) * randNormal, 0f, 1f);
            
            _shader.SetUniform($"uLights[{_idx}].Pos" , parent.Transform.Position);
            _shader.SetUniform($"uLights[{_idx}].Color" , Color);
            _shader.SetUniform($"uLights[{_idx}].Radius", Radius);
            _shader.SetUniform($"uLights[{_idx}].Ia", Ia );
            _shader.SetUniform($"uLights[{_idx}].Il", Il);
            _shader.SetUniform($"uLights[{_idx}].Is", Is);
            
        }

        private float RandNormal(float mean, float std)
        {
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            
            return (float) (mean + std * randStdNormal);
        }
    }
}