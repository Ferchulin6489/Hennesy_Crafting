// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Metamorph.MetamorphRewardTypeItemsClient
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.FilesInMemory.Metamorph
{
  public class MetamorphRewardTypeItemsClient : RemoteMemoryObject
  {
    public MetamorphRewardType RewardType => this.TheGame.Files.MetamorphRewardTypes.GetByAddress(this.M.Read<long>(this.Address + 8L));

    public int Unknown => this.M.Read<int>(this.Address + 16L);

    public string Description => this.M.ReadStringU(this.M.Read<long>(this.Address + 20L), (int) byte.MaxValue);

    public override string ToString() => this.RewardType.Id + ": " + this.Description;
  }
}
