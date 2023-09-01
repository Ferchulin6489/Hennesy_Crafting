// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.WorldItem
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class WorldItem : Component
  {
    private readonly CachedValue<Entity> _cachedValue;

    public WorldItem() => this._cachedValue = (CachedValue<Entity>) new FrameCache<Entity>((Func<Entity>) (() => this.Address == 0L ? (Entity) null : this.ReadObject<Entity>(this.Address + 40L)));

    public Entity ItemEntity => this._cachedValue.Value;

    public uint AllocatedToPlayer => this.M.Read<uint>(this.Address + 48L);

    public int AllocatedToOtherTime => this.M.Read<int>(this.Address + 52L);

    public DateTime DroppedTime
    {
      get
      {
        int num = this.M.Read<int>(this.Address + 56L);
        return DateTime.Now - TimeSpan.FromMilliseconds((double) Environment.TickCount) + TimeSpan.FromMilliseconds((double) num);
      }
    }

    public bool IsPermanentlyAllocated => this.AllocatedToOtherTime == 300000;

    public DateTime PublicTime => this.DroppedTime + TimeSpan.FromMilliseconds((double) this.AllocatedToOtherTime);

    public bool AllocatedToSomeoneElse => this.AllocatedToPlayer != 0U && (long) Entity.Player.GetComponent<Player>().AllocatedLootId != (long) this.AllocatedToPlayer;
  }
}
