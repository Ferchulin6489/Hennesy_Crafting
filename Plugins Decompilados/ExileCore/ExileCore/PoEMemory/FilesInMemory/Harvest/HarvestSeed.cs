// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Harvest.HarvestSeed
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory.Harvest
{
  public class HarvestSeed : RemoteMemoryObject
  {
    private string _id;
    private int? _tier;
    private int? _type;

    public string Id => this._id ?? (this._id = this.M.ReadStringU(this.M.Read<long>(this.Address)));

    public int Tier
    {
      get
      {
        int valueOrDefault = this._tier.GetValueOrDefault();
        if (this._tier.HasValue)
          return valueOrDefault;
        int tier = this.M.Read<int>(this.Address + 24L);
        this._tier = new int?(tier);
        return tier;
      }
    }

    public int Type
    {
      get
      {
        int valueOrDefault = this._type.GetValueOrDefault();
        if (this._type.HasValue)
          return valueOrDefault;
        int type = this.M.Read<int>(this.Address + 84L);
        this._type = new int?(type);
        return type;
      }
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 4);
      interpolatedStringHandler.AppendFormatted(this.Id);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted<int>(this.Tier);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted<int>(this.Type);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
