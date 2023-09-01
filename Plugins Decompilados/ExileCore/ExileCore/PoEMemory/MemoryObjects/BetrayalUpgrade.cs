// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BetrayalUpgrade
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BetrayalUpgrade : RemoteMemoryObject
  {
    public string UpgradeName => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public string UpgradeStat => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L));

    public string Art => this.M.ReadStringU(this.M.Read<long>(this.Address + 40L));

    public override string ToString() => this.UpgradeName + " (" + this.UpgradeStat + ")";
  }
}
