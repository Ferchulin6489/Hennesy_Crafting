// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Mods
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Components
{
  public class Mods : Component
  {
    private readonly CachedValue<ModsComponentOffsets> _cachedValue;
    private readonly CachedValue<ModsComponentStatsOffsets> _cachedStatsStruct;

    public Mods()
    {
      this._cachedValue = (CachedValue<ModsComponentOffsets>) new FrameCache<ModsComponentOffsets>((Func<ModsComponentOffsets>) (() => this.M.Read<ModsComponentOffsets>(this.Address)));
      this._cachedStatsStruct = (CachedValue<ModsComponentStatsOffsets>) new FrameCache<ModsComponentStatsOffsets>((Func<ModsComponentStatsOffsets>) (() => this.M.Read<ModsComponentStatsOffsets>(this._cachedValue.Value.ModsComponentStatsPtr)));
    }

    public ModsComponentOffsets ModsStruct => this._cachedValue.Value;

    public string UniqueName => this.GetUniqueName(this.ModsStruct.UniqueName);

    public bool Identified => this.Address != 0L && this.ModsStruct.Identified;

    public ItemRarity ItemRarity => this.Address == 0L ? ItemRarity.Normal : (ItemRarity) this.ModsStruct.ItemRarity;

    public long Hash
    {
      get
      {
        ModsComponentOffsets modsStruct = this.ModsStruct;
        int hashCode1 = modsStruct.implicitMods.GetHashCode();
        modsStruct = this.ModsStruct;
        int hashCode2 = modsStruct.explicitMods.GetHashCode();
        int num = hashCode1 ^ hashCode2;
        modsStruct = this.ModsStruct;
        int hashCode3 = modsStruct.GetHashCode();
        return (long) (num ^ hashCode3);
      }
    }

    public List<ItemMod> ItemMods
    {
      get
      {
        List<ItemMod> mods1 = this.GetMods(this.ModsStruct.enchantMods.First, this.ModsStruct.enchantMods.Last);
        List<ItemMod> mods2 = this.GetMods(this.ModsStruct.implicitMods.First, this.ModsStruct.implicitMods.Last);
        List<ItemMod> mods3 = this.GetMods(this.ModsStruct.explicitMods.First, this.ModsStruct.explicitMods.Last);
        List<ItemMod> mods4 = this.GetMods(this.ModsStruct.crucibleMods.First, this.ModsStruct.crucibleMods.Last);
        List<ItemMod> mods5 = this.GetMods(this.ModsStruct.ScourgeModsArray.First, this.ModsStruct.ScourgeModsArray.Last);
        List<ItemMod> second = mods2;
        return mods1.Concat<ItemMod>((IEnumerable<ItemMod>) second).ToList<ItemMod>().Concat<ItemMod>((IEnumerable<ItemMod>) mods3).ToList<ItemMod>().Concat<ItemMod>((IEnumerable<ItemMod>) mods4).ToList<ItemMod>().Concat<ItemMod>((IEnumerable<ItemMod>) mods5).ToList<ItemMod>();
      }
    }

    public int ItemLevel => this.Address == 0L ? 1 : this.ModsStruct.ItemLevel;

    public int RequiredLevel => this.Address == 0L ? 1 : this.ModsStruct.RequiredLevel;

    public bool IsUsable => this.Address != 0L && this.ModsStruct.IsUsable == (byte) 1;

    public bool IsMirrored => this.Address != 0L && this.ModsStruct.IsMirrored == (byte) 1;

    public int CountFractured => (int) this.ModsStruct.FracturedModsCount;

    public bool Synthesised => this.M.Read<byte>(this.Address + 1079L) == (byte) 1;

    public bool HaveFractured => this.CountFractured > 0;

    public ItemStats ItemStats => new ItemStats(this.Owner);

    public List<string> HumanStats => this.GetStats(this._cachedStatsStruct.Value.ExplicitStatsArray);

    public List<string> HumanCraftedStats => this.GetStats(this._cachedStatsStruct.Value.CraftedStatsArray);

    public List<string> HumanImpStats => this.GetStats(this._cachedStatsStruct.Value.ImplicitStatsArray);

    public List<string> FracturedStats => this.GetStats(this._cachedStatsStruct.Value.FracturedStatsArray);

    public List<string> EnchantedStats => this.GetStats(this._cachedStatsStruct.Value.EnchantedStatsArray);

    public List<string> CrucibleStats => this.GetStats(this._cachedStatsStruct.Value.CrucibleStatsArray);

    public ushort IncubatorKills => this.ModsStruct.IncubatorKills;

    public string IncubatorName
    {
      get
      {
        if (this.Address == 0L || this.ModsStruct.IncubatorPtr == 0L)
          return (string) null;
        return this.M.ReadStringU(this.M.Read<long>(this.ModsStruct.IncubatorPtr, 32));
      }
    }

    private List<string> GetStats(NativePtrArray array)
    {
      IList<long> longList = this.M.ReadPointersArray(array.First, array.Last, ModsComponentOffsets.HumanStats);
      List<string> stats = new List<string>();
      foreach (long num in (IEnumerable<long>) longList)
      {
        long pointer = num;
        List<string> stringList = stats;
        IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (Mods));
        interpolatedStringHandler.AppendFormatted<long>(pointer);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        Func<string> func = (Func<string>) (() => this.M.ReadStringU(pointer, 512));
        string str = stringCache.Read(stringAndClear, func);
        stringList.Add(str);
      }
      return stats;
    }

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

    private string GetUniqueName(NativePtrArray source)
    {
      List<string> words = new List<string>();
      if (this.Address == 0L || source.Size / 8L > 1000L)
        return string.Empty;
      for (long first = source.First; first < source.Last; first += (long) ModsComponentOffsets.NameRecordSize)
        words.Add(this.M.ReadStringU(this.M.Read<long>(first, ModsComponentOffsets.NameOffset)).Trim());
      IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
      interpolatedStringHandler.AppendFormatted(nameof (Mods));
      interpolatedStringHandler.AppendFormatted<long>(source.First);
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      Func<string> func = (Func<string>) (() => string.Join(" ", words.ToArray()));
      return stringCache.Read(stringAndClear, func);
    }
  }
}
