// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BestiaryGenus
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BestiaryGenus : RemoteMemoryObject
  {
    private BestiaryGroup bestiaryGroup;
    private string genusId;
    private string icon;
    private string name;
    private string name2;

    public int Id { get; internal set; }

    public string GenusId => this.genusId == null ? (this.genusId = this.M.ReadStringU(this.M.Read<long>(this.Address))) : this.genusId;

    public string Name => this.name == null ? (this.name = this.M.ReadStringU(this.M.Read<long>(this.Address + 8L))) : this.name;

    public BestiaryGroup BestiaryGroup => this.bestiaryGroup == null ? (this.bestiaryGroup = this.TheGame.Files.BestiaryGroups.GetByAddress(this.M.Read<long>(this.Address + 24L))) : this.bestiaryGroup;

    public string Name2 => this.name2 == null ? (this.name2 = this.M.ReadStringU(this.M.Read<long>(this.Address + 32L))) : this.name2;

    public string Icon => this.icon == null ? (this.icon = this.M.ReadStringU(this.M.Read<long>(this.Address + 40L))) : this.icon;

    public int MaxInStorage => this.M.Read<int>(this.Address + 48L);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 2);
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(", MaxInStorage: ");
      interpolatedStringHandler.AppendFormatted<int>(this.MaxInStorage);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
