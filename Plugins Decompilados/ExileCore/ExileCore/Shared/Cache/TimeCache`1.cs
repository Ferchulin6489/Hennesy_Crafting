// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.TimeCache`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Cache
{
  public class TimeCache<T> : CachedValue<T>
  {
    private long _waitMilliseconds;
    private long time;

    public TimeCache(Func<T> func, long waitMilliseconds)
      : base(func)
    {
      this.time = long.MinValue;
      this._waitMilliseconds = waitMilliseconds;
    }

    public void NewTime(long newTime)
    {
      this._waitMilliseconds = newTime;
      this.time = this._waitMilliseconds + CachedValue<T>.sw.ElapsedMilliseconds;
    }

    protected override bool Update(bool force)
    {
      long elapsedMilliseconds = CachedValue<T>.sw.ElapsedMilliseconds;
      if (!(elapsedMilliseconds >= this.time | force))
        return false;
      this.time = elapsedMilliseconds + this._waitMilliseconds;
      return true;
    }
  }
}
