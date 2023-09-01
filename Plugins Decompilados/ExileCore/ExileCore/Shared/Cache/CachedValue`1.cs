// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.CachedValue`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Diagnostics;
using System.Threading;

namespace ExileCore.Shared.Cache
{
  public abstract class CachedValue<T> : CachedValue
  {
    protected static Stopwatch sw = Stopwatch.StartNew();
    private readonly Func<T> _func;
    private bool _force;
    private T _value;
    private bool _updated;

    protected CachedValue(Func<T> func)
    {
      this._func = func ?? throw new ArgumentNullException(nameof (func), "Cached Value ctor null function");
      Interlocked.Increment(ref CachedValue.TotalCount);
      Interlocked.Increment(ref CachedValue.LifeCount);
    }

    public T Value
    {
      get
      {
        if (this.Update(this._force))
        {
          this._force = false;
          this._value = this._func();
          CachedValue<T>.CacheUpdateEvent onUpdate = this.OnUpdate;
          if (onUpdate != null)
            onUpdate(this._value);
          this._updated = true;
          return this._value;
        }
        return !this._updated ? this._func() : this._value;
      }
    }

    public T RealValue => this._func();

    public event CachedValue<T>.CacheUpdateEvent OnUpdate;

    public void ForceUpdate() => this._force = true;

    protected abstract bool Update(bool force);

    ~CachedValue() => Interlocked.Decrement(ref CachedValue.LifeCount);

    public delegate void CacheUpdateEvent(T t);
  }
}
