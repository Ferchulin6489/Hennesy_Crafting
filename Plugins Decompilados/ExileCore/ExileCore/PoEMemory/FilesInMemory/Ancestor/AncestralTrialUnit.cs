// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Ancestor.AncestralTrialUnit
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory.Ancestor
{
  public class AncestralTrialUnit : RemoteMemoryObject
  {
    private string _id;
    private string _name;
    private string _description;

    public string Id => this._id ?? (this._id = this.M.ReadStringU(this.M.Read<long>(this.Address)));

    public string Name => this._name ?? (this._name = this.M.ReadStringU(this.M.Read<long>(this.Address + 8L)));

    public string Description => this._description ?? (this._description = this.M.ReadStringU(this.M.Read<long>(this.Address + 56L), 5120));

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
      interpolatedStringHandler.AppendFormatted(this.Id);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
