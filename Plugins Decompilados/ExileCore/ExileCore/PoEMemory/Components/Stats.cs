// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Stats
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using GameOffsets.Native;
using Serilog;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Components
{
  public class Stats : Component
  {
    private readonly CachedValue<StatsComponentOffsets> _statsValue;
    private readonly CachedValue<SubStatsComponentOffsets> _substructStatsValue;
    private readonly CachedValue<Dictionary<GameStat, int>> _statDictionary;
    private readonly Dictionary<string, int> testHumanDictionary = new Dictionary<string, int>();
    private readonly Dictionary<GameStat, int> statDictionary = new Dictionary<GameStat, int>();

    public Stats()
    {
      this._statsValue = (CachedValue<StatsComponentOffsets>) new FrameCache<StatsComponentOffsets>((Func<StatsComponentOffsets>) (() => this.M.Read<StatsComponentOffsets>(this.Address)));
      this._substructStatsValue = (CachedValue<SubStatsComponentOffsets>) new FrameCache<SubStatsComponentOffsets>((Func<SubStatsComponentOffsets>) (() => this.M.Read<SubStatsComponentOffsets>(this._statsValue.Value.SubStatsPtr)));
      this._statDictionary = (CachedValue<Dictionary<GameStat, int>>) new FrameCache<Dictionary<GameStat, int>>(new Func<Dictionary<GameStat, int>>(this.ParseStats));
    }

    public new long OwnerAddress => this._statsValue.Value.Owner;

    public SubStatsComponentOffsets StatsComponent => this._substructStatsValue.Value;

    public Dictionary<GameStat, int> StatDictionary => this._statDictionary.Value;

    public long StatsCount => this.StatsComponent.Stats.TotalElements(8);

    public Dictionary<GameStat, int> ParseStats()
    {
      if (this.Address == 0L)
        return this.statDictionary;
      long statsCount = this.StatsCount;
      if (statsCount <= 0L || this.StatsComponent.Stats.Last > this.StatsComponent.Stats.End || this.StatsComponent.Stats.First == 0L)
        return this.statDictionary;
      if (statsCount > 9000L)
      {
        ILogger logger = Core.Logger;
        if (logger != null)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
          interpolatedStringHandler.AppendLiteral("Stats over capped: ");
          interpolatedStringHandler.AppendFormatted<StdVector>(this.StatsComponent.Stats);
          interpolatedStringHandler.AppendLiteral(" Total Stats: ");
          interpolatedStringHandler.AppendFormatted<long>(statsCount);
          logger.Error(interpolatedStringHandler.ToStringAndClear());
        }
        return this.statDictionary;
      }
      (GameStat, int)[] tupleArray = this.M.ReadStdVector<(GameStat, int)>(this.StatsComponent.Stats);
      this.statDictionary.Clear();
      this.statDictionary.EnsureCapacity(tupleArray.Length);
      foreach ((GameStat, int) tuple in tupleArray)
        this.statDictionary[tuple.Item1] = tuple.Item2;
      return this.statDictionary;
    }

    public Dictionary<string, int> HumanStats()
    {
      Dictionary<GameStat, int> statDictionary = this.StatDictionary;
      this.testHumanDictionary.Clear();
      StatsDat stats = this.TheGame.Files.Stats;
      if (stats == null)
        return (Dictionary<string, int>) null;
      foreach (KeyValuePair<GameStat, int> keyValuePair in statDictionary)
      {
        StatsDat.StatRecord statRecord;
        if (stats.recordsById.TryGetValue((int) keyValuePair.Key, out statRecord))
          this.testHumanDictionary[statRecord.Key] = keyValuePair.Value;
      }
      return this.testHumanDictionary;
    }
  }
}
