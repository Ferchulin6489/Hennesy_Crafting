// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BestiaryFamily
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BestiaryFamily : RemoteMemoryObject
  {
    private string familyId;
    private string name;

    public int Id { get; internal set; }

    public string FamilyId => this.familyId == null ? (this.familyId = this.M.ReadStringU(this.M.Read<long>(this.Address))) : this.familyId;

    public string Name => this.name == null ? (this.name = this.M.ReadStringU(this.M.Read<long>(this.Address + 8L))) : this.name;

    public string Icon => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L));

    public string SmallIcon => this.M.ReadStringU(this.M.Read<long>(this.Address + 24L));

    public string Illustration => this.M.ReadStringU(this.M.Read<long>(this.Address + 32L));

    public string PageArt => this.M.ReadStringU(this.M.Read<long>(this.Address + 40L));

    public string Description => this.M.ReadStringU(this.M.Read<long>(this.Address + 48L));

    public override string ToString() => this.Name;
  }
}
