using Silk.NET.Input;
using System;

namespace AllianceEngine
{
    public class CameraZoom
    {
        private const float maxZoom = 45f;
        private const float minZoom = 1.0f;
        
        private readonly Camera camera;

        public CameraZoom(Camera camera)
        {
            this.camera = camera;
        }

        public void ZoomControl(ScrollWheel scrollWheel)
        {
            //Clamp zoom
            camera.CameraZoom = Math.Clamp(camera.CameraZoom - scrollWheel.Y, minZoom, maxZoom);
        }
        
    }
}
