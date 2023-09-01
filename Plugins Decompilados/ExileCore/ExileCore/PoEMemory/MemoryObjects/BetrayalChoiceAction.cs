// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BetrayalChoiceAction
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BetrayalChoiceAction : RemoteMemoryObject
  {
    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public BetrayalChoice Choice => this.TheGame.Files.BetrayalChoises.GetByAddress(this.M.Read<long>(this.Address + 8L));

    public override string ToString() => this.Id + " (" + this.Choice.Name + ")";
  }
}
