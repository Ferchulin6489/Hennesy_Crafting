// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.LocalStats
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Components
{
  public class LocalStats : Component
  {
    private readonly CachedValue<LocalStatsComponentOffsets> _localStatsValue;
    private readonly CachedValue<Dictionary<GameStat, int>> _statDictionary;
    private readonly Dictionary<GameStat, int> statDictionary = new Dictionary<GameStat, int>();

    public LocalStats()
    {
      this._localStatsValue = (CachedValue<LocalStatsComponentOffsets>) new FrameCache<LocalStatsComponentOffsets>((Func<LocalStatsComponentOffsets>) (() => this.M.Read<LocalStatsComponentOffsets>(this.Address)));
      this._statDictionary = (CachedValue<Dictionary<GameStat, int>>) new FrameCache<Dictionary<GameStat, int>>(new Func<Dictionary<GameStat, int>>(this.ParseStats));
    }

    public Dictionary<GameStat, int> StatDictionary => this._statDictionary.Value;

    public Dictionary<GameStat, int> ParseStats()
    {
      if (this.Address == 0L)
        return this.statDictionary;
      (GameStat, int)[] tupleArray = this.M.ReadStdVector<(GameStat, int)>(this._localStatsValue.Value.StatsPtr);
      this.statDictionary.Clear();
      this.statDictionary.EnsureCapacity(tupleArray.Length);
      foreach ((GameStat, int) tuple in tupleArray)
        this.statDictionary[tuple.Item1] = tuple.Item2;
      return this.statDictionary;
    }

    public int GetStatValue(GameStat stat)
    {
      int num;
      return this.StatDictionary.TryGetValue(stat, out num) ? num : 0;
    }
  }
}
