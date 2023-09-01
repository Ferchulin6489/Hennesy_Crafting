// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ActorSkillCooldown
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ActorSkillCooldown : RemoteMemoryObject
  {
    private readonly CachedValue<ActorSkillCooldownOffsets> _cache;

    public ActorSkillCooldown() => this._cache = (CachedValue<ActorSkillCooldownOffsets>) new FrameCache<ActorSkillCooldownOffsets>((Func<ActorSkillCooldownOffsets>) (() => this.M.Read<ActorSkillCooldownOffsets>(this.Address)));

    public ushort Id => this._cache.Value.SkillId;

    public int SkillSubId => this._cache.Value.SkillSubId;

    private StdVector CooldownUses => this._cache.Value.Cooldowns;

    public int MaxUses => this._cache.Value.MaxUses;

    public List<SkillCooldown> SkillCooldowns => this.M.ReadStructsArray<SkillCooldown>(this.CooldownUses.First, this.CooldownUses.Last, 16, (RemoteMemoryObject) null);
  }
}
