using System.Numerics;

namespace AllianceEngine
{
    public class Material
    {
        public string Name { get; set; }

        public Vector3 ka;
        public Vector3 kd;
        public Vector3 ks;
                
        public int illum;

        public Texture texture;

        public Material(string name)
        {
            this.Name = name;
        }
        
        public Material(string name, Vector3 ka, Vector3 kd, Vector3 ks, int illum, Texture texture)
        {
            this.Name = name;
            this.ka = ka;
            this.kd = kd;
            this.ks = ks;
            this.illum = illum;
            this.texture = texture;
        }

        public void Bind(Shader shader)
        {
            shader.SetUniform("uKa", ka);
            texture.Bind();
        }

    }
}