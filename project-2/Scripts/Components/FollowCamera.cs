namespace World_3D
{
    public class FollowCamera : Component
    {
        public override void Update(double deltaTime)
        {
            parent.Transform.Position = Camera.MainCamera.parent.Transform.Position;
            parent.Transform.Rotation = Camera.MainCamera.parent.Transform.Rotation;
        }
    }
}
