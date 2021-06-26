namespace World_3D
{
    public abstract class Component
    {
        public GameObject parent;

        public virtual void Start() { }
        public virtual void Update(double deltaTime) { }
        public virtual void Destroy() { }

    }
}
