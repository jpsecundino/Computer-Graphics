using Silk.NET.Input;
using Silk.NET.Windowing;
using System.Linq;

namespace World_3D
{
    public static class Input
    {
        public static IKeyboard Keyboard;
        public static IMouse Mouse;
        public static IInputContext InputContext;
        
        public static void Initialize(IView window)
        {
            InputContext = window.CreateInput();
            Keyboard = InputContext.Keyboards.FirstOrDefault();
            Mouse = InputContext.Mice.FirstOrDefault();
        }

    }
}
