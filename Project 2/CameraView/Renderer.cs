using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World_3D.CameraView
{
    public class Renderer : Component
    {
        public static Action<Renderer> OnCreation;
        
        public List<Mesh> meshes = new();
        
        Shader shader;

        public Renderer(MeshType[] meshTypes, Shader s)
        {
            shader = s;

            foreach(MeshType m in meshTypes)
            {
                meshes.Add(MeshManager.GetMesh(m));
            }

            OnCreation(this);
        }

        public void Draw()
        {
            shader.SetUniform("uModel", parent.transform.ModelMatrix);

            foreach(Mesh m in meshes)
            {
                m.Draw(shader);
            }
        }
    }
}
