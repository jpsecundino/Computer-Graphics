using Silk.NET.Input;
using Silk.NET.Windowing;

namespace AllianceEngine
{
    public static class Input
    {
        public static IKeyboard Keyboard { get => keyboard; private set => keyboard = value; }
        public static IMouse Mouse { get => mouse; private set => mouse = value; }
        public static IInputContext InputContext { get => inputContext; private set => inputContext = value; }

        private static IKeyboard keyboard;
        private static IMouse mouse;
        private static IInputContext inputContext;

        public static void Initialize(IView window)
        {
            InputContext = window.CreateInput();
            Keyboard = InputContext.Keyboards[0];
            Mouse = InputContext.Mice[0];
        }

    }
}
