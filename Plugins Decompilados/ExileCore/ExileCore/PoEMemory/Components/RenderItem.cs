// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.RenderItem
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Helpers;
using GameOffsets.Native;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class RenderItem : Component
  {
    private const int ResourcePathOffset = 40;

    public string ResourcePath
    {
      get
      {
        NativeUtf16Text text = this.M.Read<NativeUtf16Text>(this.Address + 40L);
        return RemoteMemoryObject.Cache.StringCache.Read(nameof (RenderItem) + text.CacheString, (Func<string>) (() => text.ToString(this.M)));
      }
    }
  }
}
