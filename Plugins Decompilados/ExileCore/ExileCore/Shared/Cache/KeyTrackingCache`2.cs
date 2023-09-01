// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.KeyTrackingCache`2
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;

namespace ExileCore.Shared.Cache
{
  public class KeyTrackingCache<T, TKey> : CachedValue<T>
  {
    private readonly Func<TKey> _keyFunc;
    private TKey _lastKey;
    private bool _first;

    public KeyTrackingCache(Func<T> func, Func<TKey> keyFunc)
      : base(func)
    {
      this._keyFunc = keyFunc;
      this._first = true;
    }

    protected override bool Update(bool force)
    {
      TKey x = this._keyFunc();
      int num = this._first ? 1 : (!EqualityComparer<TKey>.Default.Equals(x, this._lastKey) ? 1 : 0);
      this._lastKey = x;
      this._first = false;
      return num != 0;
    }
  }
}
