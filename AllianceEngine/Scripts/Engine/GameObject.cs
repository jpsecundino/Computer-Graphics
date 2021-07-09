using System;
using ImGuiNET;
using System.Collections.Generic;
using System.Numerics;

namespace AllianceEngine
{
    public class GameObject
    {
        public readonly string Name = "";
        private readonly Guid _guid;
        public Transform Transform { get; set; } = new();

        private readonly List<Component> components = new();

        public GameObject(string name)
        {
            this.Name = name;
            this._guid = Guid.NewGuid();
        }

        public void AddComponent(Component c)
        {
            c.parent = this;
            components.Add(c);
        }

        public void Update(double deltaTime)
        {
            foreach(Component c in components)
            {
                c.Update(deltaTime);
            }
        }

        public void DrawInspector()
        {
            if(ImGui.CollapsingHeader(Name))
            {
                ImGui.PushID(_guid.ToString());

                ImGui.Text(nameof(Transform));
                ImGui.Separator();
                Vector3 position = Transform.Position;
                Vector3 rotation = Transform.Rotation * MathHelper.RadiansToDegrees;
                Vector3 scale = Transform.Scale;
                
                ImGui.DragFloat3("Position", ref position);
                ImGui.DragFloat3("Rotation", ref rotation);
                ImGui.DragFloat3("Scale", ref scale);

                Transform.Position = position;
                Transform.Rotation = rotation;
                Transform.Scale = scale;
                Transform.Rotation = rotation * MathHelper.DegreesToRadians;
                
                ImGui.PopID();
            }
        }

        internal void Start()
        {
            foreach (Component c in components)
            {
                c.Start();
            }
        }
    }
}
