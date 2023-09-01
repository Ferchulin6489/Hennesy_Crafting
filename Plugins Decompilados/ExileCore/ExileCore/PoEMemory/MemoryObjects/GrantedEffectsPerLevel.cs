// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.GrantedEffectsPerLevel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class GrantedEffectsPerLevel : RemoteMemoryObject
  {
    private GrantedEffectPerLevel Effect => this.TheGame.Files.GrantedEffectsPerLevel.GetByAddress(this.Address);

    public SkillGemWrapper SkillGemWrapper => this.ReadObject<SkillGemWrapper>(this.Address);

    public int Level => this.Effect.Level;

    public int RequiredLevel => this.Effect.RequiredLevel;

    public int ManaMultiplier => this.Effect.CostMultiplier;

    public int Cooldown => this.Effect.Cooldown;

    public int ManaCost => this.M.Read<int>(this.Address + 168L);

    public int EffectivenessOfAddedDamage => this.M.Read<int>(this.Address + 172L);

    public IEnumerable<Tuple<StatsDat.StatRecord, int>> Stats
    {
      get
      {
        List<Tuple<StatsDat.StatRecord, int>> stats = new List<Tuple<StatsDat.StatRecord, int>>();
        int num = this.M.Read<int>(this.Address + 20L);
        long addr = this.M.Read<long>(this.Address + 28L) + 8L;
        for (int index = 0; index < num; ++index)
        {
          StatsDat.StatRecord statByAddress = this.TheGame.Files.Stats.GetStatByAddress(this.M.Read<long>(addr));
          stats.Add(new Tuple<StatsDat.StatRecord, int>(statByAddress, this.ReadStatValue(index)));
          addr += 16L;
        }
        return (IEnumerable<Tuple<StatsDat.StatRecord, int>>) stats;
      }
    }

    public IEnumerable<string> Tags
    {
      get
      {
        List<string> tags = new List<string>();
        int num = this.M.Read<int>(this.Address + 68L);
        long addr1 = this.M.Read<long>(this.Address + 76L) + 8L;
        for (int index = 0; index < num; ++index)
        {
          long addr2 = this.M.Read<long>(this.M.Read<long>(addr1));
          tags.Add(this.M.ReadStringU(addr2));
          addr1 += 16L;
        }
        return (IEnumerable<string>) tags;
      }
    }

    public IEnumerable<Tuple<StatsDat.StatRecord, int>> QualityStats
    {
      get
      {
        List<Tuple<StatsDat.StatRecord, int>> qualityStats = new List<Tuple<StatsDat.StatRecord, int>>();
        int num = this.M.Read<int>(this.Address + 132L);
        long addr = this.M.Read<long>(this.Address + 140L) + 8L;
        for (int index = 0; index < num; ++index)
        {
          StatsDat.StatRecord statByAddress = this.TheGame.Files.Stats.GetStatByAddress(this.M.Read<long>(addr));
          qualityStats.Add(new Tuple<StatsDat.StatRecord, int>(statByAddress, this.ReadQualityStatValue(index)));
          addr += 16L;
        }
        return (IEnumerable<Tuple<StatsDat.StatRecord, int>>) qualityStats;
      }
    }

    public IEnumerable<StatsDat.StatRecord> TypeStats
    {
      get
      {
        List<StatsDat.StatRecord> typeStats = new List<StatsDat.StatRecord>();
        int num = this.M.Read<int>(this.Address + 188L);
        long addr = this.M.Read<long>(this.Address + 196L) + 8L;
        for (int index = 0; index < num; ++index)
        {
          StatsDat.StatRecord statByAddress = this.TheGame.Files.Stats.GetStatByAddress(this.M.Read<long>(addr));
          typeStats.Add(statByAddress);
          addr += 16L;
        }
        return (IEnumerable<StatsDat.StatRecord>) typeStats;
      }
    }

    internal int ReadStatValue(int index) => this.M.Read<int>(this.Address + 84L + (long) (index * 4));

    internal int ReadQualityStatValue(int index) => this.M.Read<int>(this.Address + 156L + (long) (index * 4));
  }
}
