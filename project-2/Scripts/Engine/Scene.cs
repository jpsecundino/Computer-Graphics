using System.Collections.Generic;

namespace World_3D
{
    public class Scene
    {
        private readonly List<GameObject> gameObjects = new();
        private readonly RenderPipeline renderPipeline;

        public Scene(RenderPipeline _renderPipeline)
        {
            renderPipeline = _renderPipeline;
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        public void DrawObjects()
        {
            renderPipeline.DrawRenderers();
        }

        public void StartScene()
        {
            foreach (GameObject g in gameObjects)
            {
                g.Start();
            }
        }

        public void DrawHierarchy()
        {
            if (!ImGuiNET.ImGui.Begin("Hierarchy"))
            {
                ImGuiNET.ImGui.End();
                return;
            }

            foreach (GameObject g in gameObjects)
            {
                g.DrawInspector();
            }
            ImGuiNET.ImGui.End();
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
