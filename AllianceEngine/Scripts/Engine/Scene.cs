using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;

namespace AllianceEngine
{
    public class Scene
    {
        private readonly List<GameObject> gameObjects = new();
        private readonly RenderPipeline renderPipeline;
        private ImGuiWindowFlags _imGuiWindowFlags = ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoMove;

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
            if (!UI.IsUIOpen) return;
            
            if (!ImGuiNET.ImGui.Begin("Hierarchy",_imGuiWindowFlags))
            {
                ImGuiNET.ImGui.End();
                return;
            }
            
            ImGuiNET.ImGui.SetWindowPos(new Vector2(0,0));

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
