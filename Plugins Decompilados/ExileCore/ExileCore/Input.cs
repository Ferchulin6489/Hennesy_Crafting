// Decompiled with JetBrains decompiler
// Type: ExileCore.Input
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared;
using ExileCore.Shared.Helpers;
using MoreLinq.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Vanara.PInvoke;

namespace ExileCore
{
  public class Input
  {
    private const int ACTION_DELAY = 1;
    private const int KEY_PRESS_DELAY = 10;
    public const int MOUSEEVENTF_MOVE = 1;
    public const int MOUSEEVENTF_LEFTDOWN = 2;
    public const int MOUSEEVENTF_LEFTUP = 4;
    public const int MOUSEEVENTF_MIDDOWN = 32;
    public const int MOUSEEVENTF_MIDUP = 64;
    public const int MOUSEEVENTF_RIGHTDOWN = 8;
    public const int MOUSEEVENTF_RIGHTUP = 16;
    public const int MOUSE_EVENT_WHEEL = 2048;
    private static readonly Dictionary<System.Windows.Forms.Keys, bool> Keys = new Dictionary<System.Windows.Forms.Keys, bool>();
    private static readonly HashSet<System.Windows.Forms.Keys> RegisteredKeys = new HashSet<System.Windows.Forms.Keys>();
    private static readonly object locker = new object();
    private static readonly WaitTime cursorPositionSmooth = new WaitTime(1);
    private static readonly WaitTime keyPress = new WaitTime(1);
    private static readonly Dictionary<System.Windows.Forms.Keys, bool> KeysPressed = new Dictionary<System.Windows.Forms.Keys, bool>();
    private static readonly Stopwatch sw = Stopwatch.StartNew();

    static Input()
    {
      foreach (System.Windows.Forms.Keys key in Enum.GetValues<System.Windows.Forms.Keys>())
        Input.KeysPressed[key] = false;
    }

    [Obsolete]
    public static SharpDX.Vector2 ForceMousePosition => (SharpDX.Vector2) WinApi.GetCursorPositionPoint();

    public static System.Numerics.Vector2 ForceMousePositionNum => WinApi.GetCursorPositionPoint().ToVector2();

    [Obsolete]
    public static SharpDX.Vector2 MousePosition => Input.MousePositionNum.ToSharpDx();

    public static System.Numerics.Vector2 MousePositionNum { get; private set; }

    public static bool IsKeyDown(int nVirtKey) => Input.IsKeyDown((System.Windows.Forms.Keys) nVirtKey);

    public static bool IsKeyDown(System.Windows.Forms.Keys nVirtKey)
    {
      if (nVirtKey == System.Windows.Forms.Keys.None)
        return false;
      if (!Input.Keys.ContainsKey(nVirtKey))
        Input.RegisterKey(nVirtKey);
      return Input.Keys[nVirtKey];
    }

    public static bool GetKeyState(System.Windows.Forms.Keys key) => key != System.Windows.Forms.Keys.None && WinApi.GetKeyState(key) < (short) 0;

    public static void RegisterKey(System.Windows.Forms.Keys key)
    {
      bool lockTaken;
      if (key == System.Windows.Forms.Keys.None || Input.Keys.TryGetValue(key, out lockTaken))
        return;
      object locker = Input.locker;
      lockTaken = false;
      try
      {
        Monitor.Enter(locker, ref lockTaken);
        Input.Keys[key] = false;
        Input.RegisteredKeys.Add(key);
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(locker);
      }
    }

    public static event EventHandler<System.Windows.Forms.Keys> ReleaseKey;

    public static void Update(IntPtr? windowPtr)
    {
      if (windowPtr.HasValue)
        Input.MousePositionNum = WinApi.GetCursorPosition(windowPtr.Value).ToVector2();
      try
      {
        Input.RegisteredKeys.ForEach<System.Windows.Forms.Keys>((Action<System.Windows.Forms.Keys>) (key =>
        {
          bool keyState = Input.GetKeyState(key);
          if (!keyState && Input.Keys[key])
          {
            EventHandler<System.Windows.Forms.Keys> releaseKey = Input.ReleaseKey;
            if (releaseKey != null)
              releaseKey((object) null, key);
          }
          Input.Keys[key] = keyState;
        }));
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
        interpolatedStringHandler.AppendFormatted(nameof (Input));
        interpolatedStringHandler.AppendLiteral(" ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear());
      }
    }

    [Obsolete]
    public static IEnumerator SetCursorPositionSmooth(SharpDX.Vector2 vec)
    {
      float step = Math.Max(vec.Distance(Input.ForceMousePosition) / 100f, 4f);
      if ((double) step > 6.0)
      {
        for (int i = 0; (double) i < (double) step; ++i)
        {
          Input.SetCursorPos(SharpDX.Vector2.SmoothStep(Input.ForceMousePosition, vec, (float) i / step));
          Input.MouseMove();
          yield return (object) Input.cursorPositionSmooth;
        }
      }
      else
        Input.SetCursorPos(vec);
    }

    [Obsolete]
    public static IEnumerator SetCursorPositionAndClick(
      SharpDX.Vector2 vec,
      MouseButtons button = MouseButtons.Left,
      int delay = 3)
    {
      Input.SetCursorPos(vec);
      yield return (object) new WaitTime(delay);
      Input.Click(button);
      yield return (object) new WaitTime(delay);
    }

    public static void VerticalScroll(bool forward, int clicks)
    {
      if (forward)
        User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_WHEEL, 0, 0, clicks * 120, (IntPtr) 0);
      else
        User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_WHEEL, 0, 0, -(clicks * 120), (IntPtr) 0);
    }

    [Obsolete]
    public static void SetCursorPos(SharpDX.Vector2 vec)
    {
      User32.SetCursorPos((int) vec.X, (int) vec.Y);
      Input.MouseMove();
    }

    public static void SetCursorPos(System.Numerics.Vector2 vec)
    {
      User32.SetCursorPos((int) vec.X, (int) vec.Y);
      Input.MouseMove();
    }

    public static void Click(MouseButtons buttons)
    {
      if (buttons != MouseButtons.Left)
      {
        if (buttons != MouseButtons.Right)
          return;
        Input.MouseMove();
        User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTDOWN | User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTUP, 0, 0, 0, (IntPtr) 0);
      }
      else
      {
        Input.MouseMove();
        User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN | User32.MOUSEEVENTF.MOUSEEVENTF_LEFTUP, 0, 0, 0, (IntPtr) 0);
      }
    }

    public static void LeftDown() => User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, (IntPtr) 0);

    public static void LeftUp() => User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_LEFTUP, 0, 0, 0, (IntPtr) 0);

    public static void MouseMove() => User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_MOVE, 0, 0, 0, (IntPtr) 0);

    public static IEnumerator KeyPress(System.Windows.Forms.Keys key)
    {
      if (key != System.Windows.Forms.Keys.None)
      {
        Input.KeyDown(key);
        yield return (object) Input.keyPress;
        Input.KeyUp(key);
      }
    }

    public static IEnumerator KeyPress(System.Windows.Forms.Keys key, int ms = 5)
    {
      if (key != System.Windows.Forms.Keys.None)
      {
        Input.KeyDown(key);
        yield return (object) new WaitTime(ms);
        Input.KeyUp(key);
      }
    }

    public static void KeyDown(System.Windows.Forms.Keys key)
    {
      if (key == System.Windows.Forms.Keys.None)
        return;
      User32.keybd_event((byte) key, (byte) 0, User32.KEYEVENTF.KEYEVENTF_EXTENDEDKEY, (IntPtr) 0);
    }

    public static void KeyUp(System.Windows.Forms.Keys key)
    {
      if (key == System.Windows.Forms.Keys.None)
        return;
      User32.keybd_event((byte) key, (byte) 0, User32.KEYEVENTF.KEYEVENTF_EXTENDEDKEY | User32.KEYEVENTF.KEYEVENTF_KEYUP, (IntPtr) 0);
    }

    public static void KeyDown(System.Windows.Forms.Keys key, IntPtr handle)
    {
      if (key == System.Windows.Forms.Keys.None)
        return;
      WinApi.SendMessage(handle, 256, (int) key, 0);
    }

    public static void KeyUp(System.Windows.Forms.Keys key, IntPtr handle)
    {
      if (key == System.Windows.Forms.Keys.None)
        return;
      WinApi.SendMessage(handle, 257, (int) key, 0);
    }

    public static void KeyPressRelease(System.Windows.Forms.Keys key, IntPtr handle)
    {
      if (key == System.Windows.Forms.Keys.None)
        return;
      if (Input.sw.ElapsedMilliseconds >= 10L && Input.KeysPressed[key])
      {
        Input.KeyUp(key, handle);
        lock (Input.locker)
          Input.KeysPressed[key] = false;
        Input.sw.Restart();
      }
      else
      {
        if (Input.KeysPressed[key])
          return;
        Input.KeyDown(key, handle);
        lock (Input.locker)
          Input.KeysPressed[key] = true;
        Input.sw.Restart();
      }
    }

    public static void KeyPressRelease(System.Windows.Forms.Keys key)
    {
      if (key == System.Windows.Forms.Keys.None)
        return;
      if (Input.sw.ElapsedMilliseconds >= 10L && Input.KeysPressed[key])
      {
        Input.KeyUp(key);
        lock (Input.locker)
          Input.KeysPressed[key] = false;
        Input.sw.Restart();
      }
      else
      {
        if (Input.KeysPressed[key])
          return;
        Input.KeyDown(key);
        lock (Input.locker)
          Input.KeysPressed[key] = true;
        Input.sw.Restart();
      }
    }
  }
}
