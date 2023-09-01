// Decompiled with JetBrains decompiler
// Type: ExileCore.RenderQ.DX11
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ExileCore.RenderQ
{
  public class DX11 : IDisposable
  {
    private readonly ActionOverlay _overlay;
    private readonly ReaderWriterLockSlim _sync = new ReaderWriterLockSlim();

    public DX11(ActionOverlay overlay, CoreSettings coreSettings)
    {
      this._overlay = overlay;
      this.ImGuiRender = new ImGuiRender(this, overlay, coreSettings);
      this.SpritesRender = new SpritesRender(this, this.ImGuiRender);
    }

    public ImGuiRender ImGuiRender { get; }

    [Obsolete]
    public SpritesRender SpritesRender { get; }

    public void Dispose() => this._sync.Dispose();

    public void DisposeTexture(string name)
    {
      this._sync.EnterWriteLock();
      try
      {
        if (this._overlay.RemoveImage(name))
          return;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
        interpolatedStringHandler.AppendLiteral("(");
        interpolatedStringHandler.AppendFormatted(nameof (DisposeTexture));
        interpolatedStringHandler.AppendLiteral(") Texture ");
        interpolatedStringHandler.AppendFormatted(name);
        interpolatedStringHandler.AppendLiteral(" not found.");
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear(), 10f);
      }
      finally
      {
        this._sync.ExitWriteLock();
      }
    }

    public void AddOrUpdateTexture(string name, Image<Rgba32> image)
    {
      this._sync.EnterWriteLock();
      try
      {
        this._overlay.RemoveImage(name);
        this._overlay.AddOrGetImagePointer(name, image, false, out IntPtr _);
      }
      finally
      {
        this._sync.ExitWriteLock();
      }
    }

    public IntPtr GetTexture(string name)
    {
      this._sync.EnterReadLock();
      try
      {
        return this._overlay.GetImagePointer(name);
      }
      finally
      {
        this._sync.ExitReadLock();
      }
    }

    public bool HasTexture(string name)
    {
      this._sync.EnterReadLock();
      try
      {
        return this._overlay.HasImagePointer(name);
      }
      finally
      {
        this._sync.ExitReadLock();
      }
    }

    public bool InitTexture(string name)
    {
      if (!File.Exists(name))
      {
        DebugWindow.LogError(name + " not found.");
        return false;
      }
      this._sync.EnterWriteLock();
      try
      {
        this._overlay.AddOrGetImagePointer(name, ((IEnumerable<string>) name.Split(new char[2]
        {
          '/',
          '\\'
        })).Last<string>(), false, out IntPtr _, out uint _, out uint _);
      }
      finally
      {
        this._sync.ExitWriteLock();
      }
      return true;
    }
  }
}
