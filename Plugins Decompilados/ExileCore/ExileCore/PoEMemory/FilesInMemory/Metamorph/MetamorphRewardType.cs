// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Metamorph.MetamorphRewardType
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.FilesInMemory.Metamorph
{
  public class MetamorphRewardType : RemoteMemoryObject
  {
    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address), (int) byte.MaxValue);

    public string Art => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L), (int) byte.MaxValue);

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L), (int) byte.MaxValue);

    public override string ToString() => this.Id + ": " + this.Name;
  }
}
