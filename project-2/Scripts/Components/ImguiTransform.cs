using System;
using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;

namespace World_3D
{
    public class ImguiTransform: Component
    {
        public override void Update(double deltaTime)
        {
            ImGui.Begin("Transform of " + parent.Name );

            Vector3 position = parent.Transform.Position;
            // float scale = parent.Transform.Scale;
            
            ImGui.DragFloat3("Position", ref position);
            // ImGui.DragFloat("transform", ref scale);

            parent.Transform.Position = position;
            // parent.Transform.Scale = scale;
            
            ImGui.End();
        }
    }
}