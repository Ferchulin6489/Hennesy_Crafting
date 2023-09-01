// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.AreaTransition
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets.Components;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class AreaTransition : Component
  {
    private readonly CachedValue<AreaTransitionComponentOffsets> _cache;

    public AreaTransition() => this._cache = (CachedValue<AreaTransitionComponentOffsets>) new FrameCache<AreaTransitionComponentOffsets>((Func<AreaTransitionComponentOffsets>) (() => this.M.Read<AreaTransitionComponentOffsets>(this.Address)));

    public int WorldAreaId => (int) this._cache.Value.AreaId;

    public WorldArea AreaById => this.TheGame.Files.WorldAreas.GetAreaByAreaId(this.WorldAreaId);

    public WorldArea WorldArea => this.TheGame.Files.WorldAreas.GetByAddress(this._cache.Value.WorldAreaInfoPtr);

    public AreaTransitionType TransitionType => (AreaTransitionType) this._cache.Value.TransitionType;
  }
}
