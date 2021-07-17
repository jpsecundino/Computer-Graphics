using System.Numerics;

namespace AllianceEngine
{
    public struct Material
    {
        public string name;
        
        public Vector3 ka;
        public Vector3 kd;
        public Vector3 ks;
                
        public int illum;

        public Texture texture;

        public Material(string name) : this()
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

    }
}