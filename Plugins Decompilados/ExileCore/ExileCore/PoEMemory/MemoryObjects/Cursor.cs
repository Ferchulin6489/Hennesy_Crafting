// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Cursor
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class Cursor : Element
  {
    private readonly CachedValue<CursorOffsets> _cachevalue;

    public Cursor() => this._cachevalue = (CachedValue<CursorOffsets>) new FrameCache<CursorOffsets>((Func<CursorOffsets>) (() => this.M.Read<CursorOffsets>(this.Address)));

    public MouseActionType Action => (MouseActionType) this.M.Read<int>(this.Address + 896L);

    public MouseActionType ActionCached => (MouseActionType) this._cachevalue.Value.Action;

    public int ClicksCached => this._cachevalue.Value.Clicks;

    public int Clicks => this.M.Read<int>(this.Address + 588L);

    public string ActionString => this.M.ReadNativeString(this.Address + 672L);

    public string ActionStringCached => this._cachevalue.Value.ActionString.ToString(this.M);
  }
}
