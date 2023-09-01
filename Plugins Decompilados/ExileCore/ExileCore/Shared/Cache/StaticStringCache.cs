// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.StaticStringCache
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared.Cache
{
  public class StaticStringCache
  {
    private readonly ConcurrentDictionary<IntPtr, DateTime> _lastAccess = new ConcurrentDictionary<IntPtr, DateTime>();
    private readonly int _lifeTimeForCache;
    private DateTime lastClear;
    private readonly object locker = new object();

    public StaticStringCache(int LifeTimeForCache = 300) => this._lifeTimeForCache = LifeTimeForCache;

    public Dictionary<IntPtr, string> Debug { get; } = new Dictionary<IntPtr, string>();

    public int Count => this.Debug.Count;

    public int ClearByTime()
    {
      int num = 0;
      if ((DateTime.UtcNow - this.lastClear).TotalSeconds < 60.0)
        return num;
      foreach (KeyValuePair<IntPtr, DateTime> keyValuePair in this._lastAccess)
      {
        if ((DateTime.UtcNow - keyValuePair.Value).TotalSeconds > (double) this._lifeTimeForCache && this.Debug.Remove(keyValuePair.Key))
        {
          ++num;
          this._lastAccess.TryRemove(keyValuePair.Key, out DateTime _);
        }
      }
      if (this._lastAccess.Count > 30000)
      {
        this._lastAccess.Clear();
        this.Debug.Clear();
        DebugWindow.LogMsg("Clear CACHE because so big (>30k)", 7f, Color.GreenYellow);
      }
      this.lastClear = DateTime.UtcNow;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
      interpolatedStringHandler.AppendLiteral("StaticStringCache Cleared by time: ");
      interpolatedStringHandler.AppendFormatted<int>(num);
      interpolatedStringHandler.AppendLiteral(" [");
      interpolatedStringHandler.AppendFormatted<DateTime>(this.lastClear);
      interpolatedStringHandler.AppendLiteral("]");
      DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 7f, Color.Yellow);
      return num;
    }

    public string Read(IntPtr addr, Func<string> func)
    {
      string str1;
      if (this.Debug.TryGetValue(addr, out str1))
      {
        this._lastAccess[addr] = DateTime.UtcNow;
        return str1;
      }
      string str2 = func();
      lock (this.locker)
        this.Debug[addr] = str2;
      this._lastAccess[addr] = DateTime.UtcNow;
      return str2;
    }
  }
}
