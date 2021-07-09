using System.Numerics;
using Silk.NET.Input;

namespace AllianceEngine
{
    public static class UI
    {
        public static bool IsUIOpen = false;

        public static void Initialize()
        {
            Input.Mouse.Click += MouseControl;    
            Input.Mouse.Cursor.CursorMode = CursorMode.Disabled;
        }
        
        private static void MouseControl(IMouse mouseIdx, MouseButton mouseButton, Vector2 position)
        {
            if (mouseButton != MouseButton.Right) return;

            IsUIOpen = !IsUIOpen;
                
            Input.Mouse.Cursor.CursorMode = IsUIOpen ? CursorMode.Normal : CursorMode.Disabled;

        }
    }
}