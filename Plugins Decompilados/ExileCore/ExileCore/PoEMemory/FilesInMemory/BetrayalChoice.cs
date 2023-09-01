// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.BetrayalChoice
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class BetrayalChoice : RemoteMemoryObject
  {
    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public override string ToString() => this.Name;
  }
}
