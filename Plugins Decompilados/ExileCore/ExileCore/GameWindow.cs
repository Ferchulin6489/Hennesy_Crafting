// Decompiled with JetBrains decompiler
// Type: ExileCore.GameWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared;
using ExileCore.Shared.Cache;
using GameOffsets.Native;
using SharpDX;
using System;
using System.Diagnostics;

namespace ExileCore
{
  public class GameWindow
  {
    private readonly IntPtr handle;
    private readonly CachedValue<SharpDX.RectangleF> _getWindowRectangle;
    private System.Drawing.Rectangle _lastValid = System.Drawing.Rectangle.Empty;

    public GameWindow(Process process)
    {
      this.Process = process;
      this.handle = process.MainWindowHandle;
      this._getWindowRectangle = (CachedValue<SharpDX.RectangleF>) new TimeCache<SharpDX.RectangleF>(new Func<SharpDX.RectangleF>(this.GetWindowRectangleReal), 200L);
    }

    public Process Process { get; }

    public SharpDX.RectangleF GetWindowRectangleTimeCache => this._getWindowRectangle.Value;

    public SharpDX.RectangleF GetWindowRectangle() => this._getWindowRectangle.Value;

    public SharpDX.RectangleF GetWindowRectangleReal()
    {
      System.Drawing.Rectangle rectangle = WinApi.GetClientRectangle(this.handle);
      if (rectangle.Width < 0 && rectangle.Height < 0)
        rectangle = this._lastValid;
      else
        this._lastValid = rectangle;
      return new SharpDX.RectangleF((float) rectangle.X, (float) rectangle.Y, (float) rectangle.Width, (float) rectangle.Height);
    }

    public bool IsForeground() => WinApi.IsForegroundWindow(this.handle);

    [Obsolete]
    public Vector2 ScreenToClient(int x, int y)
    {
      SharpDX.Point lpPoint = new SharpDX.Point(x, y);
      WinApi.ScreenToClient(this.handle, ref lpPoint);
      return (Vector2) lpPoint;
    }

    public Vector2i ScreenToClient(Vector2i screenCoords) => WinApi.ScreenToClient(this.handle, screenCoords);

    public Vector2i ScreenToClient2(int x, int y) => WinApi.ScreenToClient(this.handle, new Vector2i(x, y));
  }
}
