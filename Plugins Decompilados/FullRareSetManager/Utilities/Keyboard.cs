// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.Utilities.Keyboard
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

using System.Threading;
using System.Windows.Forms;

namespace FullRareSetManager.Utilities
{
  public static class Keyboard
  {
    private const int KEYEVENTF_EXTENDEDKEY = 1;
    private const int KEYEVENTF_KEYUP = 2;
    private const int ACTION_DELAY = 5;

    private static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    public static void KeyDown(Keys key)
    {
      int num = (int) Keyboard.keybd_event((byte) key, (byte) 0, 1, 0);
    }

    public static void KeyUp(Keys key)
    {
      int num = (int) Keyboard.keybd_event((byte) key, (byte) 0, 3, 0);
    }

    public static void KeyPress(Keys key)
    {
      Keyboard.KeyDown(key);
      Thread.Sleep(5);
      Keyboard.KeyUp(key);
    }
  }
}
