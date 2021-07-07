namespace World_3D
{
    public class Skybox : Component
    {
        public override void Start()
        {
            parent.Transform.Scale = new System.Numerics.Vector3(Camera.MainCamera.FarPlaneDistance / 2f);
        }

        public override void Update(double deltaTime)
        {
            parent.Transform.Position = Camera.MainCamera.parent.Transform.Position;
        }
    }
}
