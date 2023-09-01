// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.ConditionalCache`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Cache
{
  public class ConditionalCache<T> : CachedValue<T>
  {
    private readonly Func<bool> _cond;

    public ConditionalCache(Func<T> func, Func<bool> cond)
      : base(func)
    {
      this._cond = cond;
    }

    protected override bool Update(bool force) => this._cond() | force;
  }
}
