// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Chest
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class Chest : Component
  {
    private readonly CachedValue<ChestComponentOffsets> _cachedValue;
    private readonly CachedValue<StrongboxChestComponentData> _cachedValueStrongboxData;

    public Chest()
    {
      this._cachedValue = (CachedValue<ChestComponentOffsets>) new FramesCache<ChestComponentOffsets>((Func<ChestComponentOffsets>) (() => this.M.Read<ChestComponentOffsets>(this.Address)), 3U);
      this._cachedValueStrongboxData = (CachedValue<StrongboxChestComponentData>) new FramesCache<StrongboxChestComponentData>((Func<StrongboxChestComponentData>) (() => this.M.Read<StrongboxChestComponentData>(this._cachedValue.Value.StrongboxData)), 3U);
    }

    public bool IsOpened => this.Address != 0L && this._cachedValue.Value.IsOpened;

    public bool IsLocked => this.Address != 0L && this._cachedValue.Value.IsLocked;

    public bool IsStrongbox => this.Address != 0L && this._cachedValue.Value.IsStrongbox;

    private long StrongboxData => this._cachedValue.Value.StrongboxData;

    public bool DestroyingAfterOpen => this.Address != 0L && this._cachedValueStrongboxData.Value.DestroyingAfterOpen;

    public bool IsLarge => this.Address != 0L && this._cachedValueStrongboxData.Value.IsLarge;

    public bool Stompable => this.Address != 0L && this._cachedValueStrongboxData.Value.Stompable;

    public bool OpenOnDamage => this.Address != 0L && this._cachedValueStrongboxData.Value.OpenOnDamage;
  }
}
