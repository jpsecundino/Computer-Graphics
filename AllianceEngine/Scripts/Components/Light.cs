using System;
using System.Numerics;
using Silk.NET.Input;
using SixLabors.ImageSharp.Processing;

namespace AllianceEngine
{
    public class Light: Component
    {
        private static uint indexes = 0;
        protected readonly uint _idx;
        
        protected readonly Shader _shader;

        protected Vector3 Color { get; set; }
        protected float Ia { get; set; }
        protected float Il { get; set; }
        protected float Is { get; set; }

        public Light(Shader shader, Vector3 color, float ia, float il, float @is)
        {
            _shader = shader;
            this.Color = color;
            _idx = indexes++;
            Ia = ia;
            Il = il;
            Is = @is;
        }

        public override void Update(double deltaTime)
        {
            _shader.SetUniform($"uLights[{_idx}].Pos" , parent.Transform.Position);
            _shader.SetUniform($"uLights[{_idx}].Color" , Color);
            _shader.SetUniform($"uLights[{_idx}].Ia", Ia);
            _shader.SetUniform($"uLights[{_idx}].Il", Il);
            _shader.SetUniform($"uLights[{_idx}].Is", Is);
            
            ControlIntensity();

        }

        private void ControlIntensity()
        {

            if(_idx != 0) return;
            
            float delta = 0.02f;
            
            if (Input.Keyboard.IsKeyPressed(Key.Up))
            {
                //increase ambient light intensity
                Ia = Math.Clamp(Ia + delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.Down))
            {
                //decrease ambient light intensity
                Ia = Math.Clamp(Ia - delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.Right))
            {
                //decrease ambient light intensity
                Il = Math.Clamp(Il + delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.Left))
            {
                //decrease ambient light intensity
                Il = Math.Clamp(Il - delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.ShiftRight))
            {
                //decrease ambient light intensity
                Is = Math.Clamp(Is + delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.ControlRight))
            {
                //decrease ambient light intensity
                Is = Math.Clamp(Is - delta, 0f, 1f);
            }

        }
    }
}