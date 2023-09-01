// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.LakeRoom
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class LakeRoom : RemoteMemoryObject
  {
    private string _id;
    private string _description;

    public string Id => this._id ?? (this._id = this.M.ReadStringU(this.M.Read<long>(this.Address)));

    public string Description => this._description ?? (this._description = this.M.ReadStringU(this.M.Read<long>(this.Address + 80L)));

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
      interpolatedStringHandler.AppendFormatted(this.Id);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted(this.Description);
      interpolatedStringHandler.AppendLiteral(" (");
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
