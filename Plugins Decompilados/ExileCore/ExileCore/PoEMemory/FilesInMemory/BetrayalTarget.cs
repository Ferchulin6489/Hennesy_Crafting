// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.BetrayalTarget
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class BetrayalTarget : RemoteMemoryObject
  {
    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public MonsterVariety MonsterVariety => this.TheGame.Files.MonsterVarieties.GetByAddress(this.M.Read<long>(this.Address + 24L));

    public string Art => this.M.ReadStringU(this.M.Read<long>(this.Address + 56L));

    public string FullName => this.M.ReadStringU(this.M.Read<long>(this.Address + 81L));

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address + 97L));

    public override string ToString() => this.Name;
  }
}
