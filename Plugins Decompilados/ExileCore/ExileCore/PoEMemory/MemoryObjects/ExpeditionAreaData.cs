// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ExpeditionAreaData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ExpeditionAreaData : RemoteMemoryObject
  {
    public const int StructSize = 192;
    private readonly CachedValue<ExpeditionAreaDataOffsets> _cachedValue;

    public ExpeditionAreaData() => this._cachedValue = (CachedValue<ExpeditionAreaDataOffsets>) new FrameCache<ExpeditionAreaDataOffsets>((Func<ExpeditionAreaDataOffsets>) (() => this.M.Read<ExpeditionAreaDataOffsets>(this.Address)));

    public ExpeditionAreaDataOffsets ExpeditionAreaDataStruct => this._cachedValue.Value;

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address, 0, 8));

    public string Faction => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L, 8));

    public List<ItemMod> ImplicitMods => this.GetMods(this.ExpeditionAreaDataStruct.ModsData.First, this.ExpeditionAreaDataStruct.ModsData.Last);

    private List<ItemMod> GetMods(long startOffset, long endOffset)
    {
      List<ItemMod> mods = new List<ItemMod>();
      if (this.Address == 0L)
        return mods;
      long num1 = startOffset;
      long num2 = endOffset;
      if ((num2 - num1) / (long) ItemMod.STRUCT_SIZE > 12L)
        return mods;
      for (long address = num1; address < num2; address += (long) ItemMod.STRUCT_SIZE)
        mods.Add(this.GetObject<ItemMod>(address));
      return mods;
    }
  }
}
