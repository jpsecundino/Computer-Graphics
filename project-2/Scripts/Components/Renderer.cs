using System;
using System.Collections.Generic;

namespace World_3D
{
    public class Renderer : Component
    {
        public static Action<Renderer> OnCreation;
        
        public List<Mesh> models = new();
        
        Shader shader;

        public Renderer(ModelType[] modelTypes, Shader s)
        {
            shader = s;

            foreach(ModelType m in modelTypes)
            {
                models.Add(ModelManager.GetModel(m));
            }

            OnCreation(this);
        }

        public void Draw()
        {
            shader.SetUniform("uModel", parent.Transform.ModelMatrix);

            foreach(Mesh m in models)
            {
                m.Draw();
            }
        }
    }
}
