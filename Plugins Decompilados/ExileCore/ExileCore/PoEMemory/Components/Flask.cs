// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Flask
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets.Native;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Components
{
  public class Flask : Component
  {
    private readonly FrameCache<LocalStats> cacheLocalStatsComponent;
    private readonly FrameCache<Quality> cacheQualityComponent;
    private readonly CachedValue<Dictionary<GameStat, int>> _flaskStatDictionary;
    private readonly Dictionary<GameStat, int> flaskStatDictionary = new Dictionary<GameStat, int>();

    public LocalStats LocalStatsComponent => this.cacheLocalStatsComponent.Value;

    public Quality QualityComponent => this.cacheQualityComponent.Value;

    public Flask()
    {
      this.cacheQualityComponent = new FrameCache<Quality>((Func<Quality>) (() => this.ReadObjectAt<Quality>(48)));
      this.cacheLocalStatsComponent = new FrameCache<LocalStats>((Func<LocalStats>) (() => this.ReadObjectAt<LocalStats>(56)));
      this._flaskStatDictionary = (CachedValue<Dictionary<GameStat, int>>) new FrameCache<Dictionary<GameStat, int>>(new Func<Dictionary<GameStat, int>>(this.ParseStats));
    }

    public Dictionary<GameStat, int> FlaskStatDictionary => this._flaskStatDictionary.Value;

    public Dictionary<GameStat, int> ParseStats()
    {
      if (this.Address == 0L)
        return this.flaskStatDictionary;
      (GameStat, int)[] tupleArray = this.M.ReadStdVector<(GameStat, int)>(this.M.Read<StdVector>(this.M.Read<long>(this.Address + 40L) + 48L));
      this.flaskStatDictionary.Clear();
      this.flaskStatDictionary.EnsureCapacity(tupleArray.Length);
      foreach ((GameStat, int) tuple in tupleArray)
        this.flaskStatDictionary[tuple.Item1] = tuple.Item2;
      return this.flaskStatDictionary;
    }

    public int GetStatValue(GameStat stat)
    {
      int num;
      return this.FlaskStatDictionary.TryGetValue(stat, out num) ? num : 0;
    }

    public int LifeRecover
    {
      get
      {
        float statValue1 = (float) this.GetStatValue(GameStat.LocalFlaskLifeToRecover);
        float statValue2 = (float) this.LocalStatsComponent.GetStatValue(GameStat.LocalFlaskLifeToRecoverPct);
        return (int) (((double) this.LocalStatsComponent.GetStatValue(GameStat.LocalFlaskAmountToRecoverPct) * 0.009999999 + 1.0) * (((double) this.QualityComponent.ItemQuality * 0.01 + 1.0) * (double) statValue1 * ((double) statValue2 * 0.0099999997764825821 + 1.0)) + 0.5);
      }
    }

    public int ManaRecover
    {
      get
      {
        float statValue1 = (float) this.GetStatValue(GameStat.LocalFlaskManaToRecover);
        float statValue2 = (float) this.LocalStatsComponent.GetStatValue(GameStat.LocalFlaskManaToRecoverPct);
        return (int) (((double) this.LocalStatsComponent.GetStatValue(GameStat.LocalFlaskAmountToRecoverPct) * 0.009999999 + 1.0) * (((double) this.QualityComponent.ItemQuality * 0.01 + 1.0) * (double) statValue1 * ((double) statValue2 * 0.0099999997764825821 + 1.0)) + 0.5);
      }
    }
  }
}
