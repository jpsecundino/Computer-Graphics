using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World_3D.CameraView
{
    public class Scene
    {
        private List<GameObject> gameObjects = new();
        private RenderPipeline renderPipeline;
        private Shader shader;

        public Scene(RenderPipeline _renderPipeline, Shader _shader)
        {
            renderPipeline = _renderPipeline;
            shader = _shader;
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        public void DrawObjects()
        {
            renderPipeline.DrawRenderers();
        }

        public void UpdateScene(double deltaTime)
        {
            foreach(GameObject g in gameObjects)
            {
                g.Update(deltaTime);
            }
        }

    }
}
