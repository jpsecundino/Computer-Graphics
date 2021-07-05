using System;
using System.Collections.Generic;

namespace World_3D
{
    public class Renderer : Component
    {
        public static Action<Renderer> OnCreation;
        
        public List<Model3D> models = new();
        
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

            foreach(Model3D m in models)
            {
                m.Draw();
            }
        }
    }
}
