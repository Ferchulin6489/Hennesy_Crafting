// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.LatancyCache`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Cache
{
  public class LatancyCache<T> : CachedValue<T>
  {
    private readonly int _minLatency;
    private long _checkTime;

    public LatancyCache(Func<T> func, int minLatency = 10)
      : base(func)
    {
      this._minLatency = minLatency;
      this._checkTime = long.MinValue;
    }

    protected override bool Update(bool force)
    {
      float latency = CachedValue.Latency;
      long elapsedMilliseconds = CachedValue<T>.sw.ElapsedMilliseconds;
      if (!(elapsedMilliseconds >= this._checkTime | force))
        return false;
      this._checkTime = (double) latency <= (double) this._minLatency ? elapsedMilliseconds + (long) this._minLatency : (long) ((double) elapsedMilliseconds + (double) latency);
      return true;
    }
  }
}
