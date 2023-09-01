// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.AreaCache`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Cache
{
  public class AreaCache<T> : CachedValue<T>
  {
    private uint _areaHash;

    public AreaCache(Func<T> func)
      : base(func)
    {
      this._areaHash = uint.MaxValue;
    }

    protected override bool Update(bool force)
    {
      if (!((int) this._areaHash != (int) AreaInstance.CurrentHash | force))
        return false;
      this._areaHash = AreaInstance.CurrentHash;
      return true;
    }
  }
}
