// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.BetrayalReward
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class BetrayalReward : RemoteMemoryObject
  {
    public BetrayalJob Job => this.TheGame.Files.BetrayalJobs.GetByAddress(this.M.Read<long>(this.Address));

    public BetrayalTarget Target => this.TheGame.Files.BetrayalTargets.GetByAddress(this.M.Read<long>(this.Address + 16L));

    public BetrayalRank Rank => this.TheGame.Files.BetrayalRanks.GetByAddress(this.M.Read<long>(this.Address + 32L));

    public string Reward => this.M.ReadStringU(this.M.Read<long>(this.Address + 48L));

    public override string ToString() => this.Reward;
  }
}
