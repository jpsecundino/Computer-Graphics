﻿using System;
using System.Numerics;
using Silk.NET.Input;
using SixLabors.ImageSharp.Processing;

namespace AllianceEngine
{
    public class Light: Component
    {
        private Shader _shader;
        
        private Vector3 _color;
        private float _ia;
        private float _il;
        private float _is;
        
        private static uint indexes = 0;
        private uint _idx;
        private float _radius;

        public Light(Shader shader, Vector3 color, float radius, float ia, float il, float @is)
        {
            _shader = shader;
            this._color = color;
            _idx = indexes++;
            _radius = radius;
            _ia = ia;
            _il = il;
            _is = @is;
        }

        public override void Update(double deltaTime)
        {
            _shader.SetUniform($"uLights[{_idx}].Pos" , parent.Transform.Position);
            _shader.SetUniform($"uLights[{_idx}].Color" , _color);
            _shader.SetUniform($"uLights[{_idx}].Radius", _radius);
            _shader.SetUniform($"uLights[{_idx}].Ia", _ia);
            _shader.SetUniform($"uLights[{_idx}].Il", _il);
            _shader.SetUniform($"uLights[{_idx}].Is", _is);
            
            ControlIntensity();

        }

        private void ControlIntensity()
        {

            if(_idx != 0) return;
            
            float delta = 0.02f;
            
            if (Input.Keyboard.IsKeyPressed(Key.Up))
            {
                //increase ambient light intensity
                _ia = Math.Clamp(_ia + delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.Down))
            {
                //decrease ambient light intensity
                _ia = Math.Clamp(_ia - delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.Right))
            {
                //decrease ambient light intensity
                _il = Math.Clamp(_il + delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.Left))
            {
                //decrease ambient light intensity
                _il = Math.Clamp(_il - delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.ShiftRight))
            {
                //decrease ambient light intensity
                _is = Math.Clamp(_is + delta, 0f, 1f);
            }
            if (Input.Keyboard.IsKeyPressed(Key.ControlRight))
            {
                //decrease ambient light intensity
                _is = Math.Clamp(_is - delta, 0f, 1f);
            }

        }
    }
}