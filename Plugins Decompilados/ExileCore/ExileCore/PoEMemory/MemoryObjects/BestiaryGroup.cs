// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BestiaryGroup
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BestiaryGroup : RemoteMemoryObject
  {
    private BestiaryFamily family;
    private string groupId;
    private string name;

    public int Id { get; internal set; }

    public string GroupId => this.groupId == null ? (this.groupId = this.M.ReadStringU(this.M.Read<long>(this.Address))) : this.groupId;

    public string Description => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public string Illustration => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L));

    public string Name => this.name == null ? (this.name = this.M.ReadStringU(this.M.Read<long>(this.Address + 24L))) : this.name;

    public string SmallIcon => this.M.ReadStringU(this.M.Read<long>(this.Address + 32L));

    public string ItemIcon => this.M.ReadStringU(this.M.Read<long>(this.Address + 40L));

    public BestiaryFamily Family => this.family == null ? (this.family = this.TheGame.Files.BestiaryFamilies.GetByAddress(this.M.Read<long>(this.Address + 56L))) : this.family;

    public override string ToString() => this.Name;
  }
}
