// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.WinApi
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets.Native;
using ProcessMemoryUtilities.Native;
using System;
using System.Windows.Forms;
using Vanara.PInvoke;

namespace ExileCore.Shared
{
  public static class WinApi
  {
    public const int SW_HIDE = 0;
    public const int SW_SHOW = 5;
    public const int SW_SHOWNORMAL = 1;
    public const int SW_SHOWMAXIMIZED = 3;
    public const int SW_RESTORE = 9;
    private const int WS_EX_LAYERED = 524288;
    private const int WS_EX_TRANSPARENT = 32;
    private const int WS_EX_TOPMOST = 8;
    private const int WS_VISIBLE = 268435456;

    public static void EnableTransparent(IntPtr handle)
    {
      User32.SetWindowLong((HWND) handle, User32.WindowLongFlags.GWL_STYLE, 268435456);
      User32.SetWindowLong((HWND) handle, User32.WindowLongFlags.GWL_EXSTYLE, 524288);
      HWND hWnd = (HWND) handle;
      DwmApi.MARGINS margins = new DwmApi.MARGINS(-1);
      ref DwmApi.MARGINS local = ref margins;
      DwmApi.DwmExtendFrameIntoClientArea(hWnd, in local);
    }

    public static void SetTransparent(IntPtr handle)
    {
      User32.SetWindowLong((HWND) handle, User32.WindowLongFlags.GWL_STYLE, 268435456);
      User32.SetWindowLong((HWND) handle, User32.WindowLongFlags.GWL_EXSTYLE, 524328);
    }

    public static void SetNoTransparent(IntPtr handle)
    {
      User32.SetWindowLong((HWND) handle, User32.WindowLongFlags.GWL_STYLE, 268435456);
      User32.SetWindowLong((HWND) handle, User32.WindowLongFlags.GWL_EXSTYLE, 524296);
    }

    public static void EnableTransparentByColorRef(IntPtr handle, System.Drawing.Rectangle size, int color)
    {
      int dwNewLong = User32.GetWindowLong((HWND) handle, User32.WindowLongFlags.GWL_EXSTYLE) | 524288;
      User32.SetWindowLong((HWND) handle, User32.WindowLongFlags.GWL_EXSTYLE, dwNewLong);
      User32.SetLayeredWindowAttributes((HWND) handle, (COLORREF) (uint) color, (byte) 100, User32.LayeredWindowAttributes.LWA_ALPHA | User32.LayeredWindowAttributes.LWA_COLORKEY);
      HWND hWnd = (HWND) handle;
      DwmApi.MARGINS margins = new DwmApi.MARGINS(size.Left, size.Right, size.Top, size.Bottom);
      ref DwmApi.MARGINS local = ref margins;
      DwmApi.DwmExtendFrameIntoClientArea(hWnd, in local);
    }

    public static IntPtr OpenProcess(System.Diagnostics.Process process, ProcessAccessFlags flags) => Vanara.PInvoke.Kernel32.OpenProcess(new ACCESS_MASK((uint) flags), false, (uint) process.Id).ReleaseOwnership();

    public static bool IsForegroundWindow(IntPtr handle) => User32.GetForegroundWindow().DangerousGetHandle() == handle;

    public static bool SetForegroundWindow(IntPtr hWnd) => User32.SetForegroundWindow((HWND) hWnd);

    public static bool AllowSetForegroundWindow(uint dwProcessId) => User32.AllowSetForegroundWindow(dwProcessId);

    public static bool ShowWindow(IntPtr hWnd, int nCmdShow) => User32.ShowWindow((HWND) hWnd, (ShowWindowCommand) nCmdShow);

    public static IntPtr SetWindowPos(
      IntPtr hWnd,
      int hWndInsertAfter,
      int x,
      int y,
      int cx,
      int cy,
      int wFlags)
    {
      return !User32.SetWindowPos((HWND) hWnd, (HWND) new IntPtr(hWndInsertAfter), x, y, cx, cy, (User32.SetWindowPosFlags) wFlags) ? new IntPtr(0) : new IntPtr(1);
    }

    public static System.Drawing.Rectangle GetClientRectangle(IntPtr handle)
    {
      RECT lpRect;
      User32.GetClientRect((HWND) handle, out lpRect);
      POINT lpPoint = new POINT();
      User32.ClientToScreen((HWND) handle, ref lpPoint);
      return new System.Drawing.Rectangle(lpPoint.X, lpPoint.Y, lpRect.Width, lpRect.Height);
    }

    public static int MakeCOLORREF(byte r, byte g, byte b) => (int) r | (int) g << 8 | (int) b << 16;

    public static bool ScreenToClient(IntPtr hWnd, ref SharpDX.Point lpPoint)
    {
      POINT lpPoint1 = new POINT(lpPoint.X, lpPoint.Y);
      int num = User32.ScreenToClient((HWND) hWnd, ref lpPoint1) ? 1 : 0;
      lpPoint = new SharpDX.Point(lpPoint1.X, lpPoint1.Y);
      return num != 0;
    }

    public static Vector2i ScreenToClient(IntPtr hWnd, Vector2i lpPoint)
    {
      POINT lpPoint1 = new POINT(lpPoint.X, lpPoint.Y);
      User32.ScreenToClient((HWND) hWnd, ref lpPoint1);
      return new Vector2i(lpPoint1.X, lpPoint1.Y);
    }

    public static short GetKeyState(Keys vKey) => User32.GetKeyState((int) vKey);

    public static short GetAsyncKeyState(Keys vKey) => User32.GetAsyncKeyState((int) vKey);

    public static bool GetCursorPos(out SharpDX.Point lpPoint)
    {
      POINT lpPoint1;
      int num = User32.GetCursorPos(out lpPoint1) ? 1 : 0;
      lpPoint = new SharpDX.Point(lpPoint1.X, lpPoint1.Y);
      return num != 0;
    }

    public static bool CloseHandle(IntPtr hObject) => Vanara.PInvoke.Kernel32.CloseHandle(hObject);

    public static bool IsIconic(IntPtr hWnd) => User32.IsIconic((HWND) hWnd);

    public static SharpDX.Point GetCursorPosition(IntPtr hWnd)
    {
      SharpDX.Point lpPoint;
      WinApi.GetCursorPos(out lpPoint);
      WinApi.ScreenToClient(hWnd, ref lpPoint);
      return lpPoint;
    }

    public static SharpDX.Point GetCursorPositionPoint()
    {
      SharpDX.Point lpPoint;
      WinApi.GetCursorPos(out lpPoint);
      return lpPoint;
    }

    public static bool SetCursorPos(int x, int y) => User32.SetCursorPos(x, y);

    public static uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo)
    {
      User32.keybd_event(bVk, bScan, (User32.KEYEVENTF) dwFlags, (IntPtr) dwExtraInfo);
      return 0;
    }

    public static void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo) => User32.mouse_event((User32.MOUSEEVENTF) dwFlags, dx, dy, cButtons, (IntPtr) dwExtraInfo);

    public static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam) => User32.SendMessage<int>((HWND) hWnd, msg, (IntPtr) wParam, (IntPtr) lParam);
  }
}
