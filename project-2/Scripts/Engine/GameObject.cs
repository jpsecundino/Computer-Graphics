using System;
using System.Collections.Generic;
using Tutorial;

namespace World_3D
{
    public class GameObject
    {
        public readonly string Name = "";
        public Transform Transform { get; set; } = new();

        private readonly List<Component> components = new();

        public GameObject(string name)
        {
            this.Name = name;
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

        internal void Start()
        {
            foreach (Component c in components)
            {
                c.Start();
            }
        }
    }
}
