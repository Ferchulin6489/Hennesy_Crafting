// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Quest
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class Quest : RemoteMemoryObject
  {
    private string id;
    private string name;

    public string Id => this.id == null ? (this.id = this.M.ReadStringU(this.M.Read<long>(this.Address), (int) byte.MaxValue)) : this.id;

    public int Act => this.M.Read<int>(this.Address + 8L);

    public string Name => this.name == null ? (this.name = this.M.ReadStringU(this.M.Read<long>(this.Address + 12L))) : this.name;

    public string Icon => this.M.ReadStringU(this.M.Read<long>(this.Address + 24L));

    public override string ToString() => "Id: " + this.Id + ", Name: " + this.Name;
  }
}
