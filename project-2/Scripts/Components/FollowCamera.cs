namespace World_3D
{
    public class FollowCamera : Component
    {
        public override void Update(double deltaTime)
        {
            parent.Transform.Position = Camera.mainCamera.parent.Transform.Position;
            parent.Transform.Rotation = Camera.mainCamera.parent.Transform.Rotation;
        }
    }
}
