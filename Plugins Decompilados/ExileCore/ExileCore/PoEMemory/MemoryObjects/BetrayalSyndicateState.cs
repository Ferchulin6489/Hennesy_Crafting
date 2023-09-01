// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BetrayalSyndicateState
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BetrayalSyndicateState : RemoteMemoryObject
  {
    public static int STRUCT_SIZE = 160;

    public Element UIElement => this.ReadObjectAt<Element>(0);

    public float PosX => this.M.Read<float>(this.Address + 124L);

    public float PosY => this.M.Read<float>(this.Address + 124L);

    public BetrayalTarget Target => this.TheGame.Files.BetrayalTargets.GetByAddress(this.M.Read<long>(this.Address + 8L));

    public BetrayalJob Job => this.TheGame.Files.BetrayalJobs.GetByAddress(this.M.Read<long>(this.Address + 24L));

    public BetrayalRank Rank => this.TheGame.Files.BetrayalRanks.GetByAddress(this.M.Read<long>(this.Address + 40L));

    public BetrayalReward Reward => this.TheGame.Files.BetrayalRewards.EntriesList.Find((Predicate<BetrayalReward>) (x => x.Target == this.Target && x.Job == this.Job && x.Rank == this.Rank));

    public List<BetrayalUpgrade> BetrayalUpgrades
    {
      get
      {
        long num1 = this.M.Read<long>(this.Address + 56L);
        long num2 = this.M.Read<long>(this.Address + 64L);
        List<BetrayalUpgrade> betrayalUpgrades = new List<BetrayalUpgrade>();
        for (long index = num1; index < num2; index += 16L)
          betrayalUpgrades.Add(this.ReadObject<BetrayalUpgrade>(index + 8L));
        return betrayalUpgrades;
      }
    }

    public List<BetrayalSyndicateState> Relations
    {
      get
      {
        long num = this.M.Read<long>(this.Address + 80L);
        List<BetrayalSyndicateState> relations = new List<BetrayalSyndicateState>();
        for (int index = 0; index < 3; ++index)
        {
          long address = this.M.Read<long>(num + (long) (index * 8));
          if (address != 0L)
            relations.Add(this.GetObject<BetrayalSyndicateState>(address));
        }
        return relations;
      }
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
      interpolatedStringHandler.AppendFormatted(this.Target?.Name);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.Rank?.Name);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.Job?.Name);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
