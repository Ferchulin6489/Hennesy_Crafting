// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.StaticValueCache`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Cache
{
  public class StaticValueCache<T> : CachedValue<T>
  {
    private bool first;

    public StaticValueCache(Func<T> func)
      : base(func)
    {
      this.first = true;
    }

    protected override bool Update(bool force)
    {
      if (!this.first)
        return false;
      this.first = false;
      return true;
    }
  }
}
