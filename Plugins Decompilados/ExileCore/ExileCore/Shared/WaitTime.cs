// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.WaitTime
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections;

namespace ExileCore.Shared
{
  public class WaitTime : YieldBase
  {
    public WaitTime(int milliseconds)
    {
      this.Milliseconds = milliseconds;
      this.Current = (object) this.GetEnumerator();
    }

    public int Milliseconds { get; }

    public override sealed IEnumerator GetEnumerator()
    {
      TimeSpan elapsed = YieldBase.sw.Elapsed;
      double wait = elapsed.TotalMilliseconds + (double) this.Milliseconds;
      while (true)
      {
        elapsed = YieldBase.sw.Elapsed;
        if (elapsed.TotalMilliseconds < wait)
          yield return (object) null;
        else
          break;
      }
      yield return YieldBase.RealWork;
    }
  }
}
