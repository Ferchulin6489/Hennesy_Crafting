// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.StaticCache`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ExileCore.Shared.Cache
{
  public class StaticCache<T> : IStaticCache<T>, IStaticCache
  {
    private static readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
    private readonly int _lifeTimeForCache;
    private readonly string name;
    private readonly CacheItemPolicy _policy;
    private readonly MemoryCache cache;
    private bool IsEmpty = true;

    public StaticCache(int lifeTimeForCache = 120, int limit = 30, string name = null)
    {
      this._lifeTimeForCache = lifeTimeForCache;
      this.name = name ?? typeof (T).Name;
      this.cache = new MemoryCache(this.name);
      this._policy = new CacheItemPolicy()
      {
        SlidingExpiration = TimeSpan.FromSeconds((double) lifeTimeForCache),
        RemovedCallback = (CacheEntryRemovedCallback) (arguments => ++this.DeletedCache)
      };
    }

    public void UpdateCache()
    {
      if (this.IsEmpty)
        return;
      this.cache.Trim(100);
      this.IsEmpty = true;
    }

    public int Count => this.ReadMemory - this.DeletedCache;

    public int DeletedCache { get; private set; }

    public int ReadCache { get; private set; }

    public int ReadMemory { get; private set; }

    public string CoeffString
    {
      get
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
        interpolatedStringHandler.AppendFormatted<float>(this.Coeff, "0.000");
        interpolatedStringHandler.AppendLiteral("% Read from memory");
        return interpolatedStringHandler.ToStringAndClear();
      }
    }

    public float Coeff => (float) ((double) this.ReadMemory / (double) (this.ReadCache + this.ReadMemory) * 100.0);

    public T Read(string addr, Func<T> func)
    {
      StaticCache<T>.cacheLock.EnterReadLock();
      try
      {
        this.IsEmpty = false;
        object obj = this.cache[addr];
        if (obj != null)
        {
          ++this.ReadCache;
          return (T) obj;
        }
      }
      finally
      {
        StaticCache<T>.cacheLock.ExitReadLock();
      }
      StaticCache<T>.cacheLock.EnterUpgradeableReadLock();
      try
      {
        object obj1 = this.cache.Get(addr, (string) null);
        if (obj1 != null)
        {
          ++this.ReadCache;
          return (T) obj1;
        }
        try
        {
          StaticCache<T>.cacheLock.EnterWriteLock();
          T obj2 = func();
          ++this.ReadMemory;
          this.cache.Add(addr, (object) obj2, this._policy);
          return obj2;
        }
        finally
        {
          StaticCache<T>.cacheLock.ExitWriteLock();
        }
      }
      finally
      {
        StaticCache<T>.cacheLock.ExitUpgradeableReadLock();
      }
    }

    public bool Remove(string key) => this.cache.Remove(key, (string) null) != null;
  }
}
