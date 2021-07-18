using System.Numerics;

namespace AllianceEngine
{
    public class Material
    {
        public string name;
        
        public Vector3 ka;
        public Vector3 kd;
        public Vector3 ks;
        public float ns;
                
        public int illum;

        public Texture texture;

        public Material(string name)
        {
            this.name = name;
        }
        
        public Material(string name, Vector3 ka, Vector3 kd, Vector3 ks, int illum, Texture texture)
        {
            this.name = name;
            this.ka = ka;
            this.kd = kd;
            this.ks = ks;
            this.illum = illum;
            this.texture = texture;
        }

        public void Bind()
        {
            Program.Shader.SetUniform("uMaterial.Ka", ka);
            Program.Shader.SetUniform("uMaterial.Kd", kd);
            Program.Shader.SetUniform("uMaterial.Ks", ks);
            Program.Shader.SetUniform("uMaterial.Ns", ns);

            texture?.Bind();
        }

    }
}