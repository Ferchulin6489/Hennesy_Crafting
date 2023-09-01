// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Buffs
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Components
{
  public sealed class Buffs : Component
  {
    private readonly CachedValue<List<Buff>> _cachedValueBuffs;

    public Buffs() => this._cachedValueBuffs = (CachedValue<List<Buff>>) new FrameCache<List<Buff>>(CacheUtils.RememberLastValue<List<Buff>>(new Func<List<Buff>, List<Buff>>(this.ParseBuffs)));

    public List<Buff> BuffsList => this._cachedValueBuffs.Value;

    public List<Buff> ParseBuffs() => this.ParseBuffs((List<Buff>) null);

    private List<Buff> ParseBuffs(List<Buff> lastValue)
    {
      NativePtrArray buffs = this.M.Read<BuffsOffsets>(this.Address).Buffs;
      List<Buff> list = this.M.ReadPointersArray(buffs.First, buffs.Last).Select<long, Buff>(new Func<long, Buff>(((RemoteMemoryObject) this).GetObject<Buff>)).ToList<Buff>();
      return list.Count != 0 || lastValue == null || lastValue.Count <= 0 || !buffs.Equals(new NativePtrArray()) ? list : lastValue;
    }

    public bool HasBuff(string buff)
    {
      List<Buff> buffsList = this.BuffsList;
      return buffsList != null && buffsList.AnyF<Buff>((Predicate<Buff>) (x => x.Name == buff));
    }

    public bool TryGetBuff(string name, out Buff buff)
    {
      buff = this.BuffsList.FirstOrDefault<Buff>((Func<Buff, bool>) (x => x.Name == name));
      return buff != null;
    }
  }
}
