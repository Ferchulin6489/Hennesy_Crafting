// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.MinimapIcon
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using System;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Components
{
  public class MinimapIcon : Component
  {
    private FrameCache<MinimapIconOffsets> cachedValue;

    public MinimapIcon() => this.cachedValue = new FrameCache<MinimapIconOffsets>((Func<MinimapIconOffsets>) (() => this.M.Read<MinimapIconOffsets>(this.Address)));

    private MinimapIconOffsets MinimapIconOffsets => this.cachedValue.Value;

    public bool IsVisible => this.MinimapIconOffsets.IsVisible == (byte) 0;

    public bool IsHide => this.MinimapIconOffsets.IsHide == (byte) 1;

    public string TestString => this.M.ReadStringU(this.M.Read<long>(this.MinimapIconOffsets.NamePtr));

    public string Name
    {
      get
      {
        IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted<long>(this.MinimapIconOffsets.NamePtr);
        interpolatedStringHandler.AppendFormatted<long>(this.Address);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        Func<string> func = (Func<string>) (() => this.TestString);
        return stringCache.Read(stringAndClear, func);
      }
    }
  }
}
