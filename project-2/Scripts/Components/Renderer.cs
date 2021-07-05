using System;
using System.Collections.Generic;

namespace World_3D
{
    public class Renderer : Component
    {
        public static Action<Renderer> OnCreation;
        
        private readonly Mesh model;
        private readonly Shader shader;

        public Renderer(ModelType modelType, Shader s)
        {
            shader = s;

            model = ModelManager.GetModel(modelType);

            OnCreation(this);
        }

        public void Draw()
        {
            shader.SetUniform("uModel", parent.Transform.ModelMatrix);
            model.Draw();
        }
    }
}
