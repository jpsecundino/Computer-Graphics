namespace World_3D
{
    public class FollowCamera : Component
    {
        public override void Update(double deltaTime)
        {
            parent.transform.Position = Camera.mainCamera.parent.transform.Position;
            parent.transform.Rotation = Camera.mainCamera.parent.transform.Rotation;
        }
    }
}
