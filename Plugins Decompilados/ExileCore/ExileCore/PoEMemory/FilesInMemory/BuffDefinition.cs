// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.BuffDefinition
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class BuffDefinition : RemoteMemoryObject
  {
    private string _id;
    private string _description;
    private BuffVisual _buffVisual;
    private string _name;
    private bool? _isInvisible;
    private bool? _isRemovable;

    public string Id => this._id ?? (this._id = this.M.ReadStringU(this.M.Read<long>(this.Address)));

    public string Description => this._description ?? (this._description = this.M.ReadStringU(this.M.Read<long>(this.Address + 8L)));

    public bool IsInvisible
    {
      get
      {
        bool valueOrDefault = this._isInvisible.GetValueOrDefault();
        if (this._isInvisible.HasValue)
          return valueOrDefault;
        bool isInvisible = this.M.Read<bool>(this.Address + 16L);
        this._isInvisible = new bool?(isInvisible);
        return isInvisible;
      }
    }

    public bool IsRemovable
    {
      get
      {
        bool valueOrDefault = this._isRemovable.GetValueOrDefault();
        if (this._isRemovable.HasValue)
          return valueOrDefault;
        bool isRemovable = this.M.Read<bool>(this.Address + 17L);
        this._isRemovable = new bool?(isRemovable);
        return isRemovable;
      }
    }

    public string Name => this._name ?? (this._name = this.M.ReadStringU(this.M.Read<long>(this.Address + 18L)));

    public BuffVisual BuffVisual => this._buffVisual ?? (this._buffVisual = this.TheGame.Files.BuffVisuals.GetByAddress(this.M.Read<long>(this.Address + 85L)));

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 4);
      interpolatedStringHandler.AppendFormatted(this.Id);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted(this.BuffVisual?.DdsFile);
      interpolatedStringHandler.AppendLiteral(" (");
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
