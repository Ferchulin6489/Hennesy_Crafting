using SharpDX;
using System.Runtime.InteropServices;
using System.Threading;

namespace Hennesy_Crafting.Utils
{
    public class Mouse
    {
        public const int MOUSEEVENTF_LEFTDOWN = 2;
        public const int MOUSEEVENTF_LEFTUP = 4;
        public const int MOUSEEVENTF_MIDDOWN = 32;
        public const int MOUSEEVENTF_MIDUP = 64;
        public const int MOUSEEVENTF_RIGHTDOWN = 8;
        public const int MOUSEEVENTF_RIGHTUP = 16;
        public const int MOUSE_EVENT_WHEEL = 2048;
        private const int MOVEMENT_DELAY = 10;
        private const int CLICK_DELAY = 1;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern void mouse_event(
          int dwFlags,
          int dx,
          int dy,
          int cButtons,
          int dwExtraInfo);

        public static POINT GetCursorPosition()
        {
            POINT lpPoint;
            Mouse.GetCursorPos(out lpPoint);
            return lpPoint;
        }

        public static void SetCursorPos(Vector2 pos) => Mouse.SetCursorPos((int)pos.X, (int)pos.Y);

        public static void SetCursorPos(POINT pos) => Mouse.SetCursorPos(pos.X, pos.Y);

        public static void SetCursorPosAndLeftClick(Vector2 coords, int extraDelay)
        {
            Mouse.SetCursorPos((int)coords.X, (int)coords.Y);
            Thread.Sleep(10 + extraDelay);
            Mouse.mouse_event(2, 0, 0, 0, 0);
            Thread.Sleep(1);
            Mouse.mouse_event(4, 0, 0, 0, 0);
        }

        public static void SetCursorPosAndLeftClick(int xpos, int ypos, int extraDelay)
        {
            Mouse.SetCursorPos(xpos, ypos);
            Thread.Sleep(10);
            Mouse.LeftClick();
        }

        private static void LeftClick()
        {
            Mouse.LeftMouseDown();
            Thread.Sleep(1);
            Mouse.LeftMouseUp();
        }

        public static void LeftMouseDown() => Mouse.mouse_event(2, 0, 0, 0, 0);

        public static void LeftMouseUp() => Mouse.mouse_event(4, 0, 0, 0, 0);

        public static void RightMouseDown() => Mouse.mouse_event(8, 0, 0, 0, 0);

        public static void RightMouseUp() => Mouse.mouse_event(16, 0, 0, 0, 0);

        public static void VerticalScroll(bool forward, int clicks)
        {
            if (forward)
                Mouse.mouse_event(2048, 0, 0, clicks * 120, 0);
            else
                Mouse.mouse_event(2048, 0, 0, -(clicks * 120), 0);
        }
    }
}
