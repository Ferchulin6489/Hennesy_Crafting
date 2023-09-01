// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Targetable
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class Targetable : Component
  {
    private readonly CachedValue<TargetableComponentOffsets> _cachedValue;

    public Targetable() => this._cachedValue = (CachedValue<TargetableComponentOffsets>) new FrameCache<TargetableComponentOffsets>((Func<TargetableComponentOffsets>) (() => this.M.Read<TargetableComponentOffsets>(this.Address)));

    public TargetableComponentOffsets TargetableComponent => this._cachedValue.Value;

    public bool isTargetable => this.TargetableComponent.isTargetable;

    public bool isTargeted => this.TargetableComponent.isTargeted;
  }
}
