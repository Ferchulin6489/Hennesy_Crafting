// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.BetrayalRank
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class BetrayalRank : RemoteMemoryObject
  {
    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public int Unknown => this.M.Read<int>(this.Address + 16L);

    public string Art => this.M.ReadStringU(this.M.Read<long>(this.Address + 20L));

    public int RankInt
    {
      get
      {
        switch (this.Id)
        {
          case "Rank1":
            return 1;
          case "Rank2":
            return 2;
          case "Rank3":
            return 3;
          default:
            return 0;
        }
      }
    }

    public override string ToString() => this.Name;
  }
}
