// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.BlightTowerDat
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class BlightTowerDat : RemoteMemoryObject
  {
    private string _id;
    private string _name;
    private string _description;
    private string _icon;
    private int? _radius;

    public string Id => this._id ?? (this._id = this.M.ReadStringU(this.M.Read<long>(this.Address)));

    public string Name => this._name ?? (this._name = this.M.ReadStringU(this.M.Read<long>(this.Address + 8L)));

    public string Description => this._description ?? (this._description = this.M.ReadStringU(this.M.Read<long>(this.Address + 16L)));

    public string Icon => this._icon ?? (this._icon = this.M.ReadStringU(this.M.Read<long>(this.Address + 24L)));

    public int Radius
    {
      get
      {
        int valueOrDefault = this._radius.GetValueOrDefault();
        if (this._radius.HasValue)
          return valueOrDefault;
        int radius = this.M.Read<int>(this.Address + 60L);
        this._radius = new int?(radius);
        return radius;
      }
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
      interpolatedStringHandler.AppendFormatted(this.Id);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(" (");
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
