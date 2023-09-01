using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Hennesy_Crafting.Utils
{
    public static class Keyboard
    {
        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;
        private const int ACTION_DELAY = 5;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void KeyDown(Keys key)
        {
            int num = (int)keybd_event((byte)key, 0, 1, 0);
        }

        public static void KeyUp(Keys key)
        {
            int num = (int)keybd_event((byte)key, 0, 3, 0);
        }

        public static void KeyPress(Keys key)
        {
            KeyDown(key);
            Thread.Sleep(5);
            KeyUp(key);
        }
    }
}